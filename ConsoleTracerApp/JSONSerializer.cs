using System;
using System.Collections.Generic;
using System.Text;
using Tracer;
using System.Text.Json;
using System.IO;

namespace ConsoleTracerApp
{
    public class JSONSerializer : ISerializer
    {
        public void Serialize(TextWriter writer, TraceResult traceResult)
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            writer.WriteLine(JsonSerializer.Serialize(traceResult, options));
        }
    }
}
