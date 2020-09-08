using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Tracer;

namespace ConsoleTracerApp
{
    public interface ISerializer
    {
        string Serialize(TraceResult traceResult);
    }
}
