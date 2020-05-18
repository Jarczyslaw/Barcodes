using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Barcodes.Codes.Tests
{
    [TestClass]
    public class Ean128Tests
    {
        private class CodeValues
        {
            public CodeValues(string productCode, string batchId, DateTime expireDate)
            {
                ProductCode = productCode;
                BatchId = batchId;
                ExpireDate = expireDate;
            }

            public string ProductCode { get; set; }
            public string BatchId { get; set; }
            public DateTime ExpireDate { get; set; }
        }

        [TestMethod]
        public void ValidFromString()
        {
            var codeString = "(02)12345678901234(17)200517(10)batchId";
            var code = new LongProductCode(codeString);
            Assert.AreEqual("12345678901234", code.ProductCode);
            Assert.AreEqual("batchId", code.BatchId);
            Assert.AreEqual(new DateTime(2020, 5, 17), code.ExpireDate);
            Assert.AreEqual(codeString, code.Code);
        }

        [TestMethod]
        public void ValidFromData()
        {
            var productCode = "12345678901234";
            var batchId = "batchId";
            var expireDate = new DateTime(2020, 5, 17);
            var code = new LongProductCode(productCode, batchId, expireDate);
            Assert.AreEqual("(02)12345678901234(17)200517(10)batchId", code.Code);
        }

        [TestMethod]
        public void InvalidCodes()
        {
            var invalidCodes = new List<string>
            {
                "(01)12345678901234(17)200517(10)batchId", // invalid first ZZ
                "(02)12345678901234(17)200517(10)", // no batchId
                "(02)1234567890123(17)200517(10)batchId", // invalid productCode
                "(02)A2345678901234(17)200517(10)batchId", // invalid productCode
                "(02)12345678901234(17)201317(10)batchId", // invalid date
                "(02)12345678901234(17)2017(10)batchId", // invalid date
                "(02)12345678901234(17)200517batchId", // no third ZZ
            };
            foreach (var invalidCode in invalidCodes)
            {
                Assert.ThrowsException<ArgumentException>(() => new LongProductCode(invalidCode));
            }
        }

        [TestMethod]
        public void InvalidValues()
        {
            var values = new List<CodeValues>
            {
                new CodeValues("1234567890123A", "batchId", new DateTime(2020, 5, 17)), // invalid product code
                new CodeValues("12345678901234", "", new DateTime(2020, 5, 17)), // invalid batch id
            };
            foreach (var value in values)
            {
                Assert.ThrowsException<ArgumentException>(() => new LongProductCode(value.ProductCode, value.BatchId, value.ExpireDate));
            }
        }
    }
}