using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json.Serialization;

namespace Tracer
{
    public class MethodTracer
    {
        [JsonPropertyName("name")]
        public string MethodName { get; private set; }

        [JsonPropertyName("class")]
        public string ClassName { get; private set; }

        [JsonPropertyName("time")]
        public double ElapsedTime { get; private set; }

        [JsonPropertyName("methods")]
        public List<MethodTracer> Methods { get; internal set; }

        private Stopwatch stopwatch;

        public MethodTracer(string className, string methodName)
        {
            ClassName = className;
            MethodName = methodName;
            Methods = new List<MethodTracer>();
            stopwatch = new Stopwatch();
        }

        internal MethodTracer GetTraceResult()
        {
            var result = new MethodTracer(ClassName, MethodName);
            result.ElapsedTime = ElapsedTime;

            foreach (var method in Methods)
            {
                result.Methods.Add(method.GetTraceResult());
            }

            return result;
        }

        internal void StartTrace()
        {
            stopwatch.Start();
        }

        internal void StopTrace()
        {
            stopwatch.Stop();
            ElapsedTime = stopwatch.Elapsed.TotalMilliseconds;
        }
    }
}
