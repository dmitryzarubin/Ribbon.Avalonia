using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Ribbon.Avalonia.Enums;

namespace Ribbon.Avalonia;

public class RibbonSplitButton : ItemsControl, IRibbonControl
{
    public static readonly StyledProperty<ICommand?> CommandProperty = Button.CommandProperty.AddOwner<RibbonSplitButton>();
    public static readonly StyledProperty<object?> CommandParameterProperty = Button.CommandParameterProperty.AddOwner<RibbonSplitButton>();

    public static readonly AvaloniaProperty<RibbonControlSize> SizeProperty;
    public static readonly AvaloniaProperty<RibbonControlSize> MinSizeProperty;
    public static readonly AvaloniaProperty<RibbonControlSize> MaxSizeProperty;
    public static readonly StyledProperty<IControlTemplate> IconProperty = AvaloniaProperty.Register<RibbonSplitButton, IControlTemplate>(nameof(Icon));

    public static readonly StyledProperty<IControlTemplate>
        IconDisabledProperty = AvaloniaProperty.Register<RibbonSplitButton, IControlTemplate>(nameof(IconDisabled));

    public static readonly StyledProperty<IControlTemplate> LargeIconProperty = RibbonButton.LargeIconProperty.AddOwner<RibbonSplitButton>();

    public static readonly StyledProperty<IControlTemplate> LargeIconDisabledProperty =
        AvaloniaProperty.Register<RibbonSplitButton, IControlTemplate>(nameof(LargeIconDisabled));

    public static readonly StyledProperty<object?> ContentProperty = ContentControl.ContentProperty.AddOwner<RibbonSplitButton>();

    public static readonly StyledProperty<bool> CanAddToQuickAccessProperty = RibbonButton.CanAddToQuickAccessProperty.AddOwner<RibbonSplitButton>();
    public static readonly StyledProperty<IControlTemplate> QuickAccessIconProperty = RibbonButton.QuickAccessIconProperty.AddOwner<RibbonSplitButton>();
    public static readonly StyledProperty<IControlTemplate> QuickAccessTemplateProperty = RibbonButton.QuickAccessTemplateProperty.AddOwner<RibbonSplitButton>();

    public static readonly DirectProperty<RibbonSplitButton, bool> IsButtonPressedProperty =
        AvaloniaProperty.RegisterDirect<RibbonSplitButton, bool>(nameof(IsButtonPressed), b => b.IsButtonPressed);

    public static readonly DirectProperty<RibbonSplitButton, bool> IsMenuPressedProperty =
        AvaloniaProperty.RegisterDirect<RibbonSplitButton, bool>(nameof(IsMenuPressed), b => b.IsMenuPressed);

    
    private bool _isButtonPressed;
    private bool _isMenuPressed;

    static RibbonSplitButton()
    {
        RibbonControlHelper<RibbonSplitButton>.SetProperties(out SizeProperty, out MinSizeProperty, out MaxSizeProperty);
    }

    protected override Type StyleKeyOverride { get; } = typeof(RibbonSplitButton);

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

    public bool IsButtonPressed
    {
        get => _isButtonPressed;
        private set => SetAndRaise(IsButtonPressedProperty, ref _isButtonPressed, value);
    }
    
    public bool IsMenuPressed
    {
        get => _isMenuPressed;
        private set => SetAndRaise(IsMenuPressedProperty, ref _isMenuPressed, value);
    }

    internal Popup Popup { get; private set; }
    
    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
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

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        Popup = e.NameScope.Find<Popup>("PART_Popup");
        Popup!.Closed += (sender, args) => IsButtonPressed = false;

        var leftBorder = e.NameScope.Find<Border>("LeftBorder");
        if (leftBorder != null)
        {
            leftBorder.PointerPressed += (sender, args) =>
            {
                if (!args.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
                    return;

                IsButtonPressed = true;
                e.Handled = true;
            };
            
            leftBorder.PointerReleased += (sender, args) =>
            {
                if (!IsEffectivelyEnabled || args.InitialPressMouseButton != MouseButton.Left)
                    return;

                if (Command != null && Command.CanExecute(CommandParameter))
                    Command.Execute(CommandParameter);
                
                IsButtonPressed = false;
                e.Handled = true;
            };
        }
        
        var rightBorder = e.NameScope.Find<Border>("RightBorder");
        if (rightBorder != null)
        {
            rightBorder.PointerPressed += (sender, args) =>
            {
                if (!args.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
                    return;

                IsMenuPressed = true;
                e.Handled = true;

                Popup.Open();
            };

            rightBorder.PointerReleased += (sender, args) =>
            {
                if (!IsEffectivelyEnabled || args.InitialPressMouseButton != MouseButton.Left)
                    return;

                IsMenuPressed = false;
                args.Handled = true;
            };
        }
    }
}