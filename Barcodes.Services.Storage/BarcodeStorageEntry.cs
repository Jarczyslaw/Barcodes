﻿using Barcodes.Services.Generator;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcodes.Services.Storage
{
    public class BarcodeStorageEntry
    {
        public string Title { get; set; }
        public string Data { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public BarcodeType Type { get; set; }
    }
}
