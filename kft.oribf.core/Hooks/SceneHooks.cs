using HarmonyLib;
using System;

namespace kft.oribf.core.Hooks;

public class SceneHooks
{
    public Action<string> OnSceneRootUnloaded;
}

[HarmonyPatch(typeof(SceneRoot), nameof(SceneRoot.Unload))]
internal class Hook_OnSceneRootUnload
{
    private static void Postfix(SceneRoot __instance)
    {
        Hooks.Scene.OnSceneRootUnloaded?.Invoke(__instance.name);
    }
}