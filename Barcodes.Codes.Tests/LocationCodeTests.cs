using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Barcodes.Codes.Tests
{
    [TestClass]
    public class LocationCodeTests
    {
        [TestMethod]
        public void ValidFromString()
        {
            var codeString = "LK0000012345";
            var code = new LocationCode(codeString);
            Assert.AreEqual("0000012345", code.Address);
            Assert.AreEqual(codeString, code.Code);
        }

        [TestMethod]
        public void ValidFromData()
        {
            var address = "0000012345";
            var code = new LocationCode(address);
            Assert.AreEqual("LK0000012345", code.Code);
        }

        [TestMethod]
        public void InvalidCodes()
        {
            var invalidCodes = new List<string>
            {
                "Lk0000012345", // invalid prefix
                "LK000001234", // invalid length
                "LK00000123459", // invalid length
            };
            foreach (var invalidCode in invalidCodes)
            {
                Assert.ThrowsException<ArgumentException>(() => new LocationCode(invalidCode));
            }
        }
    }
}