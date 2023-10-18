using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using SmartInput;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace OriModding.BF.InputLib;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency(Core.PluginInfo.PLUGIN_GUID)]
public class Plugin : BaseUnityPlugin
{
    private readonly List<ConfigEntry<CustomInput>> customInputs = new List<ConfigEntry<CustomInput>>();
    private CustomInput button;

    internal static new ManualLogSource Logger;

    private void Awake()
    {
        Logger = base.Logger;

        TomlTypeConverter.AddConverter(typeof(CustomInput), new TypeConverter
        {
            ConvertToString = (obj, type) => ((CustomInput)obj).Serialise(),
            ConvertToObject = (str, type) => CustomInput.FromString(str)
        });

        button = BindAndRegister(this, "INPUT", "Stomp", new CustomInput().AddKeyCodes(KeyCode.I)).Value;

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
        if (button.OnPressed)
        {
            Logger.LogInfo("Pressed the button!");
        }
        else if (button.OnReleased)
        {
            Logger.LogInfo("Released the button!");
            button.Clear();
            button.AddKeyCodes(KeyCode.I);
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

public class CustomInput
{
    private readonly global::Core.Input.InputButtonProcessor processor = new();
    private readonly CompoundButtonInput input = new();
    //private string lastLoadedString;

    public bool Used
    {
        get => processor.Used;
        set => processor.Used = value;
    }

    public bool OnPressed => processor.OnPressed;
    public bool OnPressedNotUsed => processor.OnPressedNotUsed;
    public bool OnReleased => processor.OnReleased;
    public bool Pressed => processor.Pressed;
    public bool Released => processor.Released;

    public void Clear()
    {
        input.Clear();
    }

    public void Update()
    {
        processor.Update(input.GetButton());
    }

    //internal void Reload()
    //{
    //    Clear();
    //    //LoadFromString(lastLoadedString);
    //}

    public CustomInput AddKeyCodes(params KeyCode[] keyCodes)
    {
        for (int i = 0; i < keyCodes.Length; i++)
            input.Add(new KeyCodeButtonInput(keyCodes[i]));
        return this;
    }

    internal string Serialise()
    {
        if (input.Buttons == null)
            return null;

        var sb = new StringBuilder();
        foreach (var button in input.Buttons)
        {
            if (button is KeyCodeButtonInput kcbi)
            {
                sb.Append(kcbi.KeyCode.ToString());
            }
        }
        return sb.ToString().TrimEnd(',');
    }

    internal static object FromString(string str)
    {
        var input = new CustomInput();

        if (string.IsNullOrEmpty(str))
            return input;

        input.LoadFromString(str);
        return input;
    }

    private void LoadFromString(string str)
    {
        Plugin.Logger.LogInfo("Loading from string: " + str);
        //lastLoadedString = str;

        string[] parts = str.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var part in parts)
        {
            if (TryGetEnum<KeyCode>(part, out var keyCode))
                AddKeyCodes(keyCode);
        }
    }

    private static bool TryGetEnum<TEnum>(string value, out TEnum result) where TEnum : Enum
    {
        var defined = value != null && Enum.IsDefined(typeof(TEnum), value);
        result = defined ? (TEnum)Enum.Parse(typeof(TEnum), value) : default;
        return defined;
    }
}
