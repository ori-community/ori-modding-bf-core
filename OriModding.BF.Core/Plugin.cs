using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using OriModding.BF.l10n;
using OriModding.BF.UiLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace OriModding.BF.Core;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    public static new ManualLogSource Logger;

    private LocalisationManager localisationManager;

    private void Awake()
    {
        Logger = base.Logger;

        new Harmony(PluginInfo.PLUGIN_GUID).PatchAll();

        TitleScreenModMenu.Init();

        SceneBootstrap.RegisterHandler(bootstrap =>
        {
            bootstrap.BootstrapActions = new Dictionary<string, Action<SceneRoot>>
            {
                ["titleScreenSwallowsNest"] = root =>
                {
                    root.transform.Find("art/*unsorted/windTunnelsHousingFlagD").GetComponent<MeshRenderer>().material.color = new Color(0, 1, 0, 1);
                    root.transform.Find("art/*unsorted/windTunnelsHousingFlagC").GetComponent<MeshRenderer>().material.color = new Color(0, 1, 0, 1);
                }
            };
        });

        localisationManager = new LocalisationManager();

        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void OnDestroy()
    {
        localisationManager.Dispose();
    }
}
