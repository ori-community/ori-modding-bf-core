using BepInEx.Configuration;
using HarmonyLib;
using System;
using UnityEngine;

namespace kft.oribf.uilib;

public class CustomSlider : MonoBehaviour
{
    public ConfigEntry<float> Setting;

    public Action<float> OnSliderChanged;
}

[HarmonyPatch(typeof(MusicVolumeSlider), nameof(MusicVolumeSlider.Value), MethodType.Getter)]
internal class MusicVolumeSlider_GetValue
{
    private static bool Prefix(MusicVolumeSlider __instance, out float __result)
    {
        var customSlider = __instance.GetComponent<CustomSlider>();
        if (customSlider != null)
        {
            __result = customSlider.Setting.Value;
            return false;
        }

        __result = 0;
        return true; // This will allow the original to return music volume (not really hardcoded to 0)
    }
}

[HarmonyPatch(typeof(MusicVolumeSlider), nameof(MusicVolumeSlider.Value), MethodType.Setter)]
internal class MusicVolumeSlider_SetValue
{
    private static bool Prefix(MusicVolumeSlider __instance, float value)
    {
        var customSlider = __instance.GetComponent<CustomSlider>();
        if (customSlider != null)
        {
            customSlider.Setting.Value = value;
            customSlider.OnSliderChanged?.Invoke(value);
            return false;
        }

        return true;
    }
}