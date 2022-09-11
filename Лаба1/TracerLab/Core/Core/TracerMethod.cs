using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace TracerLab.Core
{
    public class MethodInfo
    {
        [JsonProperty, XmlAttribute("name")] public string MethodName { get; set; }
        [JsonProperty, XmlAttribute("class")] public string ClassName { get; set; }
        [JsonProperty, XmlAttribute("time")] public double Time { get; set; }

        [JsonProperty, XmlElement("methods")]
        //[JsonIgnore]
        public List<MethodInfo> ChildMethods { get; set; }

        [JsonIgnore]
        private readonly string _allMethodPath;

        [JsonIgnore] private readonly Stopwatch _stopwatch = new Stopwatch();

        public MethodInfo(string methodName, string className, string allMethodPath)
        {
            MethodName = methodName;
            ClassName = className;
            _allMethodPath = allMethodPath;
            _stopwatch.Start();
        }
        public MethodInfo() { }

        public void SetChildMethods(List<MethodInfo> childMethods)
        {
            ChildMethods = childMethods;
        }
        public string GetAllMethodPath()
        {
            return _allMethodPath;
        }

        public void CalculateTime()
        {
            _stopwatch.Stop();
            Time = _stopwatch.ElapsedMilliseconds;
        }

        public long GetTime()
        {
            return _stopwatch.ElapsedMilliseconds;
        }
    }
}
