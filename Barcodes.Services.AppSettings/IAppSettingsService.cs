namespace Barcodes.Services.AppSettings
{
    public interface IAppSettingsService
    {
        string StoragePath { get; set; }
        bool BarcodesVisible { get; set; }

        AppSettings AppSettings { get; }
        string AppSettingsPath { get; }

        void Load(bool throwException = false);

        void Save();
    }
}