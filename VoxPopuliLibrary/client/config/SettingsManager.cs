using Tomlyn.Model;
using Tomlyn;
using OpenTK.Windowing.Desktop;

namespace VoxPopuliLibrary.client.config
{
    public static class SettingsManager
    {
        private static Dictionary<string, ISetting> settings = new Dictionary<string, ISetting>();
        private static TomlTable settingsTable;
        const string settingsFilePath = "data/settings.toml";
        public static  GameWindow Window;

        internal static void PepareInternalSettings(GameWindow window)
        {
            Window = window;
        }
        internal static void LoadSettings()
        {
            settingsTable = Toml.ToModel(File.ReadAllText(settingsFilePath));
            foreach(var setting in settings.Values)
            {
                setting.OnDataUpdated();
            }
        }

        public static T GetSetting<T>(string settingName)
        {
            if (settings.TryGetValue(settingName, out var setting))
            {
                return (T)setting.GetData();
            }

            throw new KeyNotFoundException($"Setting '{settingName}' not found.");
        }

        public static void AddSetting<T>(string settingName, Setting<T> setting)
        {
            if (!settings.ContainsKey(settingName))
            {
                settings[settingName] = setting;
            }
        }

        public static void UpdateSetting<T>(string settingName, T value)
        {
            if (settings.TryGetValue(settingName, out var setting))
            {
                setting.SetData(value);
                settingsTable[settingName] = value;

                SaveSettings();
            }
        }

        private static void SaveSettings()
        {
            File.WriteAllText(settingsFilePath,settingsTable.ToString());
        }
    }

}
