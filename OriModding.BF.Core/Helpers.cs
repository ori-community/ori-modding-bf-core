using BepInEx;
using BepInEx.Bootstrap;
using System.IO;

namespace OriModding.BF.Core;

public static class HarmonyHelper
{
    /// <summary>Return this (or void) from a Prefix to allow the original method to run</summary>
    public const bool ContinueExecution = true;
    /// <summary>Return from a Prefix to stop the original method from running, but does not stop other prefixes</summary>
    public const bool StopExecution = false;
}

public static class FilesystemHelpers
{
    public static string GetPluginDirectory(this BaseUnityPlugin plugin)
    {
        return Path.GetDirectoryName(plugin.Info.Location);
    }

    public static string GetAssetPath(this BaseUnityPlugin plugin, params string[] relativePath)
    {
        return Path.Combine(plugin.GetPluginDirectory(), CombinePath(relativePath));
    }

    public static string CombinePath(params string[] paths)
    {
        string path = "";
        for (int i = 0; i < paths.Length; i++)
            path = Path.Combine(path, paths[i]);
        return path;
    }
}

public static class BepInExHelper
{
    /// <summary>
    /// Returns true if the plugin is loaded, false otherwise. Use [<see cref="BepInDependency"/>] to ensure the dependency is loaded first.
    /// </summary>
    public static bool IsPluginLoaded(this BaseUnityPlugin _, string guid)
        => Chainloader.PluginInfos.ContainsKey(guid);

    /// <summary>
    /// Gets a plugin if it is loaded. Cast to your type if this returns true to avoid dependency issues
    /// with missing optional plugins.
    /// </summary>
    public static bool TryGetPlugin(this BaseUnityPlugin _, string guid, out BaseUnityPlugin plugin)
    {
        var loaded = Chainloader.PluginInfos.TryGetValue(guid, out var pi);
        if (loaded)
        {
            plugin = pi.Instance;
            return true;
        }

        plugin = default;
        return false;
    }

    public static T GetPlugin<T>(this BaseUnityPlugin _, string guid) where T : BaseUnityPlugin
    {
        if (TryGetPlugin(_, guid, out var p))
        {
            return (T)p;
        }

        throw new System.Exception($"Plugin not found: {guid}");
    }
}
