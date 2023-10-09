using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;

namespace kft.oribf.qol;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency(kft.oribf.core.PluginInfo.PLUGIN_GUID)]
[BepInDependency(kft.oribf.configmenu.PluginInfo.PLUGIN_GUID, BepInDependency.DependencyFlags.SoftDependency)]
public class Plugin : BaseUnityPlugin
{
    public ConfigEntry<bool> CursorLock { get; set; }
    public ConfigEntry<bool> RunInBackground { get; set; }
    public ConfigEntry<float> BashDeadzone { get; set; }
    public ConfigEntry<float> AbilityMenuOpacity { get; set; }
    public ConfigEntry<float> ScreenShakeStrength { get; set; }
    public ConfigEntry<bool> SkipText { get; set; }
    public ConfigEntry<bool> CameraSway { get; set; }
    public ConfigEntry<float> HudScale { get; set; }

    private void Awake()
    {
        // Plugin startup logic
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

        CursorLock = Config.Bind("QOL", "Cursor Lock", false, "Whether the cursor should be locked to the screen");
        ScreenShakeStrength = Config.Bind("QOL", "Screen Shake Strength", 1f, "How strong should the screen shake effects be (min 0%, max 100%)");
        BashDeadzone = Config.Bind("QOL", "Bash Deadzone", 0.5f, "How large should the deadzone be while bashing (min 0%, max 100%)");
        RunInBackground = Config.Bind("QOL", "Run In Background", true, "Whether the game should continue to run when the window is not selected");
        AbilityMenuOpacity = Config.Bind("QOL", "Ability Menu Opacity", 1f, "How opaque should the ability menu be while moving in the background (min 0%, max 100%)");
        SkipText = Config.Bind("QOL", "Skip Text", false, "Whether the text boxes from Sein and pickups should be skipped");
        CameraSway = Config.Bind("QOL", "Camera Sway", true, "Whether the camera should subtly move when stationary");
        HudScale = Config.Bind("QOL", "HUD Scale", 1f, "How large the HUD should appear on screen (min 40%, max 160%)");

        if (Chainloader.PluginInfos.TryGetValue(kft.oribf.configmenu.PluginInfo.PLUGIN_GUID, out var configMenuInfo))
        {
            var configMenu = configMenuInfo.Instance as configmenu.Plugin;

            configMenu.ConfigureSlider(HudScale, 0.4f, 1.6f, 0.1f);
        }
    }
}
