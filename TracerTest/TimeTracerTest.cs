using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading;
using Tracer;

namespace TracerTest
{
    [TestClass]
    public class TimeTracerTest
    {
        private const int SLEEP_TIME = 40;
        private const int THREADS_COUNT = 3;

        private TimeTracer tracer;

        public TimeTracerTest()
        {
            tracer = new TimeTracer();
        }

        private void SingleMethod()
        {
            tracer.StartTrace();
            Thread.Sleep(SLEEP_TIME);
            tracer.StopTrace();
        }

        private void MethodWithInnerMethod()
        {
            tracer.StartTrace();
            Thread.Sleep(SLEEP_TIME);
            SingleMethod();
            tracer.StopTrace();
        }

        [TestMethod]
        public void TestSingleMethod()
        {
            SingleMethod();
            TraceResult traceResult = tracer.GetTraceResult();

            Assert.AreEqual(nameof(SingleMethod), traceResult.Threads[0].Methods[0].MethodName);
            Assert.AreEqual(nameof(TimeTracerTest), traceResult.Threads[0].Methods[0].ClassName);
            Assert.AreEqual(0, traceResult.Threads[0].Methods[0].Methods.Count);
            Assert.IsTrue(traceResult.Threads[0].Methods[0].ElapsedTime >= SLEEP_TIME);
        }

        [TestMethod]
        public void TestMethodWithInnerMethod()
        {
            MethodWithInnerMethod();
            TraceResult traceResult = tracer.GetTraceResult();

            Assert.AreEqual(nameof(MethodWithInnerMethod), traceResult.Threads[0].Methods[0].MethodName);
            Assert.AreEqual(nameof(SingleMethod), traceResult.Threads[0].Methods[0].Methods[0].MethodName);
            Assert.AreEqual(nameof(TimeTracerTest), traceResult.Threads[0].Methods[0].ClassName);
            Assert.AreEqual(1, traceResult.Threads[0].Methods[0].Methods.Count);
            Assert.AreEqual(0, traceResult.Threads[0].Methods[0].Methods[0].Methods.Count);
            Assert.IsTrue(traceResult.Threads[0].Methods[0].ElapsedTime >= SLEEP_TIME * 2);
        }

        [TestMethod]
        public void TestSingleMethodInMultiThreads()
        {
            var threads = new List<Thread>();
            double expectedTotalElapsedTime = 0;

            for (int i = 0; i < THREADS_COUNT; i++)
            {
                var newThread = new Thread(SingleMethod);
                threads.Add(newThread);
                newThread.Start();
                expectedTotalElapsedTime += SLEEP_TIME;
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            double actualTotalElapsedTime = 0;

            foreach (var threadResult in tracer.GetTraceResult().Threads)
            {
                actualTotalElapsedTime += threadResult.TotalElapsedTime;
            }

            Assert.IsTrue(actualTotalElapsedTime >= expectedTotalElapsedTime);
            Assert.AreEqual(THREADS_COUNT, tracer.GetTraceResult().Threads.Count);
        }
    }
}
