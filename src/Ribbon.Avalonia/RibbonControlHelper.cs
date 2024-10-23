using System;
using Avalonia;
using Avalonia.Layout;
using Ribbon.Avalonia.Enums;

namespace Ribbon.Avalonia;

public static class RibbonControlHelper<T> where T : Layoutable
{
    private static readonly AvaloniaProperty<RibbonControlSize> SizeProperty =
        AvaloniaProperty.Register<T, RibbonControlSize>("Size", RibbonControlSize.Large, coerce: CoerceSize);

    private static readonly AvaloniaProperty<RibbonControlSize> MinSizeProperty = AvaloniaProperty.Register<T, RibbonControlSize>("MinSize");
    private static readonly AvaloniaProperty<RibbonControlSize> MaxSizeProperty = AvaloniaProperty.Register<T, RibbonControlSize>("MaxSize", RibbonControlSize.Large);

    private static RibbonControlSize CoerceSize(AvaloniaObject obj, RibbonControlSize val)
    {
        if (obj is IRibbonControl ctrl)
        {
            if ((int)ctrl.MinSize > (int)val)
                return ctrl.MinSize;
            if ((int)ctrl.MaxSize < (int)val)
                return ctrl.MaxSize;
            return val;
        }

        throw new Exception("obj must be of IRibbonControl type!");
    }


    public static void SetProperties(out AvaloniaProperty<RibbonControlSize> size, out AvaloniaProperty<RibbonControlSize> minSize,
        out AvaloniaProperty<RibbonControlSize> maxSize)
    {
        size = SizeProperty;
        minSize = MinSizeProperty;
        maxSize = MaxSizeProperty;
            
        minSize.Changed.AddClassHandler<T>((sender, args) =>
        {
            if ((int)args.NewValue > (int)(sender as IRibbonControl).Size)
            {
                (sender as IRibbonControl).Size = (RibbonControlSize)args.NewValue;
                sender.InvalidateMeasure();
            }
        });

        maxSize.Changed.AddClassHandler<T>((sender, args) =>
        {
            if ((int)args.NewValue < (int)(sender as IRibbonControl).Size)
            {
                (sender as IRibbonControl).Size = (RibbonControlSize)args.NewValue;
                sender.InvalidateMeasure();
            }
        });
    }
}