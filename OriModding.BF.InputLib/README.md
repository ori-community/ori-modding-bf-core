# Input Library

Use this for custom controls.

```c#
// Plugin.Awake()
var inputLib = this.GetPlugin("OriModding.BF.InputLib");
var input = inputLib.BindAndRegister(this, "MyPlugin", "MyCustomInput",
    new CustomInput()
        .AddKeyCodes(KeyCode.T)
        .AddControllerButtons(ControllerButton.LB)
    );

// Update()
if (input.Value.OnPressed)
{
    // Button was pressed this frame
}
```
