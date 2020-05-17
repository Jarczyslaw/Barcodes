using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Barcodes.Codes.Tests
{
    [TestClass]
    public class ReleaseDocumentCodeTests
    {
        [TestMethod]
        public void ValidFromString()
        {
            var codeString = "WZ00123450010223";
            var code = new ReleaseDocumentCode(codeString);
            Assert.AreEqual(12345, code.OrderId);
            Assert.AreEqual(1, code.ContainerNumber);
            Assert.AreEqual(2, code.DivisionNumber);
            Assert.AreEqual(2023, code.Year);
            Assert.AreEqual(codeString, code.Code);
        }

        [TestMethod]
        public void ValidFromData()
        {
            var orderId = 12345;
            var containerNumber = 1;
            var divisionNumber = 2;
            var year = 2023;
            var code = new ReleaseDocumentCode(orderId, containerNumber, divisionNumber, year);
            Assert.AreEqual("WZ00123450010223", code.Code);
        }

        [TestMethod]
        public void InvalidCodes()
        {
            var invalidCodes = new List<string>
            {
                "Wz00123450010223", // invalid prefix
                "WZ001234500102234", // invalid length
                "WZ0012345001023", // invalid length
                "WZ0012345001022A", // invalid character
            };
            foreach (var invalidCode in invalidCodes)
            {
                Assert.ThrowsException<ArgumentException>(() => new ReleaseDocumentCode(invalidCode));
            }
        }
    }
}