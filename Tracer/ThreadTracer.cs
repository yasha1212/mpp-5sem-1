using System;
using System.Collections.Generic;
using System.Text;

namespace Tracer
{
    public class ThreadTracer
    {
        public int Id { get; private set; }
        public TimeSpan TotalElapsedTime { get; private set; }
        public List<MethodTracer> Methods { get; private set; }

        private Stack<MethodTracer> methodsStack; 

        public ThreadTracer(int id)
        {
            Id = id;
            TotalElapsedTime = new TimeSpan(0);
            Methods = new List<MethodTracer>();
            methodsStack = new Stack<MethodTracer>();
        }

        internal ThreadTracer GetTraceResult()
        {
            var result = new ThreadTracer(Id);
            result.TotalElapsedTime = TotalElapsedTime;

            foreach (var method in Methods)
            {
                result.Methods.Add(method.GetTraceResult());
            }

            return result;
        }

        internal void StartTrace(MethodTracer method)
        {
            if (methodsStack.Count > 0)
            {
                MethodTracer lastMethod = methodsStack.Peek();
                lastMethod.Methods.Add(method);
            }

            method.StartTrace();
            methodsStack.Push(method);
        }

        internal void StopTrace()
        {
            MethodTracer lastMethod = methodsStack.Pop();
            lastMethod.StopTrace();

            if (methodsStack.Count == 0)
            {
                Methods.Add(lastMethod);
                TotalElapsedTime += lastMethod.ElapsedTime;
            }
        }
    }
}
