using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Barcodes.Codes.Tests
{
    [TestClass]
    public class AlleyCodeTests
    {
        private class CodeValues
        {
            public string Location { get; set; }
            public string Alley { get; set; }
        }

        [TestMethod]
        public void ValidFromString()
        {
            var codeString = "AL0000012345XX";
            var code = new AlleyCode(codeString);
            Assert.AreEqual("0000012345", code.LocationAddress);
            Assert.AreEqual("XX", code.AlleyAddress);
            Assert.AreEqual(codeString, code.Code);
        }

        [TestMethod]
        public void ValidFromData()
        {
            var location = "0000012345";
            var alley = "XX";
            var code = new AlleyCode(location, alley);
            Assert.AreEqual("AL0000012345XX", code.Code);
        }

        [TestMethod]
        public void InvalidCodes()
        {
            var invalidCodes = new List<string>
            {
                "Ax0000012345XX", // invalid prefix
                "AL00000123a5XX", // invalid character
                "AL0000012345", // no station
                "AL0000012XX" // invalid length
            };
            foreach (var invalidCode in invalidCodes)
            {
                Assert.ThrowsException<ArgumentException>(() => new AlleyCode(invalidCode));
            }
        }

        [TestMethod]
        public void InvalidValues()
        {
            var values = new List<CodeValues>
            {
                new CodeValues
                {
                    Location = "",
                    Alley = ""
                },
                new CodeValues
                {
                    Location = "000000012345",
                    Alley = "AS"
                },
                new CodeValues
                {
                    Location = "0000012345",
                    Alley = "A"
                },
            };
            foreach (var value in values)
            {
                Assert.ThrowsException<ArgumentException>(() => new AlleyCode(value.Location, value.Alley));
            }
        }
    }
}