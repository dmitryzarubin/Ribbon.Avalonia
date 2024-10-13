using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;

namespace AvaloniaUI.Ribbon;

public class RibbonDropDownButton : ItemsControl, IRibbonControl, ICanAddToQuickAccess
{
    public static readonly AvaloniaProperty<RibbonControlSize> SizeProperty;
    public static readonly AvaloniaProperty<RibbonControlSize> MinSizeProperty;
    public static readonly AvaloniaProperty<RibbonControlSize> MaxSizeProperty;
    public static readonly StyledProperty<IControlTemplate> IconProperty = AvaloniaProperty.Register<RibbonDropDownButton, IControlTemplate>(nameof(Icon));

    public static readonly StyledProperty<IControlTemplate>
        IconDisabledProperty = AvaloniaProperty.Register<RibbonDropDownButton, IControlTemplate>(nameof(IconDisabled));

    public static readonly StyledProperty<IControlTemplate> LargeIconProperty = RibbonButton.LargeIconProperty.AddOwner<RibbonDropDownButton>();

    public static readonly StyledProperty<IControlTemplate> LargeIconDisabledProperty =
        AvaloniaProperty.Register<RibbonDropDownButton, IControlTemplate>(nameof(LargeIconDisabled));

    public static readonly StyledProperty<object?> ContentProperty = ContentControl.ContentProperty.AddOwner<RibbonDropDownButton>();

    public static readonly StyledProperty<bool> CanAddToQuickAccessProperty = RibbonButton.CanAddToQuickAccessProperty.AddOwner<RibbonDropDownButton>();
    public static readonly StyledProperty<IControlTemplate> QuickAccessIconProperty = RibbonButton.QuickAccessIconProperty.AddOwner<RibbonDropDownButton>();
    public static readonly StyledProperty<IControlTemplate> QuickAccessTemplateProperty = RibbonButton.QuickAccessTemplateProperty.AddOwner<RibbonDropDownButton>();
    
    public static readonly DirectProperty<RibbonDropDownButton, bool> IsPressedProperty =
        AvaloniaProperty.RegisterDirect<RibbonDropDownButton, bool>(nameof(IsPressed), b => b.IsPressed);

    private bool _isPressed;

    static RibbonDropDownButton()
    {
        RibbonControlHelper<RibbonDropDownButton>.SetProperties(out SizeProperty, out MinSizeProperty, out MaxSizeProperty);
    }

    protected override Type StyleKeyOverride { get; } = typeof(RibbonDropDownButton);

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

    public object? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }
    
    public bool IsPressed
    {
        get => _isPressed;
        private set => SetAndRaise(IsPressedProperty, ref _isPressed, value);
    }

    internal Popup Popup { get; private set; }

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

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            return;

        IsPressed = true;
        e.Handled = true;

        Popup.Open();
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);

        if ((!IsPressed && !Popup.IsOpen) || e.InitialPressMouseButton != MouseButton.Left)
            return;

        IsPressed = false;
        e.Handled = true;
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        Popup = e.NameScope.Find<Popup>("PART_Popup");
        Popup!.Closed += (sender, args) => IsPressed = false;
    }
}