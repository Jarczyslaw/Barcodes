using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Barcodes.Codes.Tests
{
    [TestClass]
    public class ProductCodeTests
    {
        [TestMethod]
        public void ValidFromString()
        {
            var codeString = "1234567890123";
            var code = new ProductCode(codeString);
            Assert.AreEqual(codeString, code.Product);
            Assert.AreEqual(codeString, code.Code);
        }

        [TestMethod]
        public void ValidFromData()
        {
            var product = "1234567890123";
            var code = new ProductCode(product);
            Assert.AreEqual("1234567890123", code.Code);
        }

        [TestMethod]
        public void InvalidCodes()
        {
            var invalidCodes = new List<string>
            {
                "123456789012",
                "12345678901234",
                "123456789012A",
            };
            foreach (var invalidCode in invalidCodes)
            {
                Assert.ThrowsException<ArgumentException>(() => new ProductCode(invalidCode));
            }
        }
    }
}