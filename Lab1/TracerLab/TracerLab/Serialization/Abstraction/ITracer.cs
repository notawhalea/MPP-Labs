public interface ITracer
{
    void StartTrace();

    void StopTrace();
 
    TraceResult GetTraceResult();
    /*
     * Here you get result of our trace time
     */
}
