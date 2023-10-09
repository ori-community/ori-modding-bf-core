using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace kft.oribf.uilib;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.kft.oribf.core")]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    private void Awake()
    {
        Logger = base.Logger;

        // Plugin startup logic
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

        new Harmony(PluginInfo.PLUGIN_GUID).PatchAll();
        TitleScreenModMenu.Init();
    }
}
