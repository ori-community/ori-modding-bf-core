using BepInEx;
using BepInEx.Bootstrap;
using CSVFile;
using Game;
using System;
using System.Collections.Generic;
using System.IO;

namespace OriModding.BF.l10n;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal Dictionary<string, string> strings = new Dictionary<string, string>();

    private void Awake()
    {
        Core.Hooks.Hooks.OnGameControllerInitialised += () =>
        {
            Events.Scheduler.OnGameLanguageChange.Add(OnLanguageChanged);
        };

        Strings.plugin = this;
    }

    private void OnDestroy()
    {
        Events.Scheduler.OnGameLanguageChange.Remove(OnLanguageChanged);
    }

    private void OnLanguageChanged()
    {
        ReloadStrings(GameSettings.Instance.Language);
    }

    public void ReloadStrings(Language language)
    {
        foreach (var plugin in Chainloader.PluginInfos.Values)
        {
            string path = Path.Combine(Path.GetDirectoryName(plugin.Location), "strings.csv");
            if (File.Exists(path))
            {
                LoadLanguage(path, language);
            }
        }
    }

    private void LoadLanguage(string path, Language language)
    {
        Logger.LogInfo("Loading strings from " + path);
        using var reader = CSVReader.FromFile(path);

        int index = Find(reader.Headers, language.ToString());
        if (index == -1)
        {
            Logger.LogWarning($"No language info found for {language}");
            return;
        }

        foreach (var line in reader.Lines())
        {
            string str = line[index];
            string key = line[0];
            if (!str.IsNullOrWhiteSpace())
                strings[key] = str;
            else if (index != 1)
                strings[key] = line[1];
            else
                strings[key] = $"ERROR: Missing string \"{key}\"";
        }
    }

    private int Find(string[] headers, string key)
    {
        for (int i = 0; i < headers.Length; i++)
        {
            if (headers[i].Equals(key, StringComparison.OrdinalIgnoreCase))
                return i;
        }
        return -1;
    }
}
