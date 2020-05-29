using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Barcodes.Codes.Tests
{
    [TestClass]
    public class PaletteCodeTests
    {
        [TestMethod]
        public void ValidFromString()
        {
            var codeString = "PA0001234";
            var code = new PaletteCode(codeString);
            Assert.AreEqual(1234, code.PaletteNumber);
            Assert.AreEqual("0001234", code.PaletteNumberCode);
            Assert.AreEqual("PA", code.CurrentPrefix);
        }

        [TestMethod]
        public void ValidFromData()
        {
            var paletteNumber = 1234;
            var code = new PaletteCode(paletteNumber);
            code.CurrentPrefix = "PT";
            Assert.AreEqual(1234, code.PaletteNumber);
            Assert.AreEqual("PT1234", code.Code);

            code = new PaletteCode(paletteNumber, 7);
            code.CurrentPrefix = "PA";
            Assert.AreEqual(1234, code.PaletteNumber);
            Assert.AreEqual("PA0001234", code.Code);
        }

        [TestMethod]
        public void InvalidCodes()
        {
            var invalidCodes = new List<string>
            {
                "Px0001", // invalid prefix
                "PA000A", // invalid character
            };
            foreach (var invalidCode in invalidCodes)
            {
                Assert.ThrowsException<ArgumentException>(() => new ContainerCode(invalidCode));
            }
        }

        [TestMethod]
        public void InvalidValues()
        {
            var values = new List<int>
            {
                -1, 0
            };
            foreach (var value in values)
            {
                Assert.ThrowsException<ArgumentException>(() => new ContainerCode(value));
            }
        }
    }
}