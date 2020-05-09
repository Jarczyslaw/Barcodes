﻿using Barcodes.Codes;
using Barcodes.Core.Abstraction;
using Barcodes.Core.Models;
using System;

namespace Barcodes.Core.ViewModels.Templates
{
    public class ContainerViewModel : BaseTemplateViewModel
    {
        private int containerNumber;

        public ContainerViewModel(IAppDialogsService dialogsService)
            : base(dialogsService)
        {
        }

        public int ContainerNumber
        {
            get => containerNumber;
            set => SetProperty(ref containerNumber, value);
        }

        public override void LoadData(string data)
        {
            if (ContainerCode.TryParse(data, out ContainerCode containerCode))
            {
                ContainerNumber = containerCode.ContainerNumber;
            }
        }

        protected override TemplateResult GetResultData()
        {
            var code = new ContainerCode(ContainerNumber);
            return new TemplateResult
            {
                BarcodeType = code.Type,
                Data = code.Code
            };
        }

        protected override bool Validate()
        {
            try
            {
                new ContainerCode(ContainerNumber);
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