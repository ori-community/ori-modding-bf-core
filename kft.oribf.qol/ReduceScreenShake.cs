using HarmonyLib;

namespace kft.oribf.qol;

[HarmonyPatch(typeof(CameraShake), nameof(CameraShake.ModifiedStrength), MethodType.Getter)]
public class ReduceScreenShake
{
    public static void Postfix(ref float __result)
    {
        __result *= Plugin.ScreenShakeStrength.Value;
    }
}
