using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Barcodes.Codes.Tests
{
    [TestClass]
    public class NmvsCodeTests
    {
        private class CodeValues
        {
            public CodeValues(string productCode, string batchId, string serialNo, NmvsDate expireDate)
            {
                ProductCode = productCode;
                BatchId = batchId;
                SerialNo = serialNo;
                ExpireDate = expireDate;
            }

            public string ProductCode { get; set; }
            public string BatchId { get; set; }
            public string SerialNo { get; set; }
            public NmvsDate ExpireDate { get; set; }
        }

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

        [TestMethod]
        public void InvalidValues()
        {
            var values = new List<CodeValues>
            {
                new CodeValues("1234567890123A", "batchId", "serialNo", new NmvsDate(2023, 1, 2)),
                new CodeValues("12345678901234", "", "serialNo", new NmvsDate(2023, 1, 2)),
                new CodeValues("12345678901234", "batchId", "", new NmvsDate(2023, 1, 2)),
            };
            foreach (var value in values)
            {
                Assert.ThrowsException<ArgumentException>(() => new NmvsCode(value.ProductCode, value.BatchId, value.SerialNo, value.ExpireDate));
            }
        }
    }
}