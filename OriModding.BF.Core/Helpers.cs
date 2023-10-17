using System.IO;
using BepInEx;

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
