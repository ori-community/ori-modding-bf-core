using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using OriModding.BF.Core;
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

        // Add converter so the config manager recognises the CustomInput format in the .cfg
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
        BindAndRegister(this, "RANDO", "1Return to Start", new CustomInput().AddChord(KeyCode.LeftAlt, KeyCode.R));
        BindAndRegister(this, "RANDO", "R2eload Seed", new CustomInput().AddChord(KeyCode.LeftAlt, KeyCode.L));
        BindAndRegister(this, "RANDO", "Sh3ow Stats", new CustomInput().AddChord(KeyCode.LeftAlt, KeyCode.Alpha3));
        BindAndRegister(this, "RANDO", "Sho4w Progress", new CustomInput().AddChord(KeyCode.LeftAlt, KeyCode.T));
        BindAndRegister(this, "RANDO", "Show5 Last Pickup", new CustomInput().AddChord(KeyCode.LeftAlt, KeyCode.P));
        BindAndRegister(this, "RANDO", "Retur6n to Start", new CustomInput().AddChord(KeyCode.LeftAlt, KeyCode.R));
        BindAndRegister(this, "RANDO", "Reload7 Seed", new CustomInput().AddChord(KeyCode.LeftAlt, KeyCode.L));
        BindAndRegister(this, "RANDO", "Show St8ats", new CustomInput().AddChord(KeyCode.LeftAlt, KeyCode.Alpha3));
        BindAndRegister(this, "RANDO", "Show Pro9gress", new CustomInput().AddChord(KeyCode.LeftAlt, KeyCode.T));
        BindAndRegister(this, "RANDO", "Show Last0 Pickup", new CustomInput().AddChord(KeyCode.LeftAlt, KeyCode.P));
        BindAndRegister(this, "RANDO", "Return to 1Start", new CustomInput().AddChord(KeyCode.LeftAlt, KeyCode.R));
        BindAndRegister(this, "RANDO", "Reload See2d", new CustomInput().AddChord(KeyCode.LeftAlt, KeyCode.L));
        BindAndRegister(this, "RANDO", "Show Stats3", new CustomInput().AddChord(KeyCode.LeftAlt, KeyCode.Alpha3));
        BindAndRegister(this, "RANDO", "Show Prog5ress", new CustomInput().AddChord(KeyCode.LeftAlt, KeyCode.T));
        BindAndRegister(this, "RANDO", "Show Last4 Pickup", new CustomInput().AddChord(KeyCode.LeftAlt, KeyCode.P));
        BindAndRegister(this, "RANDO", "Return t6o Start", new CustomInput().AddChord(KeyCode.LeftAlt, KeyCode.R));
        BindAndRegister(this, "RANDO", "Reload 7Seed", new CustomInput().AddChord(KeyCode.LeftAlt, KeyCode.L));
        BindAndRegister(this, "RANDO", "Show 8Stats", new CustomInput().AddChord(KeyCode.LeftAlt, KeyCode.Alpha3));
        BindAndRegister(this, "RANDO", "Show Pr9ogress", new CustomInput().AddChord(KeyCode.LeftAlt, KeyCode.T));
        BindAndRegister(this, "RANDO", "Show1 Last Pickup", new CustomInput().AddChord(KeyCode.LeftAlt, KeyCode.P));


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
