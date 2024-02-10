using BepInEx.Configuration;
using OriModding.BF.Core;
using OriModding.BF.UiLib.Menu;
using SmartInput;
using System;
using System.Linq;
using UnityEngine;

namespace OriModding.BF.InputLib;

public class KeybindControl : MonoBehaviour
{
    private CustomInput newInput;

    private bool editing;

    private MessageBox messageBox;

    private int exit;

    private CustomOptionsScreen owner;
    private ConfigEntry<CustomInput> inputConfig;
    private static KeyCode[] keyCodes;
    private static ControllerButton[] controllerButtons;
    private bool[] controllerButtonsPressed;
    private BasicMessageProvider tooltipProvider;

    public void Init(ConfigEntry<CustomInput> input, CustomOptionsScreen owner)
    {
        this.owner = owner;
        inputConfig = input;

        keyCodes ??= ((KeyCode[])Enum.GetValues(typeof(KeyCode))).Where(kc => kc < KeyCode.JoystickButton0).ToArray();
        controllerButtons ??= (ControllerButton[])Enum.GetValues(typeof(ControllerButton));
        controllerButtonsPressed = new bool[controllerButtons.Length];
    }

    private void Awake()
    {
        messageBox = base.transform.Find("text/stateText").GetComponent<MessageBox>();

        messageBox.SetMessage(new MessageDescriptor(inputConfig.Value.ToFriendlyString()));
        CleverMenuItemTooltip component = GetComponent<CleverMenuItemTooltip>();
        tooltipProvider = ScriptableObject.CreateInstance<BasicMessageProvider>();
        tooltipProvider.SetMessage("<icon>D</>: add or remove binds");
        component.Tooltip = tooltipProvider;
        owner.tooltipController.UpdateTooltip();
    }


    private int CurrentInputCount => newInput.input.Buttons?.Length ?? 0;

    private IButtonInput Top()
    {
        if (newInput.input.Buttons != null && newInput.input.Buttons.Length > 0)
            return newInput.input.Buttons[newInput.input.Buttons.Length - 1];
        return null;
    }

    private bool Equals(IButtonInput button, KeyCode kc)
    {
        return button is KeyCodeButtonInput k && k.KeyCode == kc;
    }

    private bool Equals(IButtonInput button, ControllerButton cb)
    {
        if (button is ControllerButtonInput cbi && cbi.ToControllerButton() == cb)
            return true;
        if (button is AxisButtonInput abi && abi.ToControllerButton() == cb)
            return true;
        return false;
    }

    public void Update()
    {
        if (!editing)
        {
            return;
        }
        if (exit < 2)
        {
            exit++;
            return;
        }
        if ((Input.GetKeyDown(KeyCode.Return) || WasPressed(ControllerButton.Start)) && CurrentInputCount > 0)
        {
            FinishEditing();
            return;
        }


        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (CurrentInputCount > 0)
            {
                newInput.input.Buttons[newInput.input.Buttons.Length - 1] = null;
                Array.Resize(ref newInput.input.Buttons, newInput.input.Buttons.Length - 1);
                UpdateMessageBox();
                return;
            }
        }
        else if (Input.anyKeyDown)
        {
            HandleKeyboard();
        }

        HandleController();

        foreach (ControllerButton button in controllerButtons)
        {
            controllerButtonsPressed[(int)button] = button.IsPressed();
        }
    }

    private void HandleKeyboard()
    {
        foreach (KeyCode keyCode in keyCodes)
        {
            if (Input.GetKeyDown(keyCode))
            {
                var top = Top();
                if (top != null && top.GetButton() && !Equals(top, keyCode))
                {
                    // Convert to chorded input
                    newInput.input.Buttons[newInput.input.Buttons.Length - 1] = new ChordedButtonInput
                    {
                        Buttons = new[] { top, new KeyCodeButtonInput(keyCode) }
                    };
                }
                else if (top is ChordedButtonInput cbi && cbi.Buttons.Length > 0 && cbi.Buttons[0].GetButton() && !Equals(cbi.Buttons[0], keyCode))
                {
                    Array.Resize(ref cbi.Buttons, cbi.Buttons.Length + 1);
                    cbi.Buttons[cbi.Buttons.Length - 1] = new KeyCodeButtonInput(keyCode);
                }
                else
                {
                    newInput.AddKeyCodes(keyCode);
                }
                UpdateMessageBox();
                return;
            }
        }
    }

    private void HandleController()
    {
        var pressedButton = GetPressedButtonAsBind();
        if (pressedButton != null)
        {
            var top = Top();
            if (top != null && top.GetButton() && !Equals(top, pressedButton.Value))
            {
                // Convert to chorded input
                newInput.input.Buttons[newInput.input.Buttons.Length - 1] = new ChordedButtonInput
                {
                    Buttons = new[] { top, pressedButton.Value.ToButtonInput() }
                };
            }
            else if (top is ChordedButtonInput cbi && cbi.Buttons.Length > 0 && cbi.Buttons[0].GetButton() && !Equals(cbi.Buttons[0], pressedButton.Value))
            {
                Array.Resize(ref cbi.Buttons, cbi.Buttons.Length + 1);
                cbi.Buttons[cbi.Buttons.Length - 1] = pressedButton.Value.ToButtonInput();
            }
            else
            {
                newInput.AddControllerButtons(pressedButton.Value);
            }
            UpdateMessageBox();
        }
    }

    private bool WasPressed(ControllerButton button)
    {
        return !controllerButtonsPressed[(int)button] && button.IsPressed();
    }

    public void BeginEditing()
    {
        newInput = new CustomInput();
        newInput.LoadFromString(inputConfig.Value.Serialise());

        SuspensionManager.SuspendAll();

        editing = true;
        exit = 0;
        tooltipProvider.SetMessage("Backspace: remove bind\n<icon>D</>: finish editing");
        owner.tooltipController.UpdateTooltip();

        for (int i = 0; i < controllerButtonsPressed.Length; i++)
            controllerButtonsPressed[i] = true;
    }

    private void FinishEditing()
    {
        editing = false;
        SuspensionManager.ResumeAll();
        PlayerInput.Instance.RefreshControlScheme();
        inputConfig.Value = newInput;
        tooltipProvider.SetMessage("[Accept]: add or remove binds");
        owner.tooltipController.UpdateTooltip();
    }

    private void UpdateMessageBox()
    {
        messageBox.SetMessage(new MessageDescriptor(newInput.ToFriendlyString()));
    }

    public void Reset()
    {
        messageBox.SetMessage(new MessageDescriptor(inputConfig.Value.ToFriendlyString()));
        editing = false;
    }

    private ControllerButton? GetPressedButtonAsBind()
    {
        foreach (ControllerButton button in controllerButtons)
        {
            if (WasPressed(button))
            {
                return button;
            }
        }

        return null;
    }
}
