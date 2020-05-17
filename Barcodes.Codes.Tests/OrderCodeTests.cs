using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Barcodes.Codes.Tests
{
    [TestClass]
    public class OrderCodeTests
    {
        [TestMethod]
        public void ValidFromString()
        {
            var codeString = "DZ0000123450010223";
            var code = new OrderCode(codeString);
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
            var code = new OrderCode(orderId, containerNumber, divisionNumber, year);
            Assert.AreEqual("DZ0000123450010223", code.Code);
        }

        [TestMethod]
        public void InvalidCodes()
        {
            var invalidCodes = new List<string>
            {
                "Dz0000123450010223", // invalid prefix
                "DZ00001234560010223", // invalid length
                "DZ000012350010223", // invalid length
                "DZ000012345A001022", // invalid character
            };
            foreach (var invalidCode in invalidCodes)
            {
                Assert.ThrowsException<ArgumentException>(() => new OrderCode(invalidCode));
            }
        }
    }
}