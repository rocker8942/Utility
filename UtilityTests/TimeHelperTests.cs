using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Utility.Tests
{
    [TestClass()]
    public class TimeHelperTests
    {
        [TestMethod()]
        public void ConvertToTimezoneTest()
        {
            // convert local to utc
            DateTime dateTimeLocal = DateTime.Now;
            bool isDaylightSavingTime = dateTimeLocal.IsDaylightSavingTime();
            if (isDaylightSavingTime)
                Assert.IsTrue(dateTimeLocal.AddHours(-11) == dateTimeLocal.ToUniversalTime());
            else
                Assert.IsTrue(dateTimeLocal.AddHours(-10) == dateTimeLocal.ToUniversalTime());

            // convert undefined to local. ToLocalTime assume the unspecified time as UTC
            DateTime dateTimeUnspecified = DateTime.Parse(DateTime.Today.ToString());
            DateTime local = dateTimeUnspecified.ToLocalTime();
            Assert.IsFalse(dateTimeUnspecified == local);

            // convert AEST to UTC
            // When AEST is not DST
            DateTime dtAEST = DateTime.Parse(new DateTime(2014, 4, 30, 0, 0, 0).ToString());
            DateTime dtAESTtoUTC = TimeHelper.ConvertToTimezone(dtAEST, TimeHelper.AEST, TimeHelper.UTC);
            Assert.IsFalse(dtAEST.IsDaylightSavingTime());
            Assert.IsTrue(dtAEST == dtAESTtoUTC.AddHours(10));

            // use ToUniversalTime
            // When AEST is not DST and GMT is not DST, the difference between AEST and GMT is 10 hours
            dtAESTtoUTC = dtAEST.ToUniversalTime();
            Assert.IsTrue(dtAEST == dtAESTtoUTC.AddHours(10));

            // convert to GMT (Daylight saving time)
            // When AEST is not DST and GMT is DST, the difference between AEST and GMT is 9 hours
            DateTime dtAESTtoGMT = TimeHelper.ConvertToTimezone(dtAEST, TimeHelper.AEST, TimeHelper.GMT);
            Assert.IsFalse(dtAESTtoGMT.IsDaylightSavingTime());
            Assert.IsTrue(TimeHelper.GMT.IsDaylightSavingTime(dtAESTtoGMT));
            Assert.IsTrue(dtAEST == dtAESTtoGMT.AddHours(9));

            // convert AEDT to UTC
            // When AEST is DST, the difference between AEST and UTC is 11 hours
            DateTime dtAEDT = DateTime.Parse(new DateTime(2014, 3, 30, 0, 0, 0).ToString());
            DateTime dtAEDTtoUTC = TimeHelper.ConvertToTimezone(dtAEDT, TimeHelper.AEST, TimeHelper.UTC);
            Assert.IsTrue(dtAEDT.IsDaylightSavingTime());
            Assert.IsTrue(dtAEDT == dtAEDTtoUTC.AddHours(11));

            // use ToUniversalTime
            dtAEDTtoUTC = dtAEDT.ToUniversalTime();
            Assert.IsTrue(dtAEDT == dtAEDTtoUTC.AddHours(11));

            // convert to GMT (non-Daylight saving time)
            // When AEST is DST and GMT is not DST, the difference between two is 11 hours
            DateTime dtAEDSTtoGMT = TimeHelper.ConvertToTimezone(dtAEDT, TimeHelper.AEST, TimeHelper.GMT);
            Assert.IsTrue(dtAEDT == dtAEDSTtoGMT.AddHours(11));
        }

        [TestMethod]
        public void TestGetOnlyTimeFromString()
        {
            DateTime actual = TimeHelper.GetOnlyTimeFromString("01:00");
            DateTime expect = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 1, 0, 0);
            Assert.AreEqual(expect, actual);
        }
    }
}
