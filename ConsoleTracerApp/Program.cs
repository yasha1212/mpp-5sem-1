using System;
using System.IO;
using System.Threading;
using Tracer;

namespace ConsoleTracerApp
{
    class Program
    {
        private static TimeTracer tracer = new TimeTracer();
        private static ISerializer serializer;

        static void Method1()
        {
            tracer.StartTrace();
            Method2();
            Method3();
            Thread.Sleep(14);
            tracer.StopTrace();
        }

        static void Method2()
        {
            tracer.StartTrace();
            Method4();
            Thread.Sleep(16);
            tracer.StopTrace();
        }

        static void Method3()
        {
            tracer.StartTrace();
            Thread.Sleep(11);
            tracer.StopTrace();
        }

        static void Method4()
        {
            tracer.StartTrace();
            Thread.Sleep(12);
            tracer.StopTrace();
        }

        static void Main(string[] args)
        {
            Thread thread1 = new Thread(new ThreadStart(Method1));
            Thread thread2 = new Thread(new ThreadStart(Method1));

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            serializer = new JSONSerializer();

            using (var fs = new FileStream("test.json", FileMode.Create))
            using (var sw = new StreamWriter(fs))
            {
                serializer.Serialize(sw, tracer.GetTraceResult());
            }

            serializer.Serialize(Console.Out, tracer.GetTraceResult());
            Console.WriteLine();

            serializer = new XMLSerializer();

            using (var fs = new FileStream("test.xml", FileMode.Create))
            using (var sw = new StreamWriter(fs))
            {
                serializer.Serialize(sw, tracer.GetTraceResult());
            }

            serializer.Serialize(Console.Out, tracer.GetTraceResult());

            Console.ReadKey();
        }
    }
}
