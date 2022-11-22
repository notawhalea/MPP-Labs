using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracer.Example
{
    internal class ConsolePrinter: IPrinter
    {
        public void PrintResult(string data)
        {
            Console.WriteLine(data);
        }
    }
}
