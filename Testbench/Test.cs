using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Saleae;
using NUnit.Framework;

namespace Testbench
{
    [TestFixture]
    public class Test
    {
        private SocketAPI s;

        [TestFixtureSetUp]
        public void Init()
        {
            s = new SocketAPI();
        }

        [SetUp]
        public void Connect()
        {
            s.Open();
        }

        [TearDown]
        public void Disconnect()
        {
            s.Close();
        }

        [Test]
        public void Trace5Seconds()
        {
            s.SetCaptureSeconds(5);
            s.Capture();
        }

        [Test]
        public void Trace5SecondsStopAfter4Seconds()
        {
            s.SetCaptureSeconds(5);
            s.Capture(true);
            System.Threading.Thread.Sleep(4000);
            s.StopCapture();
        }

        [Test]
        public void Trace3SecondsStopAfter2Seconds3Times()
        {
            for (int i = 0; i < 3; i++)
            {
                s.SetCaptureSeconds(3);
                s.Capture(true);
                System.Threading.Thread.Sleep(2000);
                s.StopCapture();
            }
        }

        [Test]
        public void Trace2File3Times()
        {
            s.SetCaptureSeconds(3);
            for (int i = 0; i < 3; i++)
            {
                s.Capture(true);
                s.SaveToFile(@"c:\file_" + i + ".logicdata");
            }
        }
    }
}
