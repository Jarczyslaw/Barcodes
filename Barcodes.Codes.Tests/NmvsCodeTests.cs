using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Barcodes.Codes.Tests
{
    [TestClass]
    public class NmvsCodeTests
    {
        [TestMethod]
        public void ValidFromString()
        {
            var codeString = "01123456789012341723010221serialNo10batchId";
            var code = new NmvsCode(codeString);
            Assert.AreEqual("12345678901234", code.ProductCode);
            Assert.AreEqual("batchId", code.BatchId);
            Assert.IsTrue(code.ExpireDate.Equals(new NmvsDate(2023, 1, 2)));
            Assert.AreEqual("serialNo", code.SerialNo);
            Assert.AreEqual(codeString, code.Code);
        }

        [TestMethod]
        public void ValidFromData()
        {
            var productCode = "12345678901234";
            var batchId = "batchId";
            var serialNo = "serialNo";
            var code = new NmvsCode(productCode, serialNo, batchId, new NmvsDate(2023, 1, 2));
            Assert.AreEqual("01123456789012341723010221serialNo10batchId", code.Code);
        }

        [TestMethod]
        public void InvalidCodes()
        {
            var invalidCodes = new List<string>
            {
                "0123456789012341723010221serialNo10batchId", // invalid structure
                "011234567890123A1723010221serialNo10batchId", // invalid productCode
                "01123456789012341723013221serialNo10batchId", // invalid date
                "0112345678901234172301022110batchId", // no serialNumber
                "01123456789012341723010221serialNo10", // no batchId
            };
            foreach (var invalidCode in invalidCodes)
            {
                Assert.ThrowsException<ArgumentException>(() => new NmvsCode(invalidCode));
            }
        }
    }
}