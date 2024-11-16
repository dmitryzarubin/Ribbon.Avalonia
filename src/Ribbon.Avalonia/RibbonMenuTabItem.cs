using System;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;

namespace Ribbon.Avalonia;

public class RibbonMenuTabItem : ToggleButton, IRibbonMenuItem
{
    public static readonly StyledProperty<IControlTemplate> IconProperty =
        AvaloniaProperty.Register<RibbonMenuTabItem, IControlTemplate>(nameof(Icon));

    public static readonly StyledProperty<RibbonMenuItemPlacement> PlacementProperty =
        AvaloniaProperty.Register<RibbonMenuTabItem, RibbonMenuItemPlacement>(nameof(Placement));

    public static readonly StyledProperty<IControlTemplate> ControlPaneProperty =
        AvaloniaProperty.Register<RibbonMenuTabItem, IControlTemplate>(nameof(ControlPane));

    protected override Type StyleKeyOverride { get; } = typeof(RibbonMenuTabItem);

    public IControlTemplate Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public IControlTemplate ControlPane
    {
        get => GetValue(ControlPaneProperty);
        set => SetValue(ControlPaneProperty, value);
    }

    public RibbonMenuItemPlacement Placement
    {
        get => GetValue(PlacementProperty);
        set => SetValue(PlacementProperty, value);
    }
}