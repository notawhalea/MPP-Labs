using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormatterConsole.Interfaces
{
    internal interface IStringFormatter
    {
        string Format(string template, params object[] parameters);
    }
}
