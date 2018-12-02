using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcodes.Services.AppSettings
{
    public interface IAppSettingsService
    {
        string StoragePath { get; set; }
        AppSettings AppSettings { get; }
        string AppSettingsPath { get; }
        void Load(bool throwException = false);
        void Save();
    }
}
