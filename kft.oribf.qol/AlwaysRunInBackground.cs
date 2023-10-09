using HarmonyLib;

namespace kft.oribf.qol;

[HarmonyPatch(typeof(GameController), "OnApplicationFocus")]
internal class AlwaysRunInBackground
{
    private static bool Prefix()
    {
        if (Plugin.RunInBackground.Value)
        {
            GameController.IsFocused = true;
            return false;
        }

        return true;
    }
}
