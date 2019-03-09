using Barcodes.Core.ViewModels;
using Barcodes.Services.Generator;
using Barcodes.Services.Storage;
using System;

namespace Barcodes.Core.Extensions
{
    public static class StorageWorkspaceExtensions
    {
        public static WorkspaceViewModel ToWorkspace(this StorageWorkspace storageWorkspace, IGeneratorService generatorService)
        {
            var newWorkspace = new WorkspaceViewModel
            {
                Name = storageWorkspace.Title,
                Default = storageWorkspace.Default
            };

            foreach (var storageBarcode in storageWorkspace.Barcodes)
            {
                if (!storageBarcode.IsValid || !storageBarcode.ValidSizes)
                {
                    throw new Exception($"Barcode {storageBarcode.Title} can not be generated due to invalid sizes");
                }

                var barcode = storageBarcode.ToBarcode(generatorService);
                newWorkspace.Barcodes.Add(barcode);
            }

            return newWorkspace;
        }
    }
}
