using System;
using System.Linq;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Layout;

namespace AvaloniaUI.Ribbon;

public class ToolBar : ItemsControl
{
    public static readonly StyledProperty<Orientation> OrientationProperty = AvaloniaProperty.Register<Ribbon, Orientation>(nameof(Orientation));

    static ToolBar()
    {
        AffectsMeasure<ToolBar>(OrientationProperty);
        OrientationProperty.Changed.AddClassHandler<ToolBar, Orientation>((toolBar, args) => { toolBar.UpdateGroupsOrientation(args.NewValue.Value); });
    }

    public ToolBar()
    {
        Items.GetWeakCollectionChangedObservable().Subscribe(args => UpdateGroupsOrientation(Orientation));
        UpdateGroupsOrientation(Orientation);
    }

    protected override Type StyleKeyOverride { get; } = typeof(ToolBar);
    
    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    private void UpdateGroupsOrientation(Orientation orientation)
    {
        Items.OfType<RibbonGroupBox>().ForEach(x => x.Orientation = orientation);
    }
}