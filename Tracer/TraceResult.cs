using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Tracer
{
    public class TraceResult
    {
        [JsonPropertyName("threads")]
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
