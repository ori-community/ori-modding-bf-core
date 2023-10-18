using BepInEx.Configuration;
using OriModding.BF.UiLib.Menu;

namespace OriModding.BF.InputLib;

public static class CustomOptionsScreenExtensions
{
    public static void AddInputBind(this CustomOptionsScreen screen, ConfigEntry<CustomInput> input)
    {
        CleverMenuItem cleverMenuItem = screen.AddItem(input.Definition.Key);
        cleverMenuItem.gameObject.name = "Keybind (" + input.Definition.Key + ")";
        KeybindControl kc = cleverMenuItem.gameObject.AddComponent<KeybindControl>();
        kc.Init(input, screen);
        cleverMenuItem.PressedCallback += kc.BeginEditing;
    }
}
