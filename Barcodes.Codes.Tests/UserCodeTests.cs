using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Barcodes.Codes.Tests
{
    [TestClass]
    public class UserCodeTests
    {
        [TestMethod]
        public void ValidFromString()
        {
            var codeString = "PR12345";
            var code = new UserCode(codeString);
            Assert.AreEqual(12345, code.Number);
            Assert.AreEqual(codeString, code.Code);
        }

        [TestMethod]
        public void ValidFromData()
        {
            var number = 12345;
            var code = new UserCode(number);
            Assert.AreEqual("PR12345", code.Code);
        }

        [TestMethod]
        public void InvalidCodes()
        {
            var invalidCodes = new List<string>
            {
                "Pr12345", // invalid prefix
                "PR123456", // invalid length
                "PR1234", // invalid length
            };
            foreach (var invalidCode in invalidCodes)
            {
                Assert.ThrowsException<ArgumentException>(() => new UserCode(invalidCode));
            }
        }

        [TestMethod]
        public void InvalidValues()
        {
            var values = new List<int>
            {
                -1, 0, 999999
            };
            foreach (var value in values)
            {
                Assert.ThrowsException<ArgumentException>(() => new UserCode(value));
            }
        }
    }
}