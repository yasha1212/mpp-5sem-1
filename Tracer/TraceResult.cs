using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Tracer
{
    public class TraceResult
    {
        public Dictionary<int, ThreadTracer> Threads { get; private set; }

        public TraceResult(Dictionary<int, ThreadTracer> threads)
        {
            Threads = new Dictionary<int, ThreadTracer>();

            foreach (var thread in threads)
            {
                Threads.Add(thread.Key, thread.Value.GetTraceResult());
            }
        }
    }
}
