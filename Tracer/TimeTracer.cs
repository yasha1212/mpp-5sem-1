using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Tracer
{
    public class TimeTracer : ITracer
    {
        static private object locker = new object();

        private Dictionary<int, ThreadTracer> threads;

        public TimeTracer()
        {
            threads = new Dictionary<int, ThreadTracer>();
        }

        public TraceResult GetTraceResult()
        {
            return new TraceResult(threads);
        }

        public void StartTrace()
        {
            MethodBase methodBase = new StackTrace().GetFrame(1).GetMethod();
            MethodTracer methodTracer = new MethodTracer(methodBase.ReflectedType.Name, methodBase.Name);
            ThreadTracer threadTracer = GetThreadTracer(Thread.CurrentThread.ManagedThreadId);

            threadTracer.StartTrace(methodTracer);
        }

        public void StopTrace()
        {
            GetThreadTracer(Thread.CurrentThread.ManagedThreadId).StopTrace();
        }

        private ThreadTracer GetThreadTracer(int id)
        {
            lock (locker)
            {
                if (!threads.TryGetValue(id, out ThreadTracer thread))
                {
                    thread = new ThreadTracer(id);
                    threads.Add(id, thread);
                }

                return thread;
            }
        }
    }
}
