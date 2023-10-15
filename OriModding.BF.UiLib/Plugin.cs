using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace OriModding.BF.UiLib;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency(Core.PluginInfo.PLUGIN_GUID)]
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
