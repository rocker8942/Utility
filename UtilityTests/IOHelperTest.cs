using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utility;

namespace UtilityTests
{
    [TestClass]
    public class IOHelperTest
    {
        [TestMethod]
        public void GetNameWithDateTest()
        {
            var ioHelper = new IOHelper();
            string result = ioHelper.GetNameWithDate("abc");
            Assert.AreEqual(result, "abc_" + DateTime.Now.ToString("yyyyMMdd"));
        }
    }
}
