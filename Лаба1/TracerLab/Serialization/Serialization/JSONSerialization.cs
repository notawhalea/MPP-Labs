using Newtonsoft.Json;

namespace Serialization
{
    internal class JSONSerialization: ISerializer
    {
        public string Serialize(TraceResult traceResult)
        {
            var arrays = new Dictionary<string, ICollection<ThreadTrace>>
            {
                {"threads", traceResult.GetThreadTraces().Values}
            };

            return JsonConvert.SerializeObject(arrays, Formatting.Indented);
        }
    }
}
