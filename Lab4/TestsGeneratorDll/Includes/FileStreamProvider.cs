using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestsGeneratorDll.Includes
{
    internal class FileStreamProvider
    {
        object syn = new object();
        int activeTasks = 0;
        int restriction;

        public void SaveTestsIntoTheFiles(List<string> xUnitTests, string savePath, int restriction)
        {
            this.restriction = restriction;

            int counter = 1;
            foreach (var xUnitTest in xUnitTests)
            {
                lock (syn)
                    if (activeTasks == restriction)
                        Monitor.Wait(syn);

                Task asyncTask = WriteFileAsync(savePath, $"Test_TheNightWeMet{counter}.cs", xUnitTest);
                counter++;
            }

            lock (syn)
                if (activeTasks != 0)
                    Monitor.Wait(syn);
        }

        private async Task WriteFileAsync(string savePath, string file, string content)
        {
            lock (syn)
                activeTasks += 1;

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(savePath, file)))
            {

                await outputFile.WriteAsync(content);
            }

            lock (syn)
            {
                activeTasks--;

                if (activeTasks == restriction - 1)
                    Monitor.Pulse(syn);

                if (activeTasks == 0)
                    Monitor.Pulse(syn);
            }
        }
    }
}
