using SmartInput;
using System;
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
            else if (button is ChordedButtonInput cbi)
            {
                sb.Append(cbi.Serialise());
            }
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
        string[] parts = str.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var part in parts)
        {
            if (part.Contains("+"))
                input.Add(ChordedButtonInput.FromString(part));
            else if (TryGetEnum<KeyCode>(part, out var keyCode))
                AddKeyCodes(keyCode);
        }
    }

    private static bool TryGetEnum<TEnum>(string value, out TEnum result) where TEnum : Enum
    {
        var defined = value != null && Enum.IsDefined(typeof(TEnum), value);
        result = defined ? (TEnum)Enum.Parse(typeof(TEnum), value) : default;
        return defined;
    }

    public string ToFriendlyString()
    {
        if (input.Buttons == null)
            return null;

        var sb = new StringBuilder();
        foreach (var button in input.Buttons)
        {
            if (button is KeyCodeButtonInput kcbi)
                sb.Append(kcbi.KeyCode.KeyCodeToButtonIcon());
            else if (button is ChordedButtonInput cbi)
                sb.Append(cbi.ToFriendlyString());
            sb.Append(", ");
        }
        return sb.ToString().TrimEnd(',', ' ');
    }
}
