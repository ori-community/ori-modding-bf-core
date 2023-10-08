using BepInEx;
using BepInEx.Configuration;

namespace kft.oribf.configmenu;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("kft.oribf.uilib")]
public class Plugin : BaseUnityPlugin
{
    private ConfigEntry<bool> config;

    private void Awake()
    {
        // Plugin startup logic
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

        config = Config.Bind("Test Section", "Test Key", true, "The description");

        Logger.LogInfo(config.Value);
        config.SettingChanged += Config_SettingChanged;
    }

    private void Config_SettingChanged(object sender, System.EventArgs e)
    {
        Logger.LogInfo($"Changed to {config.Value}");
    }
}
