using SmartInput;

namespace OriModding.BF.InputLib;

/// <summary>
/// An input that is considered pressed when all buttons in the chord are pressed. e.g. Alt + R
/// </summary>
public class ChordedButtonInput : IButtonInput
{
    public IButtonInput[] Buttons;

    public bool GetButton()
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            if (!Buttons[i].GetButton())
                return false;
        }
        return true;
    }
}
