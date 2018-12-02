﻿using Barcodes.Services.Generator;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Barcodes.Core.ViewModels
{
    public class BarcodeResultViewModel : BindableBase
    {
        public string HeaderTitle
        {
            get { return $"Barcodes - {Title}"; }
        }

        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public string TypeTitle
        {
            get => BarcodeType.ToString();
        }

        public BarcodeType BarcodeType { get; set; }

        private string data;
        public string Data
        {
            get => data;
            set => SetProperty(ref data, value);
        }

        public int BarcodeWidth
        {
            get => Barcode.PixelWidth;
        }

        public int BarcodeHeight
        {
            get => Barcode.PixelHeight;
        }

        private BitmapSource barcode;
        public BitmapSource Barcode
        {
            get => barcode;
            set => SetProperty(ref barcode, value);
        }
    }
}
