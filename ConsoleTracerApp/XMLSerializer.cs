using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Tracer;

namespace ConsoleTracerApp
{
    public class XMLSerializer : ISerializer
    {
        public string Serialize(TraceResult traceResult)
        {
            return new XDocument( new XElement("root",
                from thread in traceResult.Threads select SerializeThreadInfo(thread))).ToString();
        }

        private XElement SerializeThreadInfo(ThreadTracer thread)
        {
            return new XElement("thread",
                new XAttribute("id", thread.Id),
                new XAttribute("time", thread.TotalElapsedTime.ToString() + "ms"),
                from method in thread.Methods select SerializeMethodInfo(method));
        }

        private XElement SerializeMethodInfo(MethodTracer method)
        {
            var serializedMethod = new XElement("method",
                new XAttribute("name", method.MethodName),
                new XAttribute("time", method.ElapsedTime.ToString() + "ms"),
                new XAttribute("class", method.ClassName));

            if (method.Methods.Count > 0)
            {
                serializedMethod.Add(from innerMethod in method.Methods select SerializeMethodInfo(innerMethod));
            }

            return serializedMethod;
        }
    }
}
