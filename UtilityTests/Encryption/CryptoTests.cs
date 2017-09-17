using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Utility.Encryption.Tests
{
    [TestClass()]
    public class CryptoTests
    {
        [TestMethod()]
        public void EncryptStringAESTest()
        {
            string encrypted = Crypto.EncryptStringAES("test", "key");
            string decrypted = Crypto.DecryptStringAES(encrypted, "key");
            Assert.AreEqual("test", decrypted);
        }
    }
}
