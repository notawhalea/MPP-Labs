using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TestsGeneratorDll;

namespace TestsGeneratorConsole
{


    internal class Program
    {
        static void Main(string[] args)
        {

            List<string> files = new List<string>()
            {
                //пути файлов из которых мы берем классы для генерации тестовых классов(классы которые будут
                //сохраняться в файлы)
                @"D:\БГУИР\3 курс\5 семестр\СПП\Лабораторные\Лаба4\TestsGenerator-main\TestsGenerator-main\TestsGeneratorConsole\bin\Debug\TestsGeneratorConsole.exe",
                @"D:\БГУИР\3 курс\5 семестр\СПП\Лабораторные\Лаба4\TestsGenerator-main\TestsGenerator-main\TestsGeneratorDll\bin\Debug\TestsGeneratorDll.dll"
            };
            //Путь к папке для записи созданных файлов.
            // 10 - Ограничения на секции конвейера (ограничения количества файлов, загружаемых за раз)
            TestsGenerator.GenerateXUnitTests(files, @"D:\БГУИР\3 курс\5 семестр\СПП\Лабораторные\Лаба4\TestsGenerator-main\ResultsLab4", 10);
            
        }

    }

    public class Tests
    {
        static void TestOne()
        {
            Console.WriteLine(nameof(TestOne));
        }

        static void TestTwo()
        {
            Console.WriteLine(nameof(TestTwo));
        }
    }

    public class Bsuir
    {
        public class Sisharp
        {
            public class Teacher
            {
                static string HelloTeacher()
                {
                    Console.WriteLine("Hello");
                    return String.Empty;
                }
            }



            static string HelloStudents()
            {
                Console.WriteLine("Hello");
                return String.Empty;
            }
        }


        static string HelloWorld(string text_line)
        {
            Console.WriteLine("Hello");
            return String.Empty;
        }
    }



    public class MyClass
    {
        public void FirstMethod()
        {
            Console.WriteLine("First method");
        }

        public void SecondMethod()
        {
            Console.WriteLine("Second method");
        }

        public void ThirdMethod(int a)
        {
            Console.WriteLine("Third method (int)");
        }

        public void ThirdMethod(double a)
        {
            Console.WriteLine("Third method (double)");
        }
    }
}
