using SmartInput;

namespace OriModding.BF.InputLib;

public enum ControllerButton
{
    FaceA,
    FaceB,
    FaceX,
    FaceY,
    LT,
    RT,
    LB,
    RB,
    LS,
    RS,
    LUp,
    LDown,
    LLeft,
    LRight,
    DUp,
    DDown,
    DLeft,
    DRight,
    RUp,
    RDown,
    RLeft,
    RRight,
    Back,
    Start
}

internal static class ControllerButtonExtensions
{
    public static ControllerButton ToControllerButton(this ControllerButtonInput input)
    {
        switch (input.Button)
        {
            case XboxControllerInput.Button.ButtonA:
                return ControllerButton.FaceA;
            case XboxControllerInput.Button.ButtonX:
                return ControllerButton.FaceX;
            case XboxControllerInput.Button.ButtonY:
                return ControllerButton.FaceY;
            case XboxControllerInput.Button.ButtonB:
                return ControllerButton.FaceB;
            case XboxControllerInput.Button.LeftTrigger:
                return ControllerButton.LT;
            case XboxControllerInput.Button.RightTrigger:
                return ControllerButton.RT;
            case XboxControllerInput.Button.LeftShoulder:
                return ControllerButton.LB;
            case XboxControllerInput.Button.RightShoulder:
                return ControllerButton.RB;
            case XboxControllerInput.Button.LeftStick:
                return ControllerButton.LS;
            case XboxControllerInput.Button.RightStick:
                return ControllerButton.RS;
            case XboxControllerInput.Button.Select:
                return ControllerButton.Back;
            case XboxControllerInput.Button.Start:
                return ControllerButton.Start;
        }
        return ControllerButton.Start;
    }

    public static ControllerButton ToControllerButton(this AxisButtonInput axisInput)
    {
        switch ((axisInput.GetAxisInput() as ControllerAxisInput).Axis)
        {
            case XboxControllerInput.Axis.LeftStickX:
                return axisInput.GetAxisMode() == AxisButtonInput.AxisMode.GreaterThan ? ControllerButton.LRight : ControllerButton.LLeft;
            case XboxControllerInput.Axis.LeftStickY:
                return axisInput.GetAxisMode() == AxisButtonInput.AxisMode.GreaterThan ? ControllerButton.LUp : ControllerButton.LDown;

            case XboxControllerInput.Axis.RightStickX:
                return axisInput.GetAxisMode() == AxisButtonInput.AxisMode.GreaterThan ? ControllerButton.RRight : ControllerButton.RLeft;
            case XboxControllerInput.Axis.RightStickY:
                return axisInput.GetAxisMode() == AxisButtonInput.AxisMode.GreaterThan ? ControllerButton.RUp : ControllerButton.RDown;

            case XboxControllerInput.Axis.DpadX:
                return axisInput.GetAxisMode() == AxisButtonInput.AxisMode.GreaterThan ? ControllerButton.DRight : ControllerButton.DLeft;
            case XboxControllerInput.Axis.DpadY:
                return axisInput.GetAxisMode() == AxisButtonInput.AxisMode.GreaterThan ? ControllerButton.DUp : ControllerButton.DDown;
        }

        return ControllerButton.Start;
    }

    public static IButtonInput ToButtonInput(this ControllerButton button)
    {
        switch (button)
        {
            case ControllerButton.FaceA:
                return new ControllerButtonInput(XboxControllerInput.Button.ButtonA);
            case ControllerButton.FaceB:
                return new ControllerButtonInput(XboxControllerInput.Button.ButtonB);
            case ControllerButton.FaceX:
                return new ControllerButtonInput(XboxControllerInput.Button.ButtonX);
            case ControllerButton.FaceY:
                return new ControllerButtonInput(XboxControllerInput.Button.ButtonY);
            case ControllerButton.LT:
                return new ControllerButtonInput(XboxControllerInput.Button.LeftTrigger);
            case ControllerButton.RT:
                return new ControllerButtonInput(XboxControllerInput.Button.RightTrigger);
            case ControllerButton.LB:
                return new ControllerButtonInput(XboxControllerInput.Button.LeftShoulder);
            case ControllerButton.RB:
                return new ControllerButtonInput(XboxControllerInput.Button.RightShoulder);
            case ControllerButton.LS:
                return new ControllerButtonInput(XboxControllerInput.Button.LeftStick);
            case ControllerButton.RS:
                return new ControllerButtonInput(XboxControllerInput.Button.RightStick);
            case ControllerButton.LUp:
                return new AxisButtonInput(new ControllerAxisInput(XboxControllerInput.Axis.LeftStickY), AxisButtonInput.AxisMode.GreaterThan, 0.5f);
            case ControllerButton.LDown:
                return new AxisButtonInput(new ControllerAxisInput(XboxControllerInput.Axis.LeftStickY), AxisButtonInput.AxisMode.LessThan, -0.5f);
            case ControllerButton.LLeft:
                return new AxisButtonInput(new ControllerAxisInput(XboxControllerInput.Axis.LeftStickX), AxisButtonInput.AxisMode.LessThan, -0.5f);
            case ControllerButton.LRight:
                return new AxisButtonInput(new ControllerAxisInput(XboxControllerInput.Axis.LeftStickX), AxisButtonInput.AxisMode.GreaterThan, 0.5f);
            case ControllerButton.DUp:
                return new AxisButtonInput(new ControllerAxisInput(XboxControllerInput.Axis.DpadY), AxisButtonInput.AxisMode.GreaterThan, 0.5f);
            case ControllerButton.DDown:
                return new AxisButtonInput(new ControllerAxisInput(XboxControllerInput.Axis.DpadY), AxisButtonInput.AxisMode.LessThan, -0.5f);
            case ControllerButton.DLeft:
                return new AxisButtonInput(new ControllerAxisInput(XboxControllerInput.Axis.DpadX), AxisButtonInput.AxisMode.LessThan, -0.5f);
            case ControllerButton.DRight:
                return new AxisButtonInput(new ControllerAxisInput(XboxControllerInput.Axis.DpadX), AxisButtonInput.AxisMode.GreaterThan, 0.5f);
            case ControllerButton.RUp:
                return new AxisButtonInput(new ControllerAxisInput(XboxControllerInput.Axis.RightStickY), AxisButtonInput.AxisMode.GreaterThan, 0.5f);
            case ControllerButton.RDown:
                return new AxisButtonInput(new ControllerAxisInput(XboxControllerInput.Axis.RightStickY), AxisButtonInput.AxisMode.LessThan, -0.5f);
            case ControllerButton.RLeft:
                return new AxisButtonInput(new ControllerAxisInput(XboxControllerInput.Axis.RightStickX), AxisButtonInput.AxisMode.LessThan, -0.5f);
            case ControllerButton.RRight:
                return new AxisButtonInput(new ControllerAxisInput(XboxControllerInput.Axis.RightStickX), AxisButtonInput.AxisMode.GreaterThan, 0.5f);
            case ControllerButton.Back:
                return new ControllerButtonInput(XboxControllerInput.Button.Select);
            case ControllerButton.Start:
                return new ControllerButtonInput(XboxControllerInput.Button.Start);
            default:
                return null;
        }
    }
}
