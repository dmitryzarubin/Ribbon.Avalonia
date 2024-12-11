using System;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.LogicalTree;

namespace Ribbon.Avalonia;

public class RibbonDropDownToggleItem : ToggleButton
{
    public static readonly StyledProperty<IControlTemplate> IconProperty = RibbonToggleButton.IconProperty.AddOwner<RibbonDropDownToggleItem>();
    public static readonly StyledProperty<IControlTemplate> IconToggledProperty = RibbonToggleButton.IconToggledProperty.AddOwner<RibbonDropDownToggleItem>();
    public static readonly StyledProperty<IControlTemplate> IconDisabledProperty = RibbonToggleButton.IconDisabledProperty.AddOwner<RibbonDropDownToggleItem>();
    public static readonly StyledProperty<bool> HasToggledIconProperty = AvaloniaProperty.Register<RibbonToggleButton, bool>(nameof(HasToggledIcon));
    
    protected override Type StyleKeyOverride { get; } = typeof(RibbonDropDownToggleItem);
    
    public IControlTemplate Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public IControlTemplate IconToggled
    {
        get => GetValue(IconToggledProperty);
        set
        {
            SetValue(IconToggledProperty, value);
            SetValue(HasToggledIconProperty, value != null);
        }
    }

    public IControlTemplate IconDisabled
    {
        get => GetValue(IconDisabledProperty);
        set => SetValue(IconDisabledProperty, value);
    }
    
    public bool HasToggledIcon => GetValue(HasToggledIconProperty);

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