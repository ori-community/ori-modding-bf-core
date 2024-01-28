using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OriModding.BF.UiLib.Menu;

public class CustomMenuManager
{
    private static readonly List<CustomOptionsScreenDef> optionsScreens = new List<CustomOptionsScreenDef>();

    /// <summary>
    /// Register a screen to appear in the Options sub-menu
    /// </summary>
    public static void RegisterOptionsScreen<TController>(string name, int index, Action<CustomOptionsScreen> onCreated = null) where TController : CustomOptionsScreen
    {
        optionsScreens.Add(new CustomOptionsScreenDef
        {
            ControllerType = typeof(TController),
            Index = index,
            Name = name,
            OnCreated = onCreated
        });
    }

    internal static IEnumerable<CustomOptionsScreenDef> GetOptionsScreens()
    {
        return optionsScreens.OrderBy(o => o.Index).ThenBy(o => o.Name);
    }
}

[HarmonyPatch(typeof(OptionsScreen), nameof(OptionsScreen.Awake))]
internal class CustomMenuCreator
{
    private static void Postfix(OptionsScreen __instance)
    {
        var screens = CustomMenuManager.GetOptionsScreens().ToArray();
        for (int i = 0; i < screens.Length; i++)
        {
            var newScreen = AddSubscreen(__instance, screens[i].ControllerType, screens[i].Name.ToUpper(), i + 2);
            screens[i].OnCreated?.Invoke(newScreen);
        }
    }

    private static CustomOptionsScreen AddSubscreen(OptionsScreen optionsScreen, Type controllerType, string label, int index)
    {
        optionsScreen.Navigation.AddMenuItem(label, index, optionsScreen.Navigation.transform.FindChild("mainMenuUI").GetComponent<CleverMenuItemLayout>(), null);
        GameObject gameObject = UnityEngine.Object.Instantiate(optionsScreen.transform.FindChild("*settings").gameObject);
        gameObject.name = "*" + label.ToLower();
        gameObject.transform.SetParent(optionsScreen.transform);
        UnityEngine.Object.Destroy(gameObject.GetComponent<SettingsScreen>());
        var screen = gameObject.AddComponent(controllerType);
        gameObject.SetActive(false);
        optionsScreen.GetComponent<CleverMenuItemGroup>().AddItem(optionsScreen.Navigation.MenuItems[index], gameObject.GetComponent<CleverMenuItemGroupBase>());
        return screen as CustomOptionsScreen;
    }
}
