using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using OriModding.BF.Core;
using OriModding.BF.UiLib.Menu;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace OriModding.BF.InputLib;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency(Core.PluginInfo.PLUGIN_GUID)]
[BepInDependency(UiLib.PluginInfo.PLUGIN_GUID, BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency(ConfigMenu.PluginInfo.PLUGIN_GUID, BepInDependency.DependencyFlags.SoftDependency)]
public class Plugin : BaseUnityPlugin
{
    private readonly List<ConfigEntry<CustomInput>> customInputs = new List<ConfigEntry<CustomInput>>();

    internal static new ManualLogSource Logger;
    private ConfigEntry<CustomInput> button;

    private void Awake()
    {
        Logger = base.Logger;

        TomlTypeConverter.AddConverter(typeof(CustomInput), new TypeConverter
        {
            ConvertToString = (obj, type) => ((CustomInput)obj).Serialise(),
            ConvertToObject = (str, type) => CustomInput.FromString(str)
        });

        if (this.TryGetPlugin(ConfigMenu.PluginInfo.PLUGIN_GUID, out ConfigMenu.Plugin configMenu))
        {
            configMenu.AddConfigType<CustomInput>((screen, c) => screen.AddInputBind(c as ConfigEntry<CustomInput>));
        }

        button = BindAndRegister(this, "INPUT", "Stomp", new CustomInput().AddKeyCodes(KeyCode.I));
        BindAndRegister(this, "RANDO", "Return to Start", new CustomInput().AddChord(KeyCode.LeftAlt, KeyCode.R));
        BindAndRegister(this, "RANDO", "Reload Seed", new CustomInput().AddChord(KeyCode.LeftAlt, KeyCode.L));
        BindAndRegister(this, "RANDO", "Show Stats", new CustomInput().AddChord(KeyCode.LeftAlt, KeyCode.Alpha3));
        BindAndRegister(this, "RANDO", "Show Progress", new CustomInput().AddChord(KeyCode.LeftAlt, KeyCode.T));
        BindAndRegister(this, "RANDO", "Show Last Pickup", new CustomInput().AddChord(KeyCode.LeftAlt, KeyCode.P));

        AddDetours();

        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

        CustomMenuManager.RegisterOptionsScreen<CustomOptionsScreen>("TESTINPUT", 0, screen =>
        {
            screen.AddInputBind(button);
        });
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

    void FixedUpdate()
    {
        if (button.Value.OnPressed)
        {
            Logger.LogInfo("Pressed the button!");
        }
        else if (button.Value.OnReleased)
        {
            Logger.LogInfo("Released the button!");
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
