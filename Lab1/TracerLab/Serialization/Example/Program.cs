using Serialization;
using Tracer.Example;

namespace TracerLib
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var program = new MainClass();
            var thread = new Thread(program.Method);
            ITracer tracer = new Core.Tracer();

            var foo = new Foo(tracer);
            foo.MyMethod();
            thread.Start(tracer);
            thread.Join();

            var result = new XMLSerialization().Serialize(tracer.GetTraceResult());
            IPrinter filePrinter = new FilePrinter(PathHolder.XmlPath);
            IPrinter consolePrinter = new ConsolePrinter();

            filePrinter.PrintResult(result);
            consolePrinter.PrintResult(result);

            Console.WriteLine();

            result = new JSONSerialization().Serialize(tracer.GetTraceResult());
            filePrinter = new FilePrinter(PathHolder.JsonPath);

            filePrinter.PrintResult(result);
            consolePrinter.PrintResult(result);

            //here will be result time


            Console.ReadLine();
        }
    }
}
