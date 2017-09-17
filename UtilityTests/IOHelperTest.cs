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
            string result = IOHelper.GetNameWithDate("abc");
            Assert.AreEqual(result, "abc_" + DateTime.Now.ToString("yyyyMMdd"));
        }
    }
}
