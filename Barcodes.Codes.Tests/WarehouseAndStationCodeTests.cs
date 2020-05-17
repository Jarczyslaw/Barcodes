using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Barcodes.Codes.Tests
{
    [TestClass]
    public class WarehouseAndStationCodeTests
    {
        [TestMethod]
        public void ValidFromString()
        {
            var codeString = "ST0102";
            var code = new WarehouseAndStationCode(codeString);
            Assert.AreEqual(1, code.Warehouse);
            Assert.AreEqual("02", code.Station);
            Assert.AreEqual(codeString, code.Code);
        }

        [TestMethod]
        public void ValidFromData()
        {
            var warehouse = 1;
            var station = "02";
            var code = new WarehouseAndStationCode(warehouse, station);
            Assert.AreEqual("ST0102", code.Code);
        }

        [TestMethod]
        public void InvalidCodes()
        {
            var invalidCodes = new List<string>
            {
                "St0102", // invalid prefix
                "ST01023", // invalid length
                "ST010", // invalid length
                "ST0A02", // invalid character
            };
            foreach (var invalidCode in invalidCodes)
            {
                Assert.ThrowsException<ArgumentException>(() => new WarehouseAndStationCode(invalidCode));
            }
        }
    }
}