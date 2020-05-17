using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Barcodes.Codes.Tests
{
    [TestClass]
    public class ReleaseDocumentCodeTests
    {
        private class CodeValues
        {
            public CodeValues(int orderId, int containerNumber, int divisionNumber, int year)
            {
                OrderId = orderId;
                ContainerNumber = containerNumber;
                DivisionNumber = divisionNumber;
                Year = year;
            }

            public int OrderId { get; set; }
            public int ContainerNumber { get; set; }
            public int DivisionNumber { get; set; }
            public int Year { get; set; }
        }

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

        [TestMethod]
        public void InvalidValues()
        {
            var values = new List<CodeValues>
            {
                new CodeValues(12345678, 1, 2, 2023),
                new CodeValues(0, 1, 2, 2023),
                new CodeValues(12345, 9999, 2, 2023),
                new CodeValues(12345, 0, 2, 2023),
                new CodeValues(12345, 1, 999, 2023),
                new CodeValues(12345, 1, 0, 2023),
                new CodeValues(12345, 1, 2, 0),
            };
            foreach (var value in values)
            {
                Assert.ThrowsException<ArgumentException>(() => new ReleaseDocumentCode(value.OrderId, value.ContainerNumber, value.DivisionNumber, value.Year));
            }
        }
    }
}