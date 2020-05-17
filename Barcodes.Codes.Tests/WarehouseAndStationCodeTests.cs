using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Barcodes.Codes.Tests
{
    [TestClass]
    public class WarehouseAndStationCodeTests
    {
        private class CodeValues
        {
            public CodeValues(int warehouse, string station)
            {
                Warehouse = warehouse;
                Station = station;
            }

            public int Warehouse { get; set; }
            public string Station { get; set; }
        }

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

        [TestMethod]
        public void InvalidValues()
        {
            var values = new List<CodeValues>
            {
                new CodeValues(999, "01"),
                new CodeValues(99, "010"),
                new CodeValues(0, "01"),
                new CodeValues(1, ""),
            };
            foreach (var value in values)
            {
                Assert.ThrowsException<ArgumentException>(() => new WarehouseAndStationCode(value.Warehouse, value.Station));
            }
        }
    }
}