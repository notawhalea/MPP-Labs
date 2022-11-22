using System.Collections.Concurrent;

public class TraceResult
{
    private ConcurrentDictionary<int, ThreadTrace> ThreadTraces { get; }

    public TraceResult(ConcurrentDictionary<int, ThreadTrace> threadTraces)
    {
        ThreadTraces = threadTraces;
    }

    internal ThreadTrace GetThreadTrace(int threadId)
    {
        return ThreadTraces.GetOrAdd(threadId, new ThreadTrace(threadId));
    }

    public ConcurrentDictionary<int, ThreadTrace> GetThreadTraces()
    {
        return ThreadTraces;
    }
}