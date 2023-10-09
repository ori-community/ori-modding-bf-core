using Game;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace kft.oribf.core;

public class SceneBootstrap : MonoBehaviour
{
    public static void RegisterHandler(Action<SceneBootstrap> callback, string group = "Bootstrap")
    {
        Controllers.Add<SceneBootstrap>(group: group, callback: mb => callback(mb as SceneBootstrap));
    }

    private void Awake()
    {
        Events.Scheduler.OnSceneRootPreEnabled.Add(OnSceneRootPreEnabled);
        Hooks.Hooks.Scene.OnSceneRootUnloaded += OnSceneRootUnloaded;
    }

    private void OnSceneRootPreEnabled(SceneRoot sceneRoot)
    {
        if (!loadedScenes.Contains(sceneRoot.name) && BootstrapActions.ContainsKey(sceneRoot.name))
        {
            Plugin.Logger.LogDebug("Bootstrapping scene " + sceneRoot.name);
            BootstrapActions[sceneRoot.name].Invoke(sceneRoot);
            loadedScenes.Add(sceneRoot.name);
        }
    }

    private void OnSceneRootUnloaded(string name)
    {
        loadedScenes.Remove(name);
    }

    public Dictionary<string, Action<SceneRoot>> BootstrapActions = new Dictionary<string, Action<SceneRoot>>();
    private readonly HashSet<string> loadedScenes = new HashSet<string>();
}
