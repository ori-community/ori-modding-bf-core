﻿using BepInEx.Configuration;
using OriModding.BF.Core;
using System;
using UnityEngine;

namespace OriModding.BF.UiLib.Menu;

public class CustomOptionsScreen : MonoBehaviour
{
    public CleverMenuItemLayout layout;
    public CleverMenuItemSelectionManager selectionManager;
    public CleverMenuItemTooltipController tooltipController;
    public Transform pivot;
    public CleverMenuItemGroup group;

    public virtual void Awake()
    {
        // Layout and selection manager
        layout = GetComponent<CleverMenuItemLayout>();
        selectionManager = GetComponent<CleverMenuItemSelectionManager>();
        group = GetComponent<CleverMenuItemGroup>();
        layout.MenuItems.Clear();
        selectionManager.MenuItems.Clear();
        group.Options.Clear();
        pivot = transform.FindChild("highlightFade/pivot");

        foreach (object obj in pivot)
        {
            Destroy(((Transform)obj).gameObject);
        }

        var transparencyAnimator = transform.Find("highlightFade").GetComponent<TransparencyAnimator>();
        transparencyAnimator.Reset();

        // Tooltip
        Transform originalToolip = SettingsScreen.Instance.transform.Find("highlightFade/pivot/tooltip");
        Transform tooltip = Instantiate(originalToolip);
        tooltip.SetParent(pivot);
        tooltip.position = originalToolip.position;
        tooltipController = tooltip.GetComponent<CleverMenuItemTooltipController>();
        tooltipController.Selection = selectionManager;
        tooltipController.UpdateTooltip();
        tooltipController.enabled = true;

        InitScreen();
        gameObject.AddComponent<MenuScroller>();
        selectionManager.SetCurrentItem(0);
    }

    public virtual void InitScreen() { }

    private void AddToLayout(CleverMenuItem item)
    {
        layout.AddItem(item);
        layout.Sort();
        item.SetOpacity(1f);
        item.OnUnhighlight();
    }

    public CleverMenuItem AddItem(string label)
    {
        GameObject gameObject = Instantiate(SettingsScreen.Instance.transform.Find("highlightFade/pivot/damageText").gameObject);
        gameObject.transform.SetParent(pivot);
        foreach (var c in gameObject.GetComponentsInChildren<MonoBehaviour>())
            c.enabled = true;
        CleverMenuItem component = gameObject.GetComponent<CleverMenuItem>();
        component.Pressed = null;
        selectionManager.MenuItems.Add(component);
        AddToLayout(component);
        TransparencyAnimator[] transparencyAnimators = component.transform.GetComponentsInChildren<TransparencyAnimator>();
        for (int i = 0; i < transparencyAnimators.Length; i++)
        {
            transparencyAnimators[i].Reset();
            transparencyAnimators[i].enabled = true;
        }
        foreach (object obj in component.transform.FindChild("glowGroup"))
        {
            TransparencyAnimator.Register((Transform)obj);
        }
        gameObject.transform.Find("text/nameText").GetComponent<MessageBox>().SetMessage(new MessageDescriptor(label));
        return component;
    }

    public CleverMenuItem AddButton(string label, string tooltip, Action onPressed)
    {
        var item = AddItem("");
        item.SetValueText(label);
        ConfigureTooltip(item.GetComponent<CleverMenuItemTooltip>(), tooltip);

        //foreach (Transform child in item.transform)
        //    child.position -= new Vector3(2, 0, 0);

        item.PressedCallback += onPressed;

        return item;
    }

    public void AddToggle(ConfigEntry<bool> setting, string label, string tooltip)
    {
        CleverMenuItem cleverMenuItem = AddItem(label);
        cleverMenuItem.name = label;
        ToggleCustomSettingsAction toggleCustomSettingsAction = cleverMenuItem.gameObject.AddComponent<ToggleCustomSettingsAction>();
        toggleCustomSettingsAction.Setting = setting;
        toggleCustomSettingsAction.Init();
        cleverMenuItem.PressedCallback += () =>
        {
            toggleCustomSettingsAction.Toggle();
        };

        ConfigureTooltip(cleverMenuItem.GetComponent<CleverMenuItemTooltip>(), tooltip);
    }

    public void AddSlider(ConfigEntry<float> setting, string label, float min, float max, float step, string tooltip)
    {
        // Template is music volume slider
        GameObject clone = Instantiate(SettingsScreen.Instance.transform.Find("highlightFade/pivot/musicVolume").gameObject);
        clone.gameObject.name = label;
        foreach (var c in clone.GetComponentsInChildren<MonoBehaviour>())
            c.enabled = true;

        // Add to navigation manager (required for all option types)
        clone.transform.SetParent(pivot);
        CleverMenuItem cleverMenuItem = clone.GetComponent<CleverMenuItem>();
        selectionManager.MenuItems.Add(cleverMenuItem);
        AddToLayout(cleverMenuItem);

        // Add to group (required for sliders and dropdown items, but not toggles)
        CleverValueSlider slider = clone.transform.FindChild("slider").GetComponent<CleverValueSlider>();
        slider.NavigateMessageBoxes = new MessageBox[]
        {
            transform.FindChild("highlightFade/legend/pcLegend/navigate").GetComponent<MessageBox>(),
            transform.FindChild("highlightFade/legend/xBoxLegend/navigate").GetComponent<MessageBox>()
        };
        group.AddItem(cleverMenuItem, slider);

        slider.MinValue = min;
        slider.MaxValue = max;
        slider.Step = step;
        CustomSlider customSlider = slider.gameObject.AddComponent<CustomSlider>();
        customSlider.Setting = setting;

        MessageBox nameTextBox = clone.transform.Find("nameText").GetComponent<MessageBox>();
        nameTextBox.MessageProvider = null;
        nameTextBox.SetMessage(new MessageDescriptor(label));

        ConfigureTooltip(clone.GetComponent<CleverMenuItemTooltip>(), tooltip);

        foreach (var renderer in clone.GetComponentsInChildren<Renderer>())
            TransparencyAnimator.Register(renderer.transform);
    }

    //static Transform resolutionTemplate = null;
    //public void AddDropdown(ConfigEntry<int> intSetting, string label, string tooltip, OptionsListItem[] options)
    //{
    //    if (!resolutionTemplate)
    //    {
    //        resolutionTemplate = SceneManager.GetSceneByName("loadBootstrap").GetRootGameObjects().First((GameObject go) => go.name == "optionsScreen").transform.Find("*settings/highlightFade/pivot/resolution");
    //    }

    //    GameObject clone = Instantiate(resolutionTemplate.gameObject);
    //    clone.name = label;

    //    clone.transform.SetParent(pivot);
    //    CleverMenuItem cleverMenuItem = clone.GetComponent<CleverMenuItem>();
    //    selectionManager.MenuItems.Add(cleverMenuItem);
    //    AddToLayout(cleverMenuItem);

    //    Destroy(cleverMenuItem.Visible);
    //    cleverMenuItem.Visible = null;


    //    MessageBox nameTextBox = clone.transform.Find("text/nameText").GetComponent<MessageBox>();
    //    nameTextBox.MessageProvider = null;
    //    nameTextBox.SetMessage(new MessageDescriptor(label));

    //    ConfigureTooltip(cleverMenuItem.GetComponent<CleverMenuItemTooltip>(), tooltip);


    //    MessageBox valueTextBox = clone.transform.Find("text/stateText").GetComponent<MessageBox>();
    //    valueTextBox.MessageProvider = null;
    //    valueTextBox.SetMessage(new MessageDescriptor(options[intSetting.Value].label));


    //    // Replace the resolution options component with our own generic one
    //    var resolutionOptions = clone.GetComponentInChildren<ResolutionOptions>();
    //    resolutionOptions.ClearItems();

    //    resolutionOptions.GetComponent<TransparencyAnimator>().Reset(); // Back game bug also causes this list to grow every time the popup is dismissed, so let's clear it up for ours.
    //    TransparencyAnimator.Register(resolutionOptions.transform.Find("abilityMessageBackground"));


    //    // The items are added to a component so they are delayed in getting added.
    //    // If they are added immediately the resolution options stay on top. (?!?!)
    //    var dropdownList = resolutionOptions.gameObject.AddComponent<CustomDropdown>();
    //    dropdownList.gameObject.name = label + " options";
    //    dropdownList.Item = resolutionOptions.Item;
    //    dropdownList.Origin = dropdownList.transform;
    //    dropdownList.Spacing = 0.4f;
    //    dropdownList.Scrollable = true;
    //    dropdownList.ScrollPivot = dropdownList.transform.Find("pivot");
    //    dropdownList.OnScreenLimit = 16;
    //    dropdownList.ScrollingSpeed = 8;
    //    dropdownList.items = options;
    //    dropdownList.dismissOnChoose = true;
    //    dropdownList.intSetting = intSetting;

    //    DestroyImmediate(resolutionOptions);


    //    var dropdownGroup = dropdownList.GetComponent<CleverMenuItemGroup>();
    //    dropdownGroup.OnBackPressed = this.group.OnOptionBackPressed;
    //    dropdownGroup.IsVisible = false;
    //    dropdownGroup.IsActive = false;
    //    dropdownGroup.IsHighlightVisible = false;

    //    // Required to allow the popup to be dismissed
    //    this.group.AddItem(cleverMenuItem, dropdownGroup);
    //}

    private void ConfigureTooltip(CleverMenuItemTooltip tooltipComponent, string tooltip)
    {
        var tooltipMessageProvider = ScriptableObject.CreateInstance<BasicMessageProvider>();
        tooltipMessageProvider.SetMessage(tooltip);
        tooltipComponent.Tooltip = tooltipMessageProvider;
    }
}
