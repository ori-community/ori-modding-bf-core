using BepInEx.Bootstrap;
using OriModding.BF.Core;
using OriModding.BF.UiLib.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OriModding.BF.UiLib;

internal class TitleScreenModMenu
{
    internal static void Init()
    {
        SceneBootstrap.RegisterHandler(Bootstrap);
    }

    private static void Bootstrap(SceneBootstrap bootstrap)
    {
        bootstrap.BootstrapActions = new Dictionary<string, Action<SceneRoot>>
        {
            ["titleScreenSwallowsNest"] = sceneRoot =>
            {
                var manager = sceneRoot.transform.Find("ui/group/3. fullGameMainMenu").GetComponent<CleverMenuItemSelectionManager>();

                var messageProvider = ScriptableObject.CreateInstance<BasicMessageProvider>();

                messageProvider.SetMessage(string.Join("\n", Chainloader.PluginInfos.Select(x => x.Value.Metadata.Name).ToArray()));

                manager.AddMenuItem("MODS", manager.MenuItems.Count - 1, () =>
                {
                    HintsPlus.Show(messageProvider, OnScreenPositions.MiddleLeft);
                });
            }
        };
    }
}
