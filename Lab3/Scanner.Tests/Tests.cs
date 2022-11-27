namespace Scanner.Tests
{
    public class Tests
    {
        [Fact]
        public void WorkingVariable_WhenStoppedGauging_EqualsFalse()
        {
            DirectoryScanner.DirScanner.StopProcessing();

            Assert.False(DirectoryScanner.DirScanner.isWorking);
        }

        [Fact]
        public void WorkingVariable_WhenStartedGauging_EqualsTrue()
        {
            DirectoryScanner.DirScanner.Scan("D:\\БГУИР\\3 курс\\5 семестр\\СПП\\Лабораторные\\Лаба3\\LaboratoryWork_DirectoryScanner-main\\Test");

            Assert.True(DirectoryScanner.DirScanner.isWorking);

            DirectoryScanner.DirScanner.StopProcessing();
        }

        [Fact]
        public void StartGauging_WhenDirectoryDoesNotExists_ThrowAnException()
        {
            string dirPath = "abcdefghij...wxyz";

            Assert.Throws<Exception>(() => DirectoryScanner.DirScanner.Scan(dirPath));
        }

        [Fact]
        public void StartGauging_WhenDirectoryPathEqualsNull_ThrowAnException()
        {
            string dirPath = null;

            Assert.Throws<Exception>(() => DirectoryScanner.DirScanner.Scan(dirPath));
        }

        // This test works only for basic folders. When we work with folder wich has git system inside, there will be 
        // a lot of hidden files too and this test will fail
        [Fact]
        public void Gauge_DirectoryWith3FilesInside_ReturnListWith3Entities()
        {
            string dirPath = "D:\\БГУИР\\3 курс\\5 семестр\\СПП\\Лабораторные\\Лаба3\\LaboratoryWork_DirectoryScanner-main\\Test";

            List<DirectoryScanner.Entity> entities = DirectoryScanner.DirScanner.Scan(dirPath);

            Assert.Equal(4, entities.Count); // 1 Head directory + 3 files inside = 4 entities total
        }
    }
}