using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo {FileName = string.Format("{0}{1}", Environment.CurrentDirectory, @"\Service.exe")};
            Process.Start(startInfo);
            startInfo.FileName = string.Format("{0}{1}", Environment.CurrentDirectory, @"\Client.exe");
            Process.Start(startInfo);
        }
    }
}