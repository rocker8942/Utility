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
            var offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
            bool isDaylightSavingTime = dateTimeLocal.IsDaylightSavingTime();
            if (isDaylightSavingTime)
                Assert.IsTrue(dateTimeLocal.AddHours(offset.Hours * -1) == dateTimeLocal.ToUniversalTime());
            else
                Assert.IsTrue(dateTimeLocal.AddHours(offset.Hours * -1) == dateTimeLocal.ToUniversalTime());

            // convert undefined to local. ToLocalTime assume the unspecified time as UTC
            DateTime dateTimeUnspecified = DateTime.Parse(DateTime.Today.ToString());
            DateTime local = dateTimeUnspecified.ToLocalTime();
            Assert.IsFalse(dateTimeUnspecified == local);

            // convert AEST to UTC
            // When AEST is not DST
            DateTime dtLocal = DateTime.Parse(new DateTime(2014, 4, 30, 0, 0, 0).ToString());
            DateTime dtAESTtoUTC = TimeHelper.ConvertToTimezone(dtLocal, TimeHelper.AEST, TimeHelper.UTC);
            Assert.IsFalse(dtLocal.IsDaylightSavingTime());
            Assert.IsTrue(dtLocal == dtAESTtoUTC.AddHours(10));

            // use ToUniversalTime
            // When AEST is not DST and GMT is not DST, the difference between AEST and GMT is 10 hours
            dtAESTtoUTC = dtLocal.ToUniversalTime();
            Assert.IsTrue(dtLocal == dtAESTtoUTC.AddHours(offset.Hours));

            // convert to GMT (Daylight saving time)
            // When AEST is not DST and GMT is DST, the difference between AEST and GMT is 9 hours
            DateTime dtAESTtoGMT = TimeHelper.ConvertToTimezone(dtLocal, TimeZoneInfo.Local, TimeHelper.GMT);
            Assert.IsFalse(dtAESTtoGMT.IsDaylightSavingTime());
            Assert.IsTrue(TimeHelper.GMT.IsDaylightSavingTime(dtAESTtoGMT));
            Assert.IsTrue(dtLocal == dtAESTtoGMT.AddHours(offset.Hours - 1));

            // convert AEDT to UTC
            // When AEST is DST, the difference between AEST and UTC is 11 hours
            // todo: need to find a way to create a AEDT localtime
            dtLocal = DateTime.Parse(new DateTime(2014, 3, 30, 0, 0, 0).ToString());
            DateTime dtAEDTtoUTC = TimeHelper.ConvertToTimezone(dtLocal, TimeHelper.AEST, TimeHelper.UTC);
            // Assert.IsTrue(dtLocal.IsDaylightSavingTime());
            Assert.IsTrue(dtLocal == dtAEDTtoUTC.AddHours(11));

            // convert to GMT (non-Daylight saving time)
            // When AEST is DST and GMT is not DST, the difference between two is 11 hours
            DateTime dtAEDSTtoGMT = TimeHelper.ConvertToTimezone(dtLocal, TimeHelper.AEST, TimeHelper.GMT);
            Assert.IsTrue(dtLocal == dtAEDSTtoGMT.AddHours(11));
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
