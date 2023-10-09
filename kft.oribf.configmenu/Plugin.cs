using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using kft.oribf.uilib;
using System.Collections.Generic;

namespace kft.oribf.configmenu;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency(kft.oribf.uilib.PluginInfo.PLUGIN_GUID)]
public class Plugin : BaseUnityPlugin
{
    private void Start()
    {
        Logger.LogInfo("Initialising config screens");

        Dictionary<string, List<ConfigEntryBase>> allConfigs = new();

        var plugins = Chainloader.PluginInfos;
        foreach (var plugin in plugins.Values)
        {
            foreach (var config in plugin.Instance.Config)
            {
                if (!allConfigs.ContainsKey(config.Key.Section))
                    allConfigs.Add(config.Key.Section, new List<ConfigEntryBase>());
                allConfigs[config.Key.Section].Add(config.Value);
            }
        }

        foreach (var kvp in allConfigs)
        {
            CustomMenuManager.RegisterOptionsScreen<AutoOptionsScreen>(kvp.Key, 5, screen =>
            {
                Logger.LogDebug($"Setting up config screen \"{kvp.Key}\"");

                foreach (var config in kvp.Value)
                {
                    if (config.SettingType == typeof(bool))
                        screen.AddToggle(config as ConfigEntry<bool>, config.Definition.Key, config.Description.Description);
                    else if (config.SettingType == typeof(float))
                        screen.AddSlider(config as ConfigEntry<float>, config.Definition.Key, 0f, 1f, 0.1f, config.Description.Description);
                }
            });
        }
    }
}
