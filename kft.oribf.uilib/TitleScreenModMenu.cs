using BepInEx.Bootstrap;
using kft.oribf.core;
using kft.oribf.uilib.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace kft.oribf.uilib;

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
