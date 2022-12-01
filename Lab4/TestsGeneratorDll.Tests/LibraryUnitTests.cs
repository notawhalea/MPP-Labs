namespace TestsGeneratorDll.Tests
{
    public class LibraryUnitTests
    {
        public LibraryUnitTests()
        {
            List<string> files = new List<string>()
            {
                @"D:\�����\3 ����\5 �������\���\������������\����4\TestsGenerator-main\TestsGenerator-main\TestsGeneratorConsole\bin\Debug\TestsGeneratorConsole.exe",
                @"D:\�����\3 ����\5 �������\���\������������\����4\TestsGenerator-main\TestsGenerator-main\TestsGeneratorDll\bin\Debug\TestsGeneratorDll.dll"
            };
            TestsGenerator.GenerateXUnitTests(files, @"D:\�����\3 ����\5 �������\���\������������\����4\TestsGenerator-main\ResultsLab4", 10);
        }

        [Fact]
        public void GenerateTests_WithSpecificFiles_ReturnRightNumberOfTestClassesGenerated()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(@"D:\�����\3 ����\5 �������\���\������������\����4\TestsGenerator-main\ResultsLab4");

            int filesCount = directoryInfo.GetFiles().Length;

            Assert.Equal(6, filesCount);
        }


        [Fact]
        public void GenerateTests_WithSpecificFiles_ReturnNotEmptyGeneratedFiles()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(@"D:\�����\3 ����\5 �������\���\������������\����4\TestsGenerator-main\ResultsLab4");

            var files = directoryInfo.GetFiles();

            bool isAnyFileEmpty = false;

            foreach (var file in files)
            {
                if (file.Length == 0)
                {
                    isAnyFileEmpty = true;
                    break;
                }
            }
            Assert.False(isAnyFileEmpty);
        }
    }
}