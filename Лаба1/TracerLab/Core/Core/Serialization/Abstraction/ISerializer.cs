using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serialization
{
    internal interface ISerializer
    {
        string Serialize(TraceResult traceResult);
    }
}
