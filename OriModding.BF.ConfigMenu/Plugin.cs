using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using OriModding.BF.UiLib.Menu;
using System.Collections.Generic;

namespace OriModding.BF.ConfigMenu;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency(UiLib.PluginInfo.PLUGIN_GUID)]
[BepInDependency(Core.PluginInfo.PLUGIN_GUID)]
public class Plugin : BaseUnityPlugin
{
    private readonly Dictionary<string, SliderProps> sliderProps = new();

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
                    {
                        var props = GetSliderProps(config);
                        screen.AddSlider(config as ConfigEntry<float>, config.Definition.Key, props.Min, props.Max, props.Step, config.Description.Description);
                    }
                }
            });
        }
    }

    public void ConfigureSlider(ConfigEntryBase config, float min, float max, float step)
    {
        sliderProps[$"{config.Definition.Section}.{config.Definition.Key}"] = new SliderProps
        {
            Min = min,
            Max = max,
            Step = step
        };
    }

    private SliderProps GetSliderProps(ConfigEntryBase config)
    {
        if (sliderProps.TryGetValue($"{config.Definition.Section}.{config.Definition.Key}", out var props))
            return props;
        return new SliderProps();
    }
}

internal struct SliderProps
{
    public float Min { get; set; } = 0f;
    public float Max { get; set; } = 1f;
    public float Step { get; set; } = 0.1f;

    public SliderProps() { }
}
