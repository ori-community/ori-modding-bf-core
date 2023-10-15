using HarmonyLib;
using System;
using UnityEngine;

namespace KFT.OriBF.Qol;

public class HudScaler : MonoBehaviour
{
    Transform bottom, left, right;

    void Awake()
    {
        bottom = transform.Find("ui/status/dots");
        left = transform.Find("ui/left/mapstones");
        right = transform.Find("ui/right/keystones");

        Plugin.HudScale.SettingChanged += UpdateScale;

        UpdateScale(null, null);
    }

    void OnDestroy()
    {
        Plugin.HudScale.SettingChanged -= UpdateScale;
    }

    void UpdateScale(object sender, EventArgs e)
    {
        float scale = Plugin.HudScale.Value;
        float bottomScale = scale * 0.15f;
        bottom.transform.localScale = new Vector3(bottomScale, bottomScale, 1);
        left.transform.localScale = Vector3.one * scale;
        right.transform.localScale = Vector3.one * scale;
    }
}

[HarmonyPatch(typeof(SeinUI), nameof(SeinUI.Awake))]
static class HudScalePatch
{
    static void Postfix(SeinUI __instance)
    {
        __instance.gameObject.AddComponent<HudScaler>();
    }
}
