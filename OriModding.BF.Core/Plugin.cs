using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using OriModding.BF.InputLib;
using OriModding.BF.l10n;
using OriModding.BF.UiLib;

namespace OriModding.BF.Core;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    public static new ManualLogSource Logger;

    private LocalisationManager localisationManager;
    public InputManager InputManager { get; private set; }

    private void Awake()
    {
        Logger = base.Logger;

        // Add converter so the config manager recognises the CustomInput format in the .cfg
        TomlTypeConverter.AddConverter(typeof(CustomInput), new TypeConverter
        {
            ConvertToString = (obj, type) => ((CustomInput)obj).Serialise(),
            ConvertToObject = (str, type) => CustomInput.FromString(str)
        });

        new Harmony(PluginInfo.PLUGIN_GUID).PatchAll();

        TitleScreenModMenu.Init();

        localisationManager = new LocalisationManager();
        InputManager = new InputManager();

        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void OnDestroy()
    {
        localisationManager.Dispose();
    }
}
