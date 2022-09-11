using System;
using System.IO;

namespace Tracer.Example
{
    public static class PathHolder
    {
        public static string JsonPath { get; private set; }

        public static string XmlPath { get; private set; }

        static PathHolder()
        {

            var projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            var dataPath = Path.Combine(projectDirectory, "Files");

            JsonPath = Path.GetFullPath(Path.Combine(dataPath, "json.json"));
            XmlPath = Path.GetFullPath(Path.Combine(dataPath, "test.xml"));

        }
    }
}
