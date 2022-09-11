using System.Xml.Serialization;
using System.IO;
using System.Linq;
using System.Xml;
using Formatting = System.Xml.Formatting;

namespace Serialization
{
    internal class XMLSerialization: ISerializer
    {
        public string Serialize(TraceResult traceResult)
        {
            var data = traceResult.GetThreadTraces().Values.ToArray();
            var xmlSerializer = new XmlSerializer(data.GetType());
            var stringWriter = new StringWriter();

            using (var writer = new XmlTextWriter(stringWriter))
            {
                writer.Formatting = Formatting.Indented;
                xmlSerializer.Serialize(writer, data);
            }

            var result = stringWriter.ToString().Replace("ArrayOfThread", "root");

            return result;
        }
    }
}

