using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracer.Example
{
    internal class FilePrinter : IPrinter
    {
        private string FilePath;

        public FilePrinter(string filePath)
        {
            FilePath = filePath;
        }

        public void PrintResult(string data)
        {
            try
            {
                using (FileStream fstream = new FileStream(FilePath, FileMode.OpenOrCreate))
                {
                    byte[] array = System.Text.Encoding.Default.GetBytes(data);
                    fstream.Write(array, 0, array.Length);
                }
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
