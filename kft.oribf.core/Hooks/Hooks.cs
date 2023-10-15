using System;

namespace kft.oribf.core.Hooks;

public class Hooks
{
    public static SeinHooks Sein { get; } = new();
    public static GameHooks Game { get; } = new();
    public static SceneHooks Scene { get; } = new();

    public static Action OnGameControllerInitialised { get; set; }

    internal static void SetupHooks()
    {
        Game.SetupHooks();
    }
}
