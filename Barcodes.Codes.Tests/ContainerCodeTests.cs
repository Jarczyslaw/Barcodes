using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Barcodes.Codes.Tests
{
    [TestClass]
    public class ContainerCodeTests
    {
        [TestMethod]
        public void ValidFromString()
        {
            var codeString = "PJ0001234";
            var code = new ContainerCode(codeString);
            Assert.AreEqual(1234, code.ContainerNumber);
            Assert.AreEqual(codeString, code.Code);
        }

        [TestMethod]
        public void ValidFromData()
        {
            var containerNumber = 1234;
            var code = new ContainerCode(containerNumber);
            Assert.AreEqual(1234, code.ContainerNumber);
            Assert.AreEqual("PJ0001234", code.Code);
        }

        [TestMethod]
        public void InvalidCodes()
        {
            var invalidCodes = new List<string>
            {
                "PJ0001", // invalid length
                "Pj0001234", // invalid prefix
                "PJ00000001", // invalid length
                "PJ000000A" // invalid character
            };
            foreach (var invalidCode in invalidCodes)
            {
                Assert.ThrowsException<ArgumentException>(() => new ContainerCode(invalidCode));
            }
        }
    }
}