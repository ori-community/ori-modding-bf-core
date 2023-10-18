using SmartInput;
using System;
using System.Text;
using UnityEngine;

namespace OriModding.BF.InputLib;

/// <summary>
/// An input that is considered pressed when all buttons in the chord are pressed. e.g. Alt + R
/// </summary>
public class ChordedButtonInput : IButtonInput
{
    public IButtonInput[] Buttons;

    public bool GetButton()
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            if (!Buttons[i].GetButton())
                return false;
        }
        return true;
    }

    internal string ToFriendlyString()
    {
        if (Buttons == null)
            return null;

        var sb = new StringBuilder();
        foreach (var button in Buttons)
        {
            if (button is KeyCodeButtonInput kcbi)
                sb.Append(kcbi.KeyCode.KeyCodeToButtonIcon());
            sb.Append("+");
        }
        return sb.ToString().TrimEnd('+');
    }

    internal string Serialise()
    {
        if (Buttons == null)
            return null;

        var sb = new StringBuilder();
        foreach (var button in Buttons)
        {
            if (button is KeyCodeButtonInput kcbi)
                sb.Append(kcbi.KeyCode);
            sb.Append("+");
        }
        return sb.ToString().TrimEnd('+');
    }

    internal static ChordedButtonInput FromString(string str)
    {
        var input = new ChordedButtonInput();

        if (string.IsNullOrEmpty(str))
            return input;

        input.LoadFromString(str);
        return input;
    }

    internal void LoadFromString(string str)
    {
        string[] parts = str.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var part in parts)
        {
            if (TryGetEnum<KeyCode>(part, out var keyCode))
                AddKeyCodes(keyCode);
        }
    }

    public void AddKeyCodes(params KeyCode[] keyCodes)
    {
        for (int i = 0; i < keyCodes.Length; i++)
            Add(new KeyCodeButtonInput(keyCodes[i]));
    }

    public void Add(IButtonInput button)
    {
        if (Buttons == null)
        {
            Buttons = new IButtonInput[] { button };
            return;
        }
        Array.Resize(ref Buttons, Buttons.Length + 1);
        Buttons[Buttons.Length - 1] = button;
    }

    private static bool TryGetEnum<TEnum>(string value, out TEnum result) where TEnum : Enum
    {
        var defined = value != null && Enum.IsDefined(typeof(TEnum), value);
        result = defined ? (TEnum)Enum.Parse(typeof(TEnum), value) : default;
        return defined;
    }
}
