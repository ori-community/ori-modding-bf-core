using kft.oribf.uilib;

namespace kft.oribf.configmenu;

internal class MyOptionsScreen : CustomOptionsScreen
{
    public override void InitScreen()
    {
        Plugin.Logger.LogWarning("InitScreen called");

        AddButton("Hi a button", "press this to do something", () =>
        {
            Plugin.Logger.LogInfo("You clicked the button!");
        });
    }
}