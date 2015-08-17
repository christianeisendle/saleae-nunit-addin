using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Core;
using NUnit.Core.Extensibility;
using NUnit.Framework;
using Saleae;
using System.Timers;
using System.IO;
using System.Threading;

namespace SaleaeAddin
{
    [NUnitAddin(Name = "Saleae Logic Analyzer Remote Control 1.0", Description = "Remote Control for Saleae Logic software. Starts a trace for every testcase and saves the result to a file (into working directory).")]
    public class Addin : IAddin, EventListener
    {
        private SocketAPI s;
        private System.Timers.Timer t;
        private TestName currentTestName;
        private bool testRunning;
        private bool captureRunning;
        private Mutex m;
        private const int CAPTURE_TIME_IN_SECONDS = 10;

        public bool Install(IExtensionHost host)
        {
            IExtensionPoint decorators = host.GetExtensionPoint("EventListeners");
            if (decorators == null)
                return false;

            s = new SocketAPI();
            
            t = new System.Timers.Timer();
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Interval = CAPTURE_TIME_IN_SECONDS * 1000 * 0.8;
            m = new Mutex(false);
            t.AutoReset = false;
            testRunning = false;
            decorators.Install(this);
            
            return true;
        }

        public void RunFinished(Exception exception)
        {
            
        }

        public void RunFinished(TestResult result)
        {
            s.Close();
        }

        public void RunStarted(string name, int testCount)
        {
            s.Open();
            s.SetNumSamples(16000000);
        }

        public void SuiteFinished(TestResult result)
        {
            
        }

        public void SuiteStarted(TestName testName)
        {
            
        }

        private void StopAndSave()
        {
            if (captureRunning)
            {
                s.StopCapture();
                captureRunning = false;
                while (!s.IsProcessingComplete())
                { }
                s.SaveToFile(@Directory.GetCurrentDirectory() + "\\" + currentTestName.FullName + "_" + DateTime.Now.ToString(@"yyyy-MM-dd_HH-mm-ss.f") + ".logicdata");
            }
        }

        public void TestFinished(TestResult result)
        {
            m.WaitOne();
            try
            {
                t.Stop();
                testRunning = false;
                StopAndSave();                
            }
            catch
            {}
            m.ReleaseMutex();
        }

        public void TestOutput(TestOutput testOutput)
        {
            
        }

        public void TestStarted(TestName testName)
        {
            testRunning = true;
            currentTestName = testName;
            s.SetCaptureSeconds(CAPTURE_TIME_IN_SECONDS);
            t.Start();
            s.Capture(true);
            captureRunning = true;
        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            m.WaitOne();
            try
            {
                if (testRunning)
                {
                    DateTime now = DateTime.Now;

                    StopAndSave();
                    
                    s.Capture(true);
                    captureRunning = true;
                    t.Start();
                }
            }
            catch
            { }
            m.ReleaseMutex();
        }

        public void UnhandledException(Exception exception)
        {
            
        }
    }
}
