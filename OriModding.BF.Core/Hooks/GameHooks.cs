using System;

namespace OriModding.BF.Core.Hooks;

public class GameHooks
{
    public Action OnStartNewGame;

    internal void SetupHooks()
    {
        On.GameController.SetupGameplay += (orig, self, sceneRoot, worldEventsOnAwake) =>
        {
            orig(self, sceneRoot, worldEventsOnAwake);
            OnStartNewGame?.Invoke();
        };
    }
}
