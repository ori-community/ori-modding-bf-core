# Config Menu

Adds a config menu in-game for every mod that has config options.

## For developers

To customise the behaviour of the menu:

```c#
HudScale = Config.Bind("QOL", "HUD Scale", 1f, "How large the HUD should appear on screen (min 40%, max 160%)");

if (Chainloader.PluginInfos.TryGetValue(OriModding.BF.ConfigMenu.PluginInfo.PLUGIN_GUID, out var pi) && pi.Instance is OriModding.BF.ConfigMenu.Plugin configMenu)
{
    configMenu.ConfigureSlider(HudScale, 0.4f, 1.6f, 0.1f);
}
```
