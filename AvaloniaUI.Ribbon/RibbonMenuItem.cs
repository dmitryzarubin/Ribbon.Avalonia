using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;

namespace AvaloniaUI.Ribbon;

public class RibbonMenuItem : HeaderedItemsControl
{
    public static readonly StyledProperty<object> IconProperty = AvaloniaProperty.Register<RibbonMenuItem, object>(nameof(Icon));
    public static readonly StyledProperty<bool> IsSubmenuOpenProperty = AvaloniaProperty.Register<RibbonMenuItem, bool>(nameof(IsSubmenuOpen));
    public static readonly DirectProperty<RibbonMenuItem, bool> HasItemsProperty = AvaloniaProperty.RegisterDirect<RibbonMenuItem, bool>(nameof(HasItems),
        m => m.Items.Count > 0);
    public static readonly StyledProperty<bool> IsSelectedProperty = AvaloniaProperty.Register<RibbonMenuItem, bool>(nameof(IsSelected));
    public static readonly DirectProperty<RibbonMenuItem, ICommand> CommandProperty =
        AvaloniaProperty.RegisterDirect<RibbonMenuItem, ICommand>("Command", button => button.Command, (button, command) => button.Command = command);
    public static readonly StyledProperty<object> CommandParameterProperty = Button.CommandParameterProperty.AddOwner<RibbonMenuItem>();
    
    public static readonly RoutedEvent<RoutedEventArgs> ClickEvent = RoutedEvent.Register<Button, RoutedEventArgs>(nameof(Click), RoutingStrategies.Bubble);
    
    private ICommand _command;

    public object Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public bool IsSubmenuOpen
    {
        get => GetValue(IsSubmenuOpenProperty);
        set => SetValue(IsSubmenuOpenProperty, value);
    }

    public bool HasItems => GetValue(HasItemsProperty);

    public bool IsSelected
    {
        get => GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    public ICommand Command
    {
        get => _command;
        set => SetAndRaise(CommandProperty, ref _command, value);
    }

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public event EventHandler<RoutedEventArgs> Click
    {
        add => AddHandler(ClickEvent, value);
        remove => RemoveHandler(ClickEvent, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        e.NameScope.Get<Button>("PART_ContentButton").Click += (_, _) =>
        {
            RaiseEvent(new RoutedEventArgs(ClickEvent));
        };
    }
}