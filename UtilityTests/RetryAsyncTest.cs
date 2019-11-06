using System;
using System.Diagnostics;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utility;

namespace UtilityTests
{
    [TestClass]
    public class RetryAsyncTest
    {
        private int _retries;

        private async Task<bool> SucceedAlways(int anything)
        {
            _retries++;

            return await Task.FromResult(true);
        }

        private async Task<bool> SuccessOnSubsequentTry(int anything)
        {
            _retries++;
            if (_retries == 1)
                throw new Exception("first try always fail");

            return await Task.FromResult(true);
        }

        private async Task<bool> FailAlways()
        {
            _retries++;

            throw new InvalidOperationException("first try always fail");
        }

        [TestMethod]
        public async Task RetrySucceedsOnFirstTryAsync()
        {
            _retries = 0;

            var completedDelegate = await Retry.DoAsync(
                async () => await SucceedAlways(1)
                , TimeSpan.FromSeconds(1));

            Debug.WriteLine(_retries.ToString());
            Assert.IsTrue(_retries == 1);
        }

        [TestMethod]
        public async Task RetrySucceedsOnSubsequentTryAsync()
        {
            _retries = 0;
            var completedDelegate = await Retry.DoAsync(
                async () => await SuccessOnSubsequentTry(1)
                , TimeSpan.FromSeconds(1));

            Debug.WriteLine(_retries.ToString());
            Assert.IsTrue(_retries > 1);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public async Task RetryFail()
        {
            _retries = 0;
            bool completedDelegate = await Retry.DoAsync(
                async () => await FailAlways()
                , TimeSpan.FromSeconds(1)
                , 3);

            Debug.WriteLine(_retries.ToString());
            Assert.IsTrue(_retries == 3);
        }
    }
}