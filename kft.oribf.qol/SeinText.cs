using HarmonyLib;
using kft.oribf.core;
using UnityEngine;

namespace kft.oribf.qol;

[HarmonyPatch(typeof(MessageControllerB), nameof(MessageControllerB.ShowMessageBox))]
public class SeinSpeech
{
    public static bool Prefix(MessageControllerB __instance, GameObject messageBoxPrefab)
    {
        if (!Plugin.SkipText.Value)
            return HarmonyHelper.ContinueExecution;

        // Just get rid of sein text, not spirit tree/control hints etc.
        if (messageBoxPrefab == __instance.StoryMessage
            || messageBoxPrefab == __instance.PickupMessage
            || messageBoxPrefab == __instance.AbilityMessage)
            return HarmonyHelper.StopExecution;

        return HarmonyHelper.ContinueExecution;
    }
}
