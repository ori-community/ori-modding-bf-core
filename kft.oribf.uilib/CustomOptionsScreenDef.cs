using System;

namespace kft.oribf.uilib;

internal class CustomOptionsScreenDef
{
    public string Name { get; set; }
    public int Index { get; set; }
    public Type ControllerType { get; set; }
    public Action<CustomOptionsScreen> OnCreated { get; set; }
}