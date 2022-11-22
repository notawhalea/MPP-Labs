using System;
using Serialization;
using Tracer.Example;

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

class MainClass
{
    public void Method(object o)
    {
        var tracer = (Core.Tracer)o;
        tracer.StartTrace();
        Thread.Sleep(100);
        tracer.StopTrace();
    }
}
