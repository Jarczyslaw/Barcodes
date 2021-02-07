namespace Barcodes.Services.AppSettings
{
    public interface IAppSettingsService
    {
        StartupMode StartupMode { get; set; }
        int QuickBarcodesCount { get; set; }
        string StoragePath { get; set; }
        bool BarcodesVisible { get; set; }
        AddMode BarcodeAddMode { get; set; }
        AddMode WorkspaceAddMode { get; set; }
        string AntiKeyProtection { get; set; }
        GenerationSettings GenerationSettings { get; set; }
        bool UpdateAfterEveryGeneration { get; set; }

        AppSettings AppSettings { get; }
        string AppSettingsPath { get; }

        void Load(bool throwException = false);

        void Save();

        void Save(AppSettings appSettings);

        void Save(string settings);

        void TryUpdateGenerationSettings(GenerationSettings generationSettings);
    }
}