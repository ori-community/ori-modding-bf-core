using Game;
using HarmonyLib;
using OriModding.BF.Core;
using UnityEngine;

namespace OriModding.BF.UiLib;

public static class HintsPlus
{
    public static Vector3 nextHintLocation = OnScreenPositions.TopCenter;

    public static MessageBox Show(MessageProvider message, HintLayer layer = HintLayer.Gameplay, float duration = 3)
        => Show(message, OnScreenPositions.TopCenter, layer, duration);

    public static MessageBox Show(MessageProvider message, Vector3 position, HintLayer layer = HintLayer.Gameplay, float duration = 3)
    {
        nextHintLocation = position;
        var mbox = UI.Hints.Show(message, layer, duration);
        nextHintLocation = OnScreenPositions.TopCenter;
        return mbox;
    }
}


[HarmonyPatch(typeof(UI.Hints), nameof(UI.Hints.HintPosition), MethodType.Getter)]
internal static class HintPositionPatch
{
    internal static bool Prefix(ref Vector3 __result)
    {
        __result = HintsPlus.nextHintLocation;
        return HarmonyHelper.StopExecution;
    }
}
