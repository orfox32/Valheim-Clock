using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using HarmonyLib;
using System.IO;
using System.Reflection;

namespace ClockMod
{
    public enum TextLayoutStyle { SingleLine, Stacked }

    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInProcess("valheim.exe")]
    [BepInDependency("randyknapp.mods.minimalstatuseffects", BepInDependency.DependencyFlags.SoftDependency)]
    public class ClockPlugin : BaseUnityPlugin
    {
        public const string PluginGUID = "com.orfox.valheimclock";
        public const string PluginName = "Valheim Clock";
        public const string PluginVersion = "1.3.1";

        private readonly Harmony _harmony = new Harmony(PluginGUID);

        public static bool HasMSE { get; private set; }

        public static ConfigEntry<float> ConfigPosX;
        public static ConfigEntry<float> ConfigPosY;
        public static ConfigEntry<float> ConfigWidth;
        public static ConfigEntry<float> ConfigHeight;
        public static ConfigEntry<int> ConfigFontSize;
        public static ConfigEntry<bool> ConfigUse12HourFormat;
        public static ConfigEntry<TextLayoutStyle> ConfigLayoutStyle;

        private void Awake()
        {

            HasMSE = Chainloader.PluginInfos.ContainsKey("randyknapp.mods.minimalstatuseffects");

            ConfigPosX = Config.Bind("Position", "Offset X", 0f, "Horizontal offset relative to the bottom center of the minimap.");
            ConfigPosY = Config.Bind("Position", "Offset Y", -10f, "Vertical offset relative to the bottom center of the minimap.");
            ConfigWidth = Config.Bind("Visuals", "Panel Width", 160f, "Width of the clock panel.");
            ConfigHeight = Config.Bind("Visuals", "Panel Height", 34f, "Height of the clock panel.");
            ConfigFontSize = Config.Bind("Visuals", "Font Size", 18, "Size of the clock text.");
            ConfigUse12HourFormat = Config.Bind("General", "Use12HourFormat", false, "If true, displays the clock in a 12-hour format with AM/PM instead of 24-hour format time.");
            ConfigLayoutStyle = Config.Bind("Visuals", "Text Layout", TextLayoutStyle.SingleLine, "Day and time arrangement.");

            var orphanedEntriesProp = typeof(ConfigFile).GetProperty("OrphanedEntries", BindingFlags.NonPublic | BindingFlags.Instance);
            if (orphanedEntriesProp != null)
            {
                var orphanedEntries = orphanedEntriesProp.GetValue(Config, null) as System.Collections.IDictionary;
                orphanedEntries?.Clear();
            }

            _harmony.PatchAll();
            Logger.LogInfo("ClockMod initialized.");

            SetupConfigWatcher();
        }

        private void SetupConfigWatcher()
        {
            var watcher = new FileSystemWatcher(Paths.ConfigPath, $"{PluginGUID}.cfg")
            {
                IncludeSubdirectories = true,
                SynchronizingObject = ThreadingHelper.SynchronizingObject,
                EnableRaisingEvents = true
            };

            watcher.Changed += ReloadConfig;
            watcher.Created += ReloadConfig;
            watcher.Renamed += ReloadConfig;
        }

        private void ReloadConfig(object sender, FileSystemEventArgs e)
        {
            if (!File.Exists(Config.ConfigFilePath)) return;

            try
            {
                Config.Reload();
                ClockUI.ApplyConfigs();
            }
            catch { /* Ignore lock exceptions during rapid Notepad saves */ }
        }
    }
}
