using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TestsGeneratorDll.Includes;

namespace TestsGeneratorDll
{
    public class TestsGenerator
    {
        public static void GenerateXUnitTests(List<string> filePaths, string savingPath, int restriction)
        {
            //restriction = ограничение
            AssembliesProvider assembliesProvider = new AssembliesProvider();
            MethodsProvider methodsProvider = new MethodsProvider();
            XUnitTestsProvider xUnitTestsProvider = new XUnitTestsProvider();
            FileStreamProvider fileStreamProvider = new FileStreamProvider();

            List<Assembly> assemblies = assembliesProvider.GetAssemblies(filePaths, restriction);
            List<MethodInfo> methodInfos = methodsProvider.GetMethodsInfo(assemblies);
            List<string> generatedTests = xUnitTestsProvider.GetXUnitTests(methodInfos, restriction);

            fileStreamProvider.SaveTestsIntoTheFiles(generatedTests, savingPath, restriction);
        }
    }
}
