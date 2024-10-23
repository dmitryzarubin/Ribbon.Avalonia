using System;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Ribbon.Avalonia.Enums;

namespace Ribbon.Avalonia;

public class RibbonToggleButton : ToggleButton, IRibbonControl, ICanAddToQuickAccess
{
    public static readonly AvaloniaProperty<RibbonControlSize> SizeProperty;
    public static readonly AvaloniaProperty<RibbonControlSize> MinSizeProperty;
    public static readonly AvaloniaProperty<RibbonControlSize> MaxSizeProperty;
    public static readonly StyledProperty<IControlTemplate> IconProperty = AvaloniaProperty.Register<RibbonToggleButton, IControlTemplate>(nameof(Icon));
    public static readonly StyledProperty<IControlTemplate> IconToggledProperty = AvaloniaProperty.Register<RibbonToggleButton, IControlTemplate>(nameof(IconToggled));
    public static readonly StyledProperty<IControlTemplate> IconDisabledProperty = AvaloniaProperty.Register<RibbonToggleButton, IControlTemplate>(nameof(IconDisabled));
    public static readonly StyledProperty<IControlTemplate> LargeIconProperty = AvaloniaProperty.Register<RibbonToggleButton, IControlTemplate>(nameof(LargeIcon));
    public static readonly StyledProperty<IControlTemplate> LargeToggledIconProperty = AvaloniaProperty.Register<RibbonToggleButton, IControlTemplate>(nameof(LargeToggledIcon));
    public static readonly StyledProperty<bool> HasToggledIconProperty = AvaloniaProperty.Register<RibbonToggleButton, bool>(nameof(HasToggledIcon));
    public static readonly StyledProperty<IControlTemplate> LargeIconDisabledProperty = AvaloniaProperty.Register<RibbonToggleButton, IControlTemplate>(nameof(LargeIconDisabled));
    
    public static readonly StyledProperty<IControlTemplate> QuickAccessTemplateProperty = AvaloniaProperty.Register<RibbonToggleButton, IControlTemplate>(nameof(Template));
    public static readonly StyledProperty<IControlTemplate> QuickAccessIconProperty = RibbonButton.QuickAccessIconProperty.AddOwner<RibbonToggleButton>();
    public static readonly StyledProperty<bool> CanAddToQuickAccessProperty = RibbonButton.CanAddToQuickAccessProperty.AddOwner<RibbonToggleButton>();

    static RibbonToggleButton()
    {
        RibbonControlHelper<RibbonToggleButton>.SetProperties(out SizeProperty, out MinSizeProperty, out MaxSizeProperty);
        FocusableProperty.OverrideDefaultValue<RibbonToggleButton>(false);
    }

    protected override Type StyleKeyOverride { get; } = typeof(RibbonToggleButton);

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
    
    public IControlTemplate LargeIcon
    {
        get => GetValue(LargeIconProperty);
        set => SetValue(LargeIconProperty, value);
    }
    
    public IControlTemplate LargeToggledIcon
    {
        get => GetValue(LargeToggledIconProperty);
        set
        {
            SetValue(LargeToggledIconProperty, value);
            SetValue(HasToggledIconProperty, value != null);
        }
    }

    public IControlTemplate LargeIconDisabled
    {
        get => GetValue(LargeIconDisabledProperty);
        set => SetValue(LargeIconDisabledProperty, value);
    }
    
    public bool HasToggledIcon => GetValue(HasToggledIconProperty);

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
        get => (RibbonControlSize)GetValue(MinSizeProperty)!;
        set => SetValue(MinSizeProperty, value);
    }

    public RibbonControlSize MaxSize
    {
        get => (RibbonControlSize)GetValue(MaxSizeProperty)!;
        set => SetValue(MaxSizeProperty, value);
    }
}