using SmartInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace OriModding.BF.InputLib;

public class CustomInput
{
    private readonly global::Core.Input.InputButtonProcessor processor = new();
    internal readonly CompoundButtonInput input = new();

    public bool Used
    {
        get => processor.Used;
        set => processor.Used = value;
    }

    /// <summary>Was pressed this frame</summary>
    public bool OnPressed => processor.OnPressed;
    /// <summary>Was pressed this frame, but has not been used</summary>
    public bool OnPressedNotUsed => processor.OnPressedNotUsed;
    /// <summary>Was released this frame</summary>
    public bool OnReleased => processor.OnReleased;
    /// <summary>Is currently pressed</summary>
    public bool Pressed => processor.Pressed;
    /// <summary>Is currently not pressed</summary>
    public bool Released => processor.Released;

    public void Clear()
    {
        input.Clear();
    }

    public void Update()
    {
        processor.Update(input.GetButton());
    }

    public CustomInput AddKeyCodes(params KeyCode[] keyCodes)
    {
        for (int i = 0; i < keyCodes.Length; i++)
            input.Add(new KeyCodeButtonInput(keyCodes[i]));
        return this;
    }

    public CustomInput AddChord(params KeyCode[] keyCodes)
    {
        var chord = new ChordedButtonInput();
        chord.AddKeyCodes(keyCodes);
        input.Add(chord);
        return this;
    }

    public CustomInput AddControllerButtons(params ControllerButton[] buttons)
    {
        for (int i = 0; i < buttons.Length; i++)
            input.Add(buttons[i].ToButtonInput());
        return this;
    }

    internal string Serialise()
    {
        if (input.Buttons == null)
            return null;

        var sb = new StringBuilder();
        foreach (var button in input.Buttons)
        {
            sb.Append(SerialiseButton(button));
            sb.Append(",");
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

    internal void LoadFromString(string str)
    {
        input.Buttons = ParseButtons(str);
    }

    private static bool TryGetEnum<TEnum>(string value, out TEnum result) where TEnum : Enum
    {
        var defined = value != null && Enum.IsDefined(typeof(TEnum), value);
        result = defined ? (TEnum)Enum.Parse(typeof(TEnum), value) : default;
        return defined;
    }

    internal string ToFriendlyString()
    {
        if (input.Buttons == null)
            return null;

        var sb = new StringBuilder();
        foreach (var button in input.Buttons)
        {
            sb.Append(button.ToFriendlyString());
            sb.Append(", ");
        }
        return sb.ToString().TrimEnd(',', ' ');
    }

    public static IButtonInput[] ParseButtons(string input, char delimiter = ',')
    {
        List<IButtonInput> buttons = new List<IButtonInput>();

        string[] parts = input.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var part in parts)
        {
            if (part.Contains("+"))
            {
                buttons.Add(new ChordedButtonInput()
                {
                    Buttons = ParseButtons(part, '+').ToArray()
                });
                continue;
            }

            if (TryGetEnum<ControllerButton>(part, out var button))
                buttons.Add(button.ToButtonInput());
            else if (TryGetEnum<KeyCode>(part, out var keyCode))
                buttons.Add(new KeyCodeButtonInput(keyCode));
        }

        return buttons.ToArray();
    }

    internal static string SerialiseButton(IButtonInput button)
    {
        if (button is KeyCodeButtonInput kcbi)
            return kcbi.KeyCode.ToString();
        if (button is ControllerButtonInput controllerBI)
            return controllerBI.ToControllerButton().ToString();
        if (button is AxisButtonInput axisInput)
            return axisInput.ToControllerButton().ToString();
        if (button is ChordedButtonInput cbi)
            return cbi.Serialise();

        return null;
    }
}
