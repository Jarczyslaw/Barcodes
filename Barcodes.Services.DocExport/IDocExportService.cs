﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcodes.Services.DocExport
{
    public interface IDocExportService
    {
        void Export(string filePath);
    }
}