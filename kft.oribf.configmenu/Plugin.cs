using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using kft.oribf.uilib;

namespace kft.oribf.configmenu;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency(kft.oribf.uilib.PluginInfo.PLUGIN_GUID)]
public class Plugin : BaseUnityPlugin
{
    internal static ConfigEntry<bool> config;
    internal static ConfigEntry<float> configFloat;

    internal static new ManualLogSource Logger;

    private void Awake()
    {
        Logger = base.Logger;

        config = Config.Bind("Test Section", "Test Key", true, "The description");
        configFloat = Config.Bind("Test Section", "Test Float", 0.5f, "The description for the float");

        Logger.LogInfo(config.Value);
        config.SettingChanged += Config_SettingChanged;

        CustomMenuManager.RegisterOptionsScreen<MyOptionsScreen>("Name", 0);
    }

    private void Config_SettingChanged(object sender, System.EventArgs e)
    {
        Logger.LogInfo($"Changed to {config.Value}");
    }
}
