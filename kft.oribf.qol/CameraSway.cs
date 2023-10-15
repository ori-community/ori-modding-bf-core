using HarmonyLib;
using UnityEngine;

namespace KFT.OriBF.Qol;

[HarmonyPatch(typeof(AreaMapNavigation), nameof(AreaMapNavigation.UpdatePlane))]
internal class AreaMapCameraSwayPatch
{
    private static void Postfix(AreaMapNavigation __instance)
    {
        if (Plugin.CameraSway.Value)
            return;

        // Eliminate the offset used to move the camera around
        __instance.MapPivot.position = -__instance.ScrollPosition * __instance.Zoom;
    }
}

[HarmonyPatch(typeof(WorldMapUI), nameof(WorldMapUI.UpdateCameraPosition))]
internal class WorldMapCameraSwayPatch
{
    private static void Postfix(WorldMapUI __instance)
    {
        if (Plugin.CameraSway.Value)
            return;

        Vector3 position;
        position.x = Mathf.Lerp(__instance.FarPosition.x, __instance.ClosePosition.x, __instance.ZoomXYCurve.Evaluate(__instance.ZoomTime));
        position.y = Mathf.Lerp(__instance.FarPosition.y, __instance.ClosePosition.y, __instance.ZoomXYCurve.Evaluate(__instance.ZoomTime));
        position.z = Mathf.Lerp(__instance.FarPosition.z, __instance.ClosePosition.z, __instance.ZoomZCurve.Evaluate(__instance.ZoomTime));
        __instance.Camera.transform.position = position;
    }
}

[HarmonyPatch(typeof(CameraOffsetController), nameof(CameraOffsetController.Awake))]
internal class GameplayCameraSwayPatch
{
    private static void Postfix(CameraOffsetController __instance)
    {
        __instance.transform.GetChild(0).gameObject.AddComponent<CameraSwayToggler>();
    }
}

public class CameraSwayToggler : MonoBehaviour
{
    private SinMovement sinMovement;

    private void Awake()
    {
        sinMovement = GetComponent<SinMovement>();
    }

    private void FixedUpdate()
    {
        sinMovement.enabled = Plugin.CameraSway.Value;
    }
}
