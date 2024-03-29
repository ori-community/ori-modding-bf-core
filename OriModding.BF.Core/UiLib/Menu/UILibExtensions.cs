﻿using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OriModding.BF.UiLib.Menu;

public static class UILibExtensions
{
    public static void AddMenuItem(this CleverMenuItemSelectionManager selectionManager, string label, Action onPress)
    {
        selectionManager.AddMenuItem(label, selectionManager.MenuItems.Count - 1, onPress);
    }

    public static void AddMenuItem(this CleverMenuItemSelectionManager selectionManager, string label, int index, Action onPress)
    {
        CleverMenuItemLayout component = selectionManager.gameObject.GetComponent<CleverMenuItemLayout>();
        if (component != null)
        {
            selectionManager.AddMenuItem(label, index, component, onPress);
            return;
        }
    }

    public static void AddMenuItem(this CleverMenuItemSelectionManager selectionManager, string label, int index, CleverMenuItemLayout layout, Action onPress)
    {
        CleverMenuItem cleverMenuItem = UnityEngine.Object.Instantiate(selectionManager.MenuItems[0]);
        cleverMenuItem.gameObject.name = label;
        cleverMenuItem.transform.SetParentMaintainingRotationAndScale(selectionManager.MenuItems[1].transform.parent);
        cleverMenuItem.Pressed = null;
        if (onPress != null)
            cleverMenuItem.PressedCallback += onPress;
        cleverMenuItem.gameObject.GetComponentInChildren<MessageBox>().SetMessage(new MessageDescriptor(label));
        cleverMenuItem.ApplyColors();
        selectionManager.MenuItems.Insert(index, cleverMenuItem);
        layout.AddItem(cleverMenuItem, index);
    }

    public static void AddItem(this CleverMenuItemLayout layout, CleverMenuItem item)
    {
        layout.MenuItems.Add(item);
        layout.Sort();
    }

    public static void AddItem(this CleverMenuItemLayout layout, CleverMenuItem item, int index)
    {
        layout.MenuItems.Insert(index, item);
        layout.Sort();
    }

    public static void AddItem(this CleverMenuItemGroup group, CleverMenuItem item, CleverMenuItemGroupBase itemGroup)
    {
        CleverMenuItemGroup.CleverMenuItemGroupItem cleverMenuItemGroupItem = new CleverMenuItemGroup.CleverMenuItemGroupItem
        {
            ItemGroup = itemGroup,
            MenuItem = item,
        };
        cleverMenuItemGroupItem.ItemGroup.IsActive = false;
        itemGroup.OnBackPressed = (Action)Delegate.Combine(itemGroup.OnBackPressed, new Action(group.OnOptionBackPressed));
        group.Options.Add(cleverMenuItemGroupItem);
    }

    public static void Reset(this TransparencyAnimator animator)
    {
        var childTransparencyAnimators = Traverse.Create(animator).Field("m_childTransparencyAnimators").GetValue<IList>();
        var cleverMenuItems = Traverse.Create(animator).Field("m_cleverMenuItems").GetValue<IList>();
        var rendererData = Traverse.Create(animator).Field("m_rendererData").GetValue<IList>();
        var renderers = Traverse.Create(animator).Field("m_renderers").GetValue<HashSet<Renderer>>();

        childTransparencyAnimators?.Clear();
        cleverMenuItems?.Clear();
        rendererData?.Clear();
        renderers?.Clear();
    }

    public static void SetValueText(this CleverMenuItem item, string text)
    {
        var textTransform = item.transform.Find("text/stateText");
        if (!textTransform)
            return;

        var messageBox = textTransform.GetComponent<MessageBox>();
        if (!messageBox)
            return;

        messageBox.MessageProvider = null;
        messageBox.SetMessage(new MessageDescriptor(text));
    }

    public static Transform EmbedChildren(this Transform transform)
    {
        var newTransform = new GameObject("container").transform;
        newTransform.parent = transform;
        newTransform.localPosition = Vector3.zero;
        newTransform.localScale = Vector3.one;
        for (int i = transform.childCount - 1; i >= 0; i--)
            transform.GetChild(i).SetParentMaintainingLocalTransform(newTransform);
        return newTransform;
    }
}
