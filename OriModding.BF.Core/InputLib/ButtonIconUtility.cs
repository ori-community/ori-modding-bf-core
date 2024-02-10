using SmartInput;
using UnityEngine;

namespace OriModding.BF.InputLib;

internal static class ButtonIconUtility
{
    public static string ToFriendlyString(this IButtonInput button)
    {
        if (button is KeyCodeButtonInput kcbi)
            return kcbi.KeyCode.KeyCodeToButtonIcon();
        else if (button is ControllerButtonInput controllerBI)
            return controllerBI.ToControllerButton().ButtonToIcon();
        else if (button is AxisButtonInput axisInput)
            return axisInput.ToControllerButton().ButtonToIcon();
        else if (button is ChordedButtonInput cbi)
            return cbi.ToFriendlyString();
        return null;
    }

    public static string KeyCodeToButtonIcon(this KeyCode keyCode)
    {
        switch (keyCode)
        {
            case KeyCode.A: return "<icon>n</>";
            case KeyCode.B: return "<icon>o</>";
            case KeyCode.C: return "<icon>p</>";
            case KeyCode.D: return "<icon>q</>";
            case KeyCode.DownArrow: return "<icon>r</>";
            case KeyCode.LeftArrow: return "<icon>s</>";
            case KeyCode.RightArrow: return "<icon>t</>";
            case KeyCode.S: return "<icon>u</>";
            case KeyCode.UpArrow: return "<icon>v</>";
            case KeyCode.W: return "<icon>w</>";
            case KeyCode.X: return "<icon>x</>";
            case KeyCode.Escape: return "<icon>y</>";
            case KeyCode.Tab: return "<icon>z</>";
            case KeyCode.V: return "<icon>A</>";
            case KeyCode.Z: return "<icon>B</>";
            case KeyCode.Space: return "<icon>C</>";
            case KeyCode.Return: return "<icon>D</>";
            case KeyCode.Mouse0: return "<icon>E</>";
            case KeyCode.Mouse1: return "<icon>F</>";
            case KeyCode.LeftControl: return "<icon>G</>";
            case KeyCode.RightControl: return "<icon>G</>";
            case KeyCode.E: return "<icon>H</>";
            case KeyCode.Q: return "<icon>I</>";
            case KeyCode.Alpha7: return "<icon>Q</>";
            case KeyCode.Alpha8: return "<icon>Q</>";
            case KeyCode.LeftShift: return "<icon>L</>";
            case KeyCode.RightShift: return "<icon>L</>";
            case KeyCode.Delete: return "<icon>M</>";
            case KeyCode.K: return "<icon>O</>";
            case KeyCode.L: return "<icon>P</>";
            case KeyCode.F: return "<icon>N</>";
            case KeyCode.R: return "<icon>T</>";
            default: return keyCode.ToString();
        }
    }


    public static string ButtonToIcon(this ControllerButton button)
    {
        switch (button)
        {
            case ControllerButton.FaceA: return "<icon>e</>";
            case ControllerButton.FaceB: return "<icon>f</>";
            case ControllerButton.FaceX: return "<icon>h</>";
            case ControllerButton.FaceY: return "<icon>i</>";
            case ControllerButton.LT: return "<icon>m</>";
            case ControllerButton.RT: return "<icon>k</>";
            case ControllerButton.LB: return "<icon>R</>";
            case ControllerButton.RB: return "<icon>S</>";
            case ControllerButton.LS: return "<icon>J</>";
            case ControllerButton.Back: return "<icon>l</>";
            case ControllerButton.Start: return "<icon>g</>";

            case ControllerButton.DLeft: return "DPad <icon>c</>";
            case ControllerButton.DRight: return "DPad <icon>d</>";
            case ControllerButton.DUp: return "DPad <icon>a</>";
            case ControllerButton.DDown: return "DPad <icon>b</>";

            case ControllerButton.LLeft: return "LS <icon>c</>";
            case ControllerButton.LRight: return "LS <icon>d</>";
            case ControllerButton.LUp: return "LS <icon>a</>";
            case ControllerButton.LDown: return "LS <icon>b</>";

            case ControllerButton.RLeft: return "RS <icon>c</>";
            case ControllerButton.RRight: return "RS <icon>d</>";
            case ControllerButton.RUp: return "RS <icon>a</>";
            case ControllerButton.RDown: return "RS <icon>b</>";
            default: return button.ToString();
        }
    }
}

