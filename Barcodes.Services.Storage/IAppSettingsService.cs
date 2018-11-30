using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcodes.Services.Storage
{
    public interface IAppSettingsService
    {
        AppSettings AppSettings { get; }
        string AppSettingsPath { get; }
        void Load(bool throwException = false);
        void Save();
    }
}
