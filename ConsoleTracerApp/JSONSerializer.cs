using System;
using System.Collections.Generic;
using System.Text;
using Tracer;
using System.Text.Json;

namespace ConsoleTracerApp
{
    public class JSONSerializer : ISerializer
    {
        public string Serialize(TraceResult traceResult)
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            return JsonSerializer.Serialize(traceResult, options);
        }
    }
}
