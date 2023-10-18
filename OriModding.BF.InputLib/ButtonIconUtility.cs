using UnityEngine;

namespace OriModding.BF.InputLib;

public static class ButtonIconUtility
{
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

    //public static string ButtonToIcon(this ControllerRebinds.ControllerButton button)
    //{
    //    switch (button)
    //    {
    //        case ControllerRebinds.ControllerButton.A: return "<icon>e</>";
    //        case ControllerRebinds.ControllerButton.B: return "<icon>f</>";
    //        case ControllerRebinds.ControllerButton.X: return "<icon>h</>";
    //        case ControllerRebinds.ControllerButton.Y: return "<icon>i</>";
    //        case ControllerRebinds.ControllerButton.LT: return "<icon>m</>";
    //        case ControllerRebinds.ControllerButton.RT: return "<icon>k</>";
    //        case ControllerRebinds.ControllerButton.LB: return "<icon>R</>";
    //        case ControllerRebinds.ControllerButton.RB: return "<icon>S</>";
    //        case ControllerRebinds.ControllerButton.LS: return "<icon>J</>";
    //        case ControllerRebinds.ControllerButton.Back: return "<icon>l</>";
    //        case ControllerRebinds.ControllerButton.Start: return "<icon>g</>";

    //        case ControllerRebinds.ControllerButton.DLeft: return "DPad <icon>c</>";
    //        case ControllerRebinds.ControllerButton.DRight: return "DPad <icon>d</>";
    //        case ControllerRebinds.ControllerButton.DUp: return "DPad <icon>a</>";
    //        case ControllerRebinds.ControllerButton.DDown: return "DPad <icon>b</>";

    //        case ControllerRebinds.ControllerButton.LLeft: return "LS <icon>c</>";
    //        case ControllerRebinds.ControllerButton.LRight: return "LS <icon>d</>";
    //        case ControllerRebinds.ControllerButton.LUp: return "LS <icon>a</>";
    //        case ControllerRebinds.ControllerButton.LDown: return "LS <icon>b</>";

    //        case ControllerRebinds.ControllerButton.RLeft: return "RS <icon>c</>";
    //        case ControllerRebinds.ControllerButton.RRight: return "RS <icon>d</>";
    //        case ControllerRebinds.ControllerButton.RUp: return "RS <icon>a</>";
    //        case ControllerRebinds.ControllerButton.RDown: return "RS <icon>b</>";
    //        default: return button.ToString();
    //    }
    //}
}