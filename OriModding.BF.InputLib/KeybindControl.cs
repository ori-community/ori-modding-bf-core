using BepInEx.Configuration;
using OriModding.BF.Core;
using OriModding.BF.UiLib.Menu;
using SmartInput;
using System;
using UnityEngine;

namespace OriModding.BF.InputLib;

public class KeybindControl : MonoBehaviour
{
    private CustomInput newInput;

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

    public void BeginEditing()
    {
        newInput = new CustomInput();
        newInput.LoadFromString(inputConfig.Value.Serialise());

        SuspensionManager.SuspendAll();

        editing = true;
        exit = 0;
        tooltipProvider.SetMessage("Backspace: remove bind\n<icon>D</>: finish editing");
        owner.tooltipController.UpdateTooltip();
    }

    private int CurrentInputCount => newInput.input.Buttons?.Length ?? 0;
    private bool CurrentInputContains(KeyCode kc)
    {
        if (newInput.input.Buttons == null)
            return false;

        foreach (var button in newInput.input.Buttons)
        {
            if (button is KeyCodeButtonInput kcbi && kcbi.KeyCode == kc)
                return true;
        }
        return false;
    }
    private IButtonInput Top()
    {
        if (newInput.input.Buttons?.Length > 0)
            return newInput.input.Buttons[newInput.input.Buttons.Length - 1];
        return null;
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
        if (Input.GetKeyDown(KeyCode.Return) && CurrentInputCount > 0)
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
            foreach (KeyCode keyCode in keyCodes)
            {
                if (Input.GetKeyDown(keyCode))
                {
                    var top = Top();
                    if (top is KeyCodeButtonInput kcbi && Input.GetKey(kcbi.KeyCode))
                    {
                        // Convert to chorded input
                        newInput.input.Buttons[newInput.input.Buttons.Length - 1] = new ChordedButtonInput
                        {
                            Buttons = new[] { top, new KeyCodeButtonInput(keyCode) }
                        };
                        UpdateMessageBox();
                        return;
                    }
                    else if (top is ChordedButtonInput cbi && cbi.Buttons.Length > 0 && cbi.Buttons[0].GetButton())
                    {
                        Array.Resize(ref cbi.Buttons, cbi.Buttons.Length + 1);
                        cbi.Buttons[cbi.Buttons.Length - 1] = new KeyCodeButtonInput(keyCode);
                        UpdateMessageBox();
                        return;
                    }

                    if (!CurrentInputContains(keyCode))
                    {
                        newInput.AddKeyCodes(keyCode);
                        UpdateMessageBox();
                    }
                }
            }
        }
    }

    private void FinishEditing()
    {
        editing = false;
        SuspensionManager.ResumeAll();
        PlayerInput.Instance.RefreshControlScheme();
        inputConfig.Value = newInput;
        tooltipProvider.SetMessage("<icon>D</>: add or remove binds");
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

    public void Init(ConfigEntry<CustomInput> input, CustomOptionsScreen owner)
    {
        this.owner = owner;
        inputConfig = input;

        keyCodes = (KeyCode[])Enum.GetValues(typeof(KeyCode));
    }


    private bool editing;

    private MessageBox messageBox;

    private int exit;

    private CustomOptionsScreen owner;
    private ConfigEntry<CustomInput> inputConfig;
    private KeyCode[] keyCodes;
    private BasicMessageProvider tooltipProvider;
}
