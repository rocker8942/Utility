using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utility;

namespace UtilityTests.ConfigManage
{
    [TestClass()]
    public class SettingTests
    {
        private string exePath = @"E:\SourceCode\Git\Utility\ConsoleAppToBeTested\bin\Debug\ConsoleAppToBeTested.exe";


        [TestMethod()]
        public void GetAppSettingTest()
        {
            string actual = Setting.GetAppSetting("key", exePath);
            Assert.AreEqual("confirmed", actual);
        }

        [TestMethod()]
        public void SetAppSettingTest()
        {
            // test
            Setting.SetAppSetting("key", "changed", exePath);
            string actual = Setting.GetAppSetting("key", exePath);
            Assert.AreEqual("changed", actual);

            // Set the value back to default
            Setting.SetAppSetting("key", "confirmed", exePath);
        }
    }
}