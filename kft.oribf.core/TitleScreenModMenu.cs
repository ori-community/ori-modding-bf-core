using BepInEx.Bootstrap;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace kft.oribf.core;

public class TitleScreenModMenu
{
    public static void Init()
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

                AddMenuItem(manager, "MODS", manager.MenuItems.Count - 1, () =>
                {
                    UI.Hints.Show(messageProvider, HintLayer.Gameplay, 3);
                });
            }
        };
    }
    private static void AddMenuItem(CleverMenuItemSelectionManager selectionManager, string label, int index, Action onPress)
    {
        CleverMenuItemLayout layout = selectionManager.gameObject.GetComponent<CleverMenuItemLayout>();
        CleverMenuItem cleverMenuItem = UnityEngine.Object.Instantiate(selectionManager.MenuItems[0]);
        cleverMenuItem.gameObject.name = label;
        cleverMenuItem.transform.SetParentMaintainingRotationAndScale(selectionManager.MenuItems[1].transform.parent);
        cleverMenuItem.Pressed = null;
        if (onPress != null)
            cleverMenuItem.PressedCallback += onPress;
        cleverMenuItem.gameObject.GetComponentInChildren<MessageBox>().SetMessage(new MessageDescriptor(label));
        cleverMenuItem.ApplyColors();
        selectionManager.MenuItems.Insert(index, cleverMenuItem);
        layout.MenuItems.Insert(index, cleverMenuItem);
        layout.Sort();
    }
}
