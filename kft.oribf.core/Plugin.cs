using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace kft.oribf.core;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    public static new ManualLogSource Logger;

    private void Awake()
    {
        Logger = base.Logger;

        new Harmony(PluginInfo.PLUGIN_GUID).PatchAll();
        Hooks.Hooks.SetupHooks();

        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }
}
