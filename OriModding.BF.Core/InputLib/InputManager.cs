using BepInEx;
using BepInEx.Configuration;
using OriModding.BF.Core;
using System;
using System.Collections.Generic;

namespace OriModding.BF.InputLib;

public class InputManager
{
    private readonly List<ConfigEntry<CustomInput>> customInputs = new List<ConfigEntry<CustomInput>>();

    public InputManager()
    {
        On.PlayerInput.ClearControls += (orig, self) =>
        {
            orig(self);

            Plugin.Logger.LogInfo("Clearing controls");
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
            Plugin.Logger.LogError(ex);
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
}
