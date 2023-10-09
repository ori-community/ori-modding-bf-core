using HarmonyLib;
using System;

namespace kft.oribf.core.Hooks;

public class GameHooks
{
    public Action OnStartNewGame;
}


[HarmonyPatch(typeof(GameController), nameof(GameController.SetupGameplay))]
internal class Hook_OnStartNewGame
{
    private static void Postfix()
    {
        Hooks.Game.OnStartNewGame?.Invoke();
    }
}
