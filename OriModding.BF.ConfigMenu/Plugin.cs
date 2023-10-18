using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using OriModding.BF.UiLib.Menu;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OriModding.BF.ConfigMenu;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency(UiLib.PluginInfo.PLUGIN_GUID)]
[BepInDependency(Core.PluginInfo.PLUGIN_GUID)]
public class Plugin : BaseUnityPlugin
{
    private readonly Dictionary<string, SliderProps> sliderProps = new();
    private readonly List<string> hiddenKeys = new();
    private readonly Dictionary<Type, AddCustomControlCallback> customControlCallbacks = new();

    private void Start()
    {
        Logger.LogInfo("Initialising config screens");

        Dictionary<string, List<ConfigEntryBase>> allConfigs = new();

        var plugins = Chainloader.PluginInfos;
        foreach (var plugin in plugins.Values)
        {
            foreach (var config in plugin.Instance.Config)
            {
                if (hiddenKeys.Contains(config.Value.ConfigKey()))
                    continue;

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
                    else if (customControlCallbacks.ContainsKey(config.SettingType))
                    {
                        customControlCallbacks[config.SettingType](screen, config);
                    }
                }
            });
        }
    }

    public delegate void AddCustomControlCallback(CustomOptionsScreen screen, ConfigEntryBase configEntry);
    public void AddConfigType<T>(AddCustomControlCallback callback)
    {
        customControlCallbacks[typeof(T)] = callback;
    }

    public void Hide(params ConfigEntryBase[] configs)
    {
        hiddenKeys.AddRange(configs.Select(x => x.ConfigKey()));
    }

    public void ConfigureSlider(ConfigEntryBase config, float min, float max, float step)
    {
        sliderProps[config.ConfigKey()] = new SliderProps
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

static class ConfigEntryExtensions
{
    internal static string ConfigKey(this ConfigEntryBase configEntry)
        => $"{configEntry.Definition.Section}.{configEntry.Definition.Key}";
}
