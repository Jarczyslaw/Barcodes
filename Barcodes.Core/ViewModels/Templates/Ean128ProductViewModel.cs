﻿using Barcodes.Codes;
using Barcodes.Core.Abstraction;
using Barcodes.Core.Models;
using System;

namespace Barcodes.Core.ViewModels.Templates
{
    public class Ean128ProductViewModel : BaseTemplateViewModel
    {
        private string productCode = string.Empty;
        private string batchId = string.Empty;
        private DateTime expireDate = DateTime.Now;

        public Ean128ProductViewModel(IAppDialogsService dialogsService)
            : base(dialogsService)
        {
        }

        public DateTime ExpireDate
        {
            get => expireDate;
            set => SetProperty(ref expireDate, value);
        }

        public string BatchId
        {
            get => batchId;
            set => SetProperty(ref batchId, value);
        }

        public string ProductCode
        {
            get => productCode;
            set => SetProperty(ref productCode, value);
        }

        private Ean128Code GetEan128Code()
        {
            return new Ean128Code(ProductCode.Trim(), BatchId.Trim(), ExpireDate);
        }

        public override void LoadData(string ean128Data)
        {
            if (Ean128Code.TryParse(ean128Data, out Ean128Code ean128Code))
            {
                ExpireDate = ean128Code.ExpireDate;
                BatchId = ean128Code.BatchId;
                ProductCode = ean128Code.ProductCode;
            }
        }

        protected override TemplateResult GetResultData()
        {
            return new TemplateResult(GetEan128Code());
        }

        protected override bool Validate()
        {
            try
            {
                GetEan128Code();
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