using kft.oribf.uilib;

namespace kft.oribf.configmenu;

internal class MyOptionsScreen : CustomOptionsScreen
{
    public override void InitScreen()
    {
        AddButton("Hi a button", "press this to do something", () =>
        {
            Plugin.Logger.LogInfo("You clicked the button!");
        });

        AddToggle(Plugin.config, "Toggle this", "This is the tooltip for the toggle thingo");
        AddSlider(Plugin.configFloat, "Change the float", 0f, 1f, 0.1f, "Slide it");
    }
}