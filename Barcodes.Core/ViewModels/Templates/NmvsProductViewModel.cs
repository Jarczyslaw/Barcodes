﻿using Barcodes.Codes;
using Barcodes.Core.Abstraction;
using System;

namespace Barcodes.Core.ViewModels.Templates
{
    public class NmvsProductViewModel : BaseTemplateViewModel
    {
        private string productCode = string.Empty;
        private string batchId = string.Empty;
        private string batchExpDate = string.Empty;
        private string serialNo = string.Empty;

        public NmvsProductViewModel(IAppDialogsService dialogsService)
            : base(dialogsService)
        {
        }

        public string ProductCode
        {
            get => productCode;
            set => SetProperty(ref productCode, value);
        }

        public string BatchId
        {
            get => batchId;
            set => SetProperty(ref batchId, value);
        }

        public string SerialNo
        {
            get => serialNo;
            set => SetProperty(ref serialNo, value);
        }

        public string BatchExpDate
        {
            get => batchExpDate;
            set => SetProperty(ref batchExpDate, value);
        }

        public NmvsCode GetNmvsCode()
        {
            return new NmvsCode(ProductCode.Trim(), SerialNo.Trim(), BatchId.Trim(), BatchExpDate.Trim());
        }

        public void LoadData(string nmvsData)
        {
            if (string.IsNullOrEmpty(nmvsData))
            {
                return;
            }

            if (NmvsCode.TryParse(nmvsData, out NmvsCode nmvsCode))
            {
                ProductCode = nmvsCode.ProductCode;
                BatchId = nmvsCode.BatchId;
                SerialNo = nmvsCode.SerialNo;
                BatchExpDate = nmvsCode.ExpireDate.ToString();
            }
        }

        protected override string GetResultData()
        {
            return GetNmvsCode().ToString();
        }

        protected override bool Validate()
        {
            try
            {
                var nmvsCode = GetNmvsCode();
                return true;
            }
            catch (Exception exc)
            {
                dialogsService.ShowException(null, exc);
                return false;
            }
        }
    }
}