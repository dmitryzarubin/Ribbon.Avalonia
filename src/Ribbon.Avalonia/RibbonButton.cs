using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Ribbon.Avalonia.Enums;

namespace Ribbon.Avalonia;

public class RibbonButton : Button, IRibbonControl, ICanAddToQuickAccess
{
    public static readonly AvaloniaProperty<RibbonControlSize> SizeProperty;
    public static readonly AvaloniaProperty<RibbonControlSize> MinSizeProperty;
    public static readonly AvaloniaProperty<RibbonControlSize> MaxSizeProperty;
    public static readonly StyledProperty<IControlTemplate> IconProperty = AvaloniaProperty.Register<RibbonButton, IControlTemplate>(nameof(Icon));
    public static readonly StyledProperty<IControlTemplate> IconDisabledProperty = AvaloniaProperty.Register<RibbonButton, IControlTemplate>(nameof(IconDisabled));
    public static readonly StyledProperty<IControlTemplate> LargeIconProperty = AvaloniaProperty.Register<RibbonButton, IControlTemplate>(nameof(LargeIcon));
    public static readonly StyledProperty<IControlTemplate> LargeIconDisabledProperty = AvaloniaProperty.Register<RibbonButton, IControlTemplate>(nameof(LargeIconDisabled));
    
    
    public static readonly StyledProperty<IControlTemplate> QuickAccessIconProperty = AvaloniaProperty.Register<RibbonButton, IControlTemplate>(nameof(QuickAccessIcon));
    public static readonly StyledProperty<bool> CanAddToQuickAccessProperty = AvaloniaProperty.Register<RibbonButton, bool>(nameof(CanAddToQuickAccess), true);
    public static readonly StyledProperty<IControlTemplate> QuickAccessTemplateProperty = AvaloniaProperty.Register<RibbonButton, IControlTemplate>(nameof(Template));

    static RibbonButton()
    {
        RibbonControlHelper<RibbonButton>.SetProperties(out SizeProperty, out MinSizeProperty, out MaxSizeProperty);
        FocusableProperty.OverrideDefaultValue<RibbonButton>(false);
    }

    protected override Type StyleKeyOverride { get; } = typeof(RibbonButton);

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

    public IControlTemplate LargeIcon
    {
        get => GetValue(LargeIconProperty);
        set => SetValue(LargeIconProperty, value);
    }
    
    public IControlTemplate LargeIconDisabled
    {
        get => GetValue(LargeIconDisabledProperty);
        set => SetValue(LargeIconDisabledProperty, value);
    }

    public IControlTemplate QuickAccessIcon
    {
        get => GetValue(QuickAccessIconProperty);
        set => SetValue(QuickAccessIconProperty, value);
    }

    public bool CanAddToQuickAccess
    {
        get => GetValue(CanAddToQuickAccessProperty);
        set => SetValue(CanAddToQuickAccessProperty, value);
    }

    public IControlTemplate QuickAccessTemplate
    {
        get => GetValue(QuickAccessTemplateProperty);
        set => SetValue(QuickAccessTemplateProperty, value);
    }
        
    public RibbonControlSize Size
    {
        get => (RibbonControlSize)GetValue(SizeProperty)!;
        set => SetValue(SizeProperty, value);
    }

    public RibbonControlSize MinSize
    {
        get => (RibbonControlSize)GetValue(MinSizeProperty);
        set => SetValue(MinSizeProperty, value);
    }

    public RibbonControlSize MaxSize
    {
        get => (RibbonControlSize)GetValue(MaxSizeProperty);
        set => SetValue(MaxSizeProperty, value);
    }
}