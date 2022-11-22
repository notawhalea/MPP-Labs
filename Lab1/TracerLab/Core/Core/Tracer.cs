using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

namespace Core { 
    public class Tracer : ITracer
    {
        private readonly TraceResult _traceResult;

        public Tracer()
        {
            _traceResult = new TraceResult(new ConcurrentDictionary<int, ThreadTrace>());
        }

        public void StartTrace()
        {
            var threadTrace = _traceResult.GetThreadTrace(Thread.CurrentThread.ManagedThreadId);

            var stackTrace = new StackTrace();

            var path = stackTrace.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.None);
            path[0] = "";

            var methodName = stackTrace.GetFrames()[1].GetMethod().Name;
            var className = stackTrace.GetFrames()[1].GetMethod().ReflectedType.Name;

            threadTrace.PushMethod(methodName, className, string.Join("", path));
        }

        public void StopTrace()
        {
            var threadTrace = _traceResult.GetThreadTrace(Thread.CurrentThread.ManagedThreadId);
            var path = new StackTrace().ToString().Split(new string[] { "\r\n" }, StringSplitOptions.None);
            path[0] = "";

            threadTrace.PopMethod(string.Join("", path));
        }

        public TraceResult GetTraceResult()
        {
            return _traceResult;
        }
    }
}