using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.LogicalTree;

namespace Ribbon.Avalonia;

public class RibbonDropDownButtonItem : Button
{
    public static readonly StyledProperty<IControlTemplate> IconProperty = RibbonButton.IconProperty.AddOwner<RibbonDropDownButtonItem>();
    public static readonly StyledProperty<IControlTemplate> IconDisabledProperty = RibbonButton.IconDisabledProperty.AddOwner<RibbonDropDownButtonItem>();

    protected override Type StyleKeyOverride { get; } = typeof(RibbonDropDownButtonItem);

    public IControlTemplate Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public IControlTemplate IconDisabled
    {
        get => GetValue(IconDisabledProperty);
        set => SetValue(IconDisabledProperty, value);
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);

        if (e.InitialPressMouseButton != MouseButton.Left)
            return;

        Popup? popup = null;
        if (this.GetLogicalParent<RibbonDropDownButton>() != null)
            popup = this.GetLogicalParent<RibbonDropDownButton>()!.Popup;
        else if (this.GetLogicalParent<RibbonSplitButton>() != null)
            popup = this.GetLogicalParent<RibbonSplitButton>()!.Popup;

        popup?.Close();
        e.Handled = true;
    }
}