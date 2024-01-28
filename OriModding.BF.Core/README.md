# Modding Library

## Player Usage

Install using the [Mod Manager](https://github.com/Kirefel/bf-mod-manager).

Or download and extract to `Ori DE\BepInEx\plugins\OriModding.BF.Core`.

## Developer instructions

Create a mod with the [BepInEx 5 Plugin Template](https://docs.bepinex.dev/articles/dev_guide/plugin_tutorial/1_setup.html#installing-bepinex-plugin-templates).

```
dotnet new bepinex5plugin -n MyPluginName -T net35 -U 5.3.2
```

Add the nuget package.

```
dotnet add package OriModding.BF.Core
```

Add `OriModding.BF.Core` to the dependencies.

```c#
[BepInPlugin(...)]
[BepInDependency(OriModding.BF.Core.PluginInfo.PLUGIN_GUID)]
public class Plugin : BaseUnityPlugin
{
    void Awake()
    {
        // Initialisation code here
    }
}
```

### Usage: MonoMod.RuntimeDetours

Use the `On.<Type>.<Method>` event to inject code.

```c#
On.GameController.Awake += (orig, self) =>
{
    // Code here
    orig(self);
    // More code here
}
```

See the [MonoMod docs](https://github.com/MonoMod/MonoMod/blob/master/README-RuntimeDetour.md#using-hookgen) for more details.

### Usage: Harmony

Use `Harmony` attributes to inject code.

```c#
[BepInPlugin(...)]
public class Plugin
{
    Harmony harmony;

    void Awake()
    {
        harmony = new Harmony("guid");
        harmony.PatchAll();
    }
}

[HarmonyPatch(typeof(GameController), nameof(GameController.Awake))]
static class GameControllerHarmonyPatch
{
    static void Prefix(GameController __instance)
    {
        // Prefix code here
    }

    static void Postfix(GameController __instance)
    {
        // Postfix code here
    }
}
```

See the [Harmony docs](https://harmony.pardeike.net/articles/patching.html) for more details.

## OriModding.BF.l10n

Localisation support for providing translations of mods.

To use localised strings, create `strings.csv` in the root of the mod directory. Example:

```csv
,English,Italian
SPIRIT_FLAME,Spirit Flame,Fiamma dello spirito
```

Then fetch it in code:

```c#
using OriModding.BF.l10n;
Strings.Get("SPIRIT_FLAME");
```

It will fall back to English for any given key if a translation isn't found.
