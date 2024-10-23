using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace Ribbon.Avalonia;

public class RibbonMenuButtonItem : Button, IRibbonMenuItem
{
    public static readonly StyledProperty<IControlTemplate> IconProperty =
        AvaloniaProperty.Register<RibbonMenuButtonItem, IControlTemplate>(nameof(Icon));

    public static readonly StyledProperty<RibbonMenuItemPlacement> PlacementProperty =
        AvaloniaProperty.Register<RibbonMenuButtonItem, RibbonMenuItemPlacement>(nameof(Placement));


    protected override Type StyleKeyOverride { get; } = typeof(RibbonMenuButtonItem);

    public IControlTemplate Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public RibbonMenuItemPlacement Placement
    {
        get => GetValue(PlacementProperty);
        set => SetValue(PlacementProperty, value);
    }
}