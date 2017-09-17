using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace Utility.Tests
{
    [TestClass]
    public class RetryTests
    {
        private int retries;

        private bool SucceedAlways()
        {
            retries++;

            return true;
        }

        private bool SucessOnSubsequentTry()
        {
            retries++;
            if (retries == 1)
                throw new Exception("first try always fail");
            //long tick = DateTime.Now.Ticks;
            //int seed = int.Parse(tick.ToString().Substring(tick.ToString().Length - 4, 4));
            //var random = new Random(seed);
            //int value = random.Next();
            //int even = value % 2;

            //if (even.ToString().EndsWith("0"))
                return true;
            // throw new Exception("fail scenario in second trial as a random");
        }

        private bool FailAlways()
        {
            retries++;

            throw new Exception("first try always fail");
        }

        [TestMethod]
        public void RetrySucceedsOnFirstTry()
        {
            retries = 0;
            bool completedDelegate = Retry.Do(
                () => SucceedAlways()
                , TimeSpan.FromSeconds(1));

            Debug.WriteLine(retries.ToString());
            Assert.IsTrue(retries == 1);
        }

        [TestMethod]
        public void RetrySucceedsOnSubsequentTry()
        {
            retries = 0;
            bool completedDelegate = Retry.Do(
                () => SucessOnSubsequentTry()
                , TimeSpan.FromSeconds(1));

            Debug.WriteLine(retries.ToString());
            Assert.IsTrue(retries > 1);
        }

        [TestMethod]
        public void RetryFail()
        {
            retries = 0;
            try
            {
                bool completedDelegate = Retry.Do(
                    () => FailAlways()
                    , TimeSpan.FromSeconds(1)
                    , 3);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            Debug.WriteLine(retries.ToString());
            Assert.IsTrue(retries == 3);
        }
    }
}