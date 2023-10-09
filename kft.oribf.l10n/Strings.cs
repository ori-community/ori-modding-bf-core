namespace kft.oribf.l10n;

public static class Strings
{
    internal static Plugin plugin;

    public static string Get(string key)
    {
        if (plugin.strings.TryGetValue(key, out var value))
            return value;
        return $"WARNING: String not found for key \"{key}\"";
    }

    public static string Get(string key, params object[] args)
    {
        return string.Format(Get(key), args);
    }
}
