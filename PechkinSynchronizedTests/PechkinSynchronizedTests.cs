using System;
using System.Collections.Specialized;
using System.Threading;
using Pechkin;
using Pechkin.Synchronized;
using PechkinTests;
using Xunit;

namespace PechkinSynchronizedTests
{
    public class PechkinSynchronizedTestsInitClass : IDisposable
    {
        public void Dispose() // deinit library
        {
            SynchronizedPechkin.ClearBeforeExit();
        }
    }

    public class PechkinSynchronizedTests : PechkinAbstractTests<SynchronizedPechkin>, IUseFixture<PechkinSynchronizedTestsInitClass>
    {
        protected override SynchronizedPechkin ProduceTestObject(GlobalConfig cfg)
        {
            return new SynchronizedPechkin(cfg);
        }

        protected override void TestEnd()
        {
        }

        public void SetFixture(PechkinSynchronizedTestsInitClass data)
        {
            
        }

        private static int _threadId;

        [Fact]
        public void ThreadedRequestsGetProcessed()
        {
            // oh well, this test doesn't fail if it does, because there's no assertions or exceptions (or anything) in threads
            // but log and debug should show anomalies (and todo I should assert on them)

            NameValueCollection properties = new NameValueCollection();
            properties["showDateTime"] = "true";

            ThreadStart ts = () =>
                                 {
                                     lock (this)
                                     {
                                         Thread.CurrentThread.Name = "test thread " + (_threadId++).ToString();
                                     }

                                     string html = GetResourceString("PechkinTests.Resources.page.html");

                                     SynchronizedPechkin c = ProduceTestObject(new GlobalConfig());
                                     //SimplePechkin c = new SimplePechkin(new GlobalConfig());

                                     byte[] ret = c.Convert(html);

                                     Assert.NotNull(ret);
                                 };

            Thread t = new Thread(ts);
            Thread t2 = new Thread(ts);

            t.Start();
            Thread.Sleep(1000);
            t2.Start();

            GC.Collect();

            Thread.Sleep(6000);

            GC.Collect();

            Thread.Sleep(60000);

            /*
            t = new Thread(ts);
            t2 = new Thread(ts);

            t.Start();
            Thread.Sleep(1000);
            t2.Start();

            GC.Collect();

            Thread.Sleep(6000);

            GC.Collect();
            */

            TestEnd();
        }
    }
}
