using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Tracer
{
    public class TraceResult
    {
        public List<ThreadTracer> Threads { get; private set; }

        public TraceResult(Dictionary<int, ThreadTracer> threads)
        {
            Threads = new List<ThreadTracer>();

            foreach (var thread in threads)
            {
                Threads.Add(thread.Value.GetTraceResult());
            }
        }
    }
}
