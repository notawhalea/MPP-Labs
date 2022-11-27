using System.Diagnostics;
using System.IO;


namespace DirectoryScanner
{
    public enum EntityType
    {
        Directory = 1,
        File = 2,
        TextFile = 3
    }

    public class Entity
    {
        public FileSystemInfo Info { get; set; } // ��������� ���������� � ��������
        public string Name { get; set; } // ���
        public EntityType Type { get; set; } // ��� ��������
        public DirectoryInfo SubDirecory { get; set; } // �������, � ������� ���������� �������� (null ��� ��������)
        public long? Size { get; set; } = null; // ������ (� ������)
        public string? Persantage { get; set; } = null; // ������ (� ��������� �� ����� ����������� ��������)

        public Entity() { }
    }


    public class DirScanner
    {
        public static bool isWorking;

        public static List<Entity> Scan(string filePath)
        {
            if (filePath == null || !Directory.Exists(filePath))
                throw new Exception("Error. Directory does not exist.");

            isWorking = true;


            // ������ ���� ��������� (������ � ����������)
            List<Entity> entities = new List<Entity>();

            //�������� �������� ������� ��� ������ ��������
            string directoryPath = filePath;
            DirectoryInfo headDirectory = new DirectoryInfo(directoryPath);

            // �������� �������� ������� ��� ��������
            entities.Add(CreateEntityFromDirectory(headDirectory, isHeadDirectory: true));

            //�������� ��������� ��� ����� � ���������� (������� �������)
            FileSystemInfo[] filesAndDirectories_HeadDirectory = headDirectory.GetFileSystemInfos();

            foreach (var item in filesAndDirectories_HeadDirectory)
            {
                if (item.GetType().Name == "FileInfo")
                    entities.Add(CreateEntityFromFile((FileInfo)item));
                else
                    GetDirectoryIerarchy(entities, (DirectoryInfo)item); // ���� ��� ���������� -> �������� �������� ���� ����������
            }

            // �������� ��� ��������� ������, ��������� ��� �������������
            ThreadPool.GetAvailableThreads(out int systemThreadsCount, out _);

            // ����������� ������� �������� ���� ������
            CalculateSizeOfAllEntities(entities, numberOfThreadsToProceed: 10, numberOfSystemThreads: systemThreadsCount);

            // ���������� ��������� ���� ������
            var result = from entitiesWithSize in entities where entitiesWithSize.Size != null select entitiesWithSize;
            return result.ToList();
        }

        //������� �������� ��� ����������
        static Entity CreateEntityFromDirectory(DirectoryInfo dir, bool isHeadDirectory = false)
        {
            return new Entity
            {
                Info = dir,
                Name = dir.Name,
                Type = EntityType.Directory,
                SubDirecory = isHeadDirectory ? null : dir.Parent,
            };
        }

        //������� �������� ��� �����
        static Entity CreateEntityFromFile(FileInfo file)
        {
            return new Entity
            {
                Info = file,
                Name = file.Name,
                Type = file.Extension == ".txt" ? EntityType.TextFile : EntityType.File,
                SubDirecory = file.Directory,
            };
        }


        static void GetDirectoryIerarchy(List<Entity> entities, DirectoryInfo dir)
        {
            // �������� ���������� � entities
            entities.Add(CreateEntityFromDirectory(dir));

            // �������� ��� ����� � ����������� � ������ ����������
            var filesAndDirectories_SubDirectory = dir.GetFileSystemInfos();

            foreach (var item in filesAndDirectories_SubDirectory)
            {
                if (item.GetType().Name == "FileInfo")
                    entities.Add(CreateEntityFromFile((FileInfo)item));
                else
                    GetDirectoryIerarchy(entities, (DirectoryInfo)item); // ���� ��� ���������� -> �������� �������� ���� ����������
            }
        }


        // ������� ���� �������� ���������
        static void CalculateSizeOfAllEntities(List<Entity> entities, int numberOfThreadsToProceed = 0, int numberOfSystemThreads = 0)
        {

            foreach (var entity in entities)
            {
                if (!isWorking)
                    break;
                //�������� ����� � ������� �� ����������. ����� �����������, ����� ���������� �������� ����� �� ���� �������.
                ThreadPool.QueueUserWorkItem(TaskForAnAsyncCalculation, entity);

                while (true) // ������ �������� ����, ���� �� � ��� ���������(�� ������� ���� �����������) �����
                {
                    ThreadPool.GetAvailableThreads(out int currentAvailableThreads, out _); // �������� ��� ��������� ��������� ������ �� ������ ������

                    if (numberOfSystemThreads - currentAvailableThreads < numberOfThreadsToProceed)
                        break;
                    else
                        Thread.Sleep(10);
                }
            }

            // � ���� ����� ���������� ���������, ���� �� �����-�� ������� �������� ����� ����, ��� �� �������� ��������� �������� 
            while (true)
            {
                ThreadPool.GetAvailableThreads(out int currentAvailableThreadsCount, out _);
                if (currentAvailableThreadsCount != numberOfSystemThreads) // ���� ��� �������� ����������
                    Thread.Sleep(100);
                else
                    break;
            }
        }


        // �����, ������������ � ���� ������� ��� ������������ ���������� ����������
        static void TaskForAnAsyncCalculation(object entityObj)
        {
            Entity entity = (Entity)entityObj;
            if (entity.Type == EntityType.Directory)
            {
                DirectoryInfo dir = (DirectoryInfo)entity.Info;
                entity.Size = GetDirectorySize(dir);
                entity.Persantage = entity.SubDirecory == null ? String.Empty : (100 * (double)entity.Size / GetDirectorySize(dir.Parent)).ToString() + "%";
            }
            else
            {
                FileInfo file = (FileInfo)entity.Info;
                entity.Size = file.Length;
                entity.Persantage = (100 * (double)file.Length / GetDirectorySize(file.Directory)).ToString() + "%";
            }
        }

        // ����� ��� ���������� ������� ��������
        static long GetDirectorySize(DirectoryInfo dir)
        {
            long size = 0;

            FileInfo[] files = dir.GetFiles();
            DirectoryInfo[] directories = dir.GetDirectories();

            foreach (var file in files)
                size += file.Length;

            foreach (var directory in directories)
                size += GetDirectorySize(directory);

            return size;
        }


        public static void StopProcessing()
        {
            isWorking = false;
        }
    }
}