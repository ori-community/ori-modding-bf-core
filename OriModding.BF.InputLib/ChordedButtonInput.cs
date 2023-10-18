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
            sb.Append(button.ToFriendlyString());
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
            sb.Append(CustomInput.SerialiseButton(button));
            sb.Append("+");
        }
        return sb.ToString().TrimEnd('+');
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
}
