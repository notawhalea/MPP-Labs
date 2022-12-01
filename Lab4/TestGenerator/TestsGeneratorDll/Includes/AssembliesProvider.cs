using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace TestsGeneratorDll.Includes
{
    // класс - поставщик сборок
    internal class AssembliesProvider
    {
        int runningCount;
        object sync = new object(); // объект заглушка

        List<string> filePaths;
        List<Assembly> filesAssemblies = new List<Assembly>();

        public List<Assembly> GetAssemblies(List<string> filePaths, int restriction)
        {
            this.filePaths = filePaths;
            //Делегат Func часто используется как параметр в методах.
            //Внимание! Последний параметр у Func это return тип.

            //вся эта херня нужна чтобы ссылаться на метод GetAssembly(string filePath) - типо указатель на функцию

            Func<string, Assembly> functionToExacute = GetAssembly;
            GetAssembliesByUsingMonitor(functionToExacute, restriction);

            return filesAssemblies;

        }

        //public void GetAssembliesByUsingMonitor(List<Func<string, Assembly>> functions, int restriction)
        public void GetAssembliesByUsingMonitor(Func<string, Assembly> function, int restriction)
        {
            object param;

            runningCount = filePaths.Count;
            //runningCount = functions.Count;
            //первый параметр - Максимальное количество рабочих потоков в пуле потоков.
            //второй параметр - Максимальное количество потоков асинхронного ввода-вывода в пуле потоков.
            ThreadPool.SetMaxThreads(restriction, restriction);

            for (int i = 0; i < filePaths.Count; i++) 
            {
                param = new List<object>() { function, i };
                //Помещает метод в очередь на выполнение и указывает объект, содержащий данные для использования методом.
                //Метод выполняется, когда становится доступен поток из пула потоков.
                ThreadPool.QueueUserWorkItem(AddAssemblyAction, param);
            }

            //заглушка - описана чуть ниже ↓
            // плюс доп статья https://professorweb.ru/my/csharp/thread_and_files/1/1_10.php

            //поток переходит в состояние ожидания, а блокировка с соответствующего объекта снимается,
            //что дает возможность использовать этот объект в другом потоке.
            lock (sync)
                if (runningCount > 0)
                    Monitor.Wait(sync);
            // Wait освобождает блокировку объекта и переводит поток в очередь ожидания объекта.
            //Следующий поток в очереди готовности объекта блокирует данный объект. А все потоки, которые вызвали
            //метод Wait, остаются в очереди ожидания, пока не получат сигнала от метода Monitor.Pulse или Monitor.PulseAll,
            //посланного владельцем блокировки.
        }

        private void AddAssemblyAction(object state)
        {
            List<object> parameters = (List<object>)state;

            //Делегат function = GetAssembly
            var function = (Func<string, Assembly>)parameters[0];
            //достать из List по индексу нужный путь к файлу
            string filePath = filePaths[(int)parameters[1]];

            // Здесь происходит вызов функции GetAssembly
            Assembly assembly = function(filePath);

            //чтобы синхронизировать потоки и ограничить доступ к разделяемым ресурсам на время их использования
            //каким-нибудь потоком. Для этого используется ключевое слово lock. Оператор lock определяет блок кода,
            //внутри которого весь код блокируется и становится недоступным для других потоков до завершения работы
            //текущего потока. Остальный потоки помещаются в очередь ожидания и ждут, пока текущий поток не освободит
            //данный блок кода.

            // это означает что доступ к этому блоку имеет только один поток
            // После окончания работы блока кода, объект locker освобождается и становится доступным для других потоков.
            lock (sync)
            {
                filesAssemblies.Add(assembly);

                runningCount--;

                if (runningCount == 0)
                    Monitor.Pulse(sync); // Pulse уведомляет главный поток из очереди ожидания, что текущий(текущие) поток(и)
                                         // отработал(и) и освободил(и) объект obj
            }
        }


        // Способ получения сборки из пути к файлу
        private Assembly GetAssembly(string filePath)
        {
            return Assembly.LoadFile(filePath);
        }
    }
}
