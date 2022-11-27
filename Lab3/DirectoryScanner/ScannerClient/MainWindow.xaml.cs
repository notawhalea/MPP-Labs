using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Win32;



namespace ScannerClient
{

    public partial class MainWindow : Window
    {

        private Uri folderUri, textFileUri, normalFileUri;

        public MainWindow()
        {
            string str = Environment.CurrentDirectory;
            InitializeComponent();

            // URIs for selecting icons
            folderUri = new Uri($@"{Environment.CurrentDirectory}\..\..\..\Images\folder.png");
            normalFileUri = new Uri($@"{Environment.CurrentDirectory}\..\..\..\Images\file.png");
            textFileUri = new Uri($@"{Environment.CurrentDirectory}\..\..\..\Images\newTxt2.png");
        }

        private async void StartScan_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog folderBrowser = new OpenFileDialog();

            folderBrowser.ValidateNames = false;
            folderBrowser.CheckFileExists = false;
            folderBrowser.CheckPathExists = true;
            folderBrowser.FileName = "Folder Selection.";

            string folderPath = null;
            if (folderBrowser.ShowDialog() == true)
            {
                folderPath = Path.GetDirectoryName(folderBrowser.FileName);
            }

            if (folderPath == null)
            {
                MessageBox.Show("Error. Folder path is null!");
                return;
            }

            var task = Task.Run(() => TaskForAnAsyncOperation(folderPath));
            //запуск асинхронной задачи (до этого момента все выполянется синхронно)
            var entities = await task;


            TreeView treeView = GenerateTreeViewFromTheEntities(null, entities, 0, null);

            // очистка раннее анализируемой библиотеки
            if (DirectoryTreeView.Children.Count > 0)
                DirectoryTreeView.Children.RemoveAt(0);

            // отобразить дерево
            DirectoryTreeView.Children.Add(treeView);
        }

        private void StopScan_Click(object sender, RoutedEventArgs e)
        {
            DirectoryScanner.DirScanner.StopProcessing();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private TreeView GenerateTreeViewFromTheEntities(TreeViewItem treeItem, List<DirectoryScanner.Entity> entities, int index, DirectoryInfo subDir)
        {
            TreeView treeView = null;

            // Установить метод, выполняющийся в качестве основного для потока
            // (необходимо для предотвращения доступа к данным этого метода из других потоков)
            Application.Current.Dispatcher.Invoke(() =>
            {
                treeView = new TreeView(); 
                TreeViewItem tempItem = new TreeViewItem(); // для хранения данных в while цикле

                while (index <= entities.Count - 1)
                {
                    // Если мы работаем с головным каталогом (subdir равен null) или мы имеем дело с любым файлом,
                    // который находится внутри предыдущего каталога
                    if (index == 0 || entities[index].SubDirecory.FullName == subDir?.FullName)
                    {
                        // Получение расширения файла и общего размера в процентах
                        string extension = entities[index].Type == DirectoryScanner.EntityType.File ? "(file)" : entities[index].Type == DirectoryScanner.EntityType.Directory ? "(dir)" : "(txt)";
                        string persantage = entities[index].Persantage == String.Empty ? "" : $", {entities[index].Persantage}";

                        var currentTreeItem = new TreeViewItem(); // текущий entity treeViewItem

                        // объект stackpanel для хранения информации о сущности с помощью значка
                        // располагает элементы в ряд по горизонтали
                        StackPanel stackPanel = new StackPanel() { Orientation = Orientation.Horizontal };

                        // Файловая информация (имя и расширение (для папки просто имя) + размер файла(директории))
                        TextBlock textBlock = new TextBlock() { Text = extension + $" {entities[index].Name} ({entities[index].Size} байт{persantage})" };

                        // Установка значка в зависимости от типа объекта (file, dir или txt).
                        string path = entities[index].Type == DirectoryScanner.EntityType.File ? normalFileUri.ToString() : entities[index].Type == DirectoryScanner.EntityType.Directory ? folderUri.ToString() : textFileUri.ToString();
                        Uri uri = new Uri(path);
                        var image = new Image() { Source = new BitmapImage(uri) };

                        stackPanel.Children.Add(image);
                        stackPanel.Children.Add(textBlock);

                        // Добавить все собранные данные о файле в заголовок TreeViewItem
                        currentTreeItem.Header = stackPanel;

                        if (treeItem == null) // Если мы имеем дело с головным каталогом -> добавить TreeViewItem в главное дерево
                        {
                            treeView.Items.Add(currentTreeItem);
                            treeItem = currentTreeItem;
                        }
                        else // Если мы имеем дело не с головным каталогом -> добавьте TreeViewItem в качестве дочернего элемента к предыдущему TreeViewItem
                        {
                            treeItem.Items.Add(currentTreeItem);
                        }

                        tempItem = currentTreeItem; // Сохранить ссылку на текущий элемент в переменной temp

                        index += 1; // увеличения индекса для выбора новой сущности

                        continue;
                    }
                    else // Если это не головной каталог, и мы ничего не знаем о текущем родительском файле, и он не связан с предыдущим элементом
                    {
                        // если мы работаем с файлами, которые находятся внутри предыдущего каталога файлов
                        if (subDir == null || entities[index].SubDirecory.FullName.Contains(entities[index - 1].SubDirecory.FullName))
                        {
                            treeItem = tempItem; // установка текущего элемента дерева в качестве текущей папки
                            subDir = entities[index].SubDirecory; // Изменить поддиректорию на текущий каталог файла
                            continue;
                        }
                        else // файл в предыдущем каталоге (не в текущем)
                        {
                            subDir = subDir.Parent; // установить поддиректорию как родительскую директорию
                            treeItem = (TreeViewItem)treeItem.Parent; // установка текущего TreeItem в качестве родительского элемента, потому что мы сделали шаг назад
                            continue;
                        }
                    }
                }

                return treeView;
            });

            if (treeView == null)
                throw new Exception("Error. Tree was not generated");

            return treeView;
        }

        private Task<List<DirectoryScanner.Entity>> TaskForAnAsyncOperation(string folderPath)
        {
            var entities = DirectoryScanner.DirScanner.Scan(folderPath);

            return Task.FromResult<List<DirectoryScanner.Entity>>(entities);
        }
    }
}
