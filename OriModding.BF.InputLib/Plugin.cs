using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using OriModding.BF.Core;
using System;
using System.Collections.Generic;

namespace OriModding.BF.InputLib;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency(Core.PluginInfo.PLUGIN_GUID)]
[BepInDependency(ConfigMenu.PluginInfo.PLUGIN_GUID, BepInDependency.DependencyFlags.SoftDependency)]
public class Plugin : BaseUnityPlugin
{
    private readonly List<ConfigEntry<CustomInput>> customInputs = new List<ConfigEntry<CustomInput>>();

    internal static new ManualLogSource Logger;

    private void Awake()
    {
        Logger = base.Logger;

        // Add converter so the config manager recognises the CustomInput format in the .cfg
        TomlTypeConverter.AddConverter(typeof(CustomInput), new TypeConverter
        {
            ConvertToString = (obj, type) => ((CustomInput)obj).Serialise(),
            ConvertToObject = (str, type) => CustomInput.FromString(str)
        });

        if (this.TryGetPlugin(ConfigMenu.PluginInfo.PLUGIN_GUID, out var configMenuPlugin))
        {
            var configMenu = configMenuPlugin as ConfigMenu.Plugin;
            configMenu.AddConfigType<CustomInput>((screen, c) => screen.AddInputBind(c as ConfigEntry<CustomInput>));
        }


        AddDetours();

        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    public ConfigEntry<CustomInput> BindAndRegister(BaseUnityPlugin plugin, string section, string key, CustomInput defaultValue)
    {
        try
        {
            ConfigEntry<CustomInput> configEntry = plugin.Config.Bind(section, key, defaultValue);

            RegisterCustomInput(configEntry);

            return configEntry;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            throw;
        }
    }

    public void RegisterCustomInput(ConfigEntry<CustomInput> customInput)
    {
        if (!customInputs.Contains(customInput))
        {
            customInputs.Add(customInput);
        }
    }

    private void AddDetours()
    {
        On.PlayerInput.ClearControls += (orig, self) =>
        {
            orig(self);

            Logger.LogInfo("Clearing");
            //foreach (var input in customInputs)
            //    input.Value.Clear();
        };

        On.PlayerInput.AddKeyboardControls += (orig, self) =>
        {
            orig(self);
            //AddCustomKeyboardControls();
            //Logger.LogInfo("Reloading controls");
            //foreach (var input in customInputs)
            //{
            //    //var sv = input.GetSerializedValue();
            //    //Logger.LogInfo(sv);

            //    //input.SetSerializedValue(sv);
            //    input.Value.Reload();
            //}
            //Config.Reload();
        };

        On.PlayerInput.FixedUpdate += (orig, self) =>
        {
            orig(self);

            foreach (var input in customInputs)
                input.Value.Update();
        };

        //On.PlayerInput.AddControllerControls += (orig, self) =>
        //{
        //    AddCustomControllerControls();
        //    AddDefaultControllerControls();
        //};
    }
}
