using System;
using Avalonia;
using Avalonia.Controls.Primitives;

namespace AvaloniaUI.Ribbon;

public class RibbonMenuSeparatorItem : TemplatedControl, IRibbonMenuItem
{
    public static readonly StyledProperty<RibbonMenuItemPlacement> PlacementProperty =
        AvaloniaProperty.Register<RibbonMenuSeparatorItem, RibbonMenuItemPlacement>(nameof(Placement));

    protected override Type StyleKeyOverride { get; } = typeof(RibbonMenuSeparatorItem);

    public RibbonMenuItemPlacement Placement
    {
        get => GetValue(PlacementProperty);
        set => SetValue(PlacementProperty, value);
    }
}