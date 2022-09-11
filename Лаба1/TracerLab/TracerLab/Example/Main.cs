using System;
class MainClass
{
    static void Main()
    {
        var program = new MainClass();
        var thread = new Thread(program.Method);
        ITracer tracer = new Tracer();

        var foo = new Foo(tracer);
        foo.MyMethod();
        thread.Start(tracer);
        thread.Join();


        //here will be result time
    

        Console.ReadLine();
    }
    public class Foo
    {
        private Bar _bar;
        private ITracer _tracer;

        internal Foo(ITracer tracer)
        {
            _tracer = tracer;
            _bar = new Bar(_tracer);
        }

        public void MyMethod()
        {
            _tracer.StartTrace();
            _bar.InnerMethod();
            _tracer.StopTrace();
        }
    }

    public class Bar
    {
        private ITracer _tracer;

        internal Bar(ITracer tracer)
        {
            _tracer = tracer;
        }

        public void InnerMethod()
        {
            _tracer.StartTrace();

            Thread.Sleep(100);
            _tracer.StopTrace();
        }
    }

    public void Method(object o)
    {
        var tracer = (Tracer)o;
        tracer.StartTrace();
        Thread.Sleep(100);
        tracer.StopTrace();
    }
}
