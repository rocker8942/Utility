using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utility.FTP;
using WinSCP;

namespace UtilityTests.FTP
{
    [TestClass]
    public class SFtpClientTests
    {
        [TestCategory("Integration")]
        [TestMethod]
        public void FTPTest()
        {
            var ftp = new SFtpClient("localhost", "admin", "syncplify", FtpSecure.None);
            var result = ftp.FtpUpload(@"d:\temp\test1.csv", "/");

            // assert
            result.IsSuccess.Should().BeTrue();
            result.Transfers.Count.Should().Be(1);
            result.Transfers.First().FileName.Should(). BeEquivalentTo(@"d:\temp\test1.csv");
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void FTPsTestWithoutFingerPrint()
        {
            var ftpS = new SFtpClient("localhost", "admin", "syncplify", FtpSecure.Explicit);
            var result = ftpS.FtpUpload(@"d:\temp\test2.csv", "/");

            // assert
            result.IsSuccess.Should().BeTrue();
            result.Transfers.Count.Should().Be(1);
            result.Transfers.First().FileName.Should().BeEquivalentTo(@"d:\temp\test2.csv");
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void FTPsTestWithFingerPrint()
        {
            var ftpS = new SFtpClient("localhost", "admin", "syncplify", FtpSecure.Explicit, "68:8c:12:46:ce:0b:2c:d3:63:95:45:b6:26:d2:ec:b9:cb:72:18:77");
            var result = ftpS.FtpUpload(@"d:\temp\test3.csv", "/");

            // assert
            result.IsSuccess.Should().BeTrue();
            result.Transfers.Count.Should().Be(1);
            result.Transfers.First().FileName.Should().BeEquivalentTo(@"d:\temp\test3.csv");
        }
    }
}