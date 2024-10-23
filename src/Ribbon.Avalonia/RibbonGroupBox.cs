using System;
using System.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Ribbon.Avalonia.Enums;

namespace Ribbon.Avalonia;

public class RibbonGroupBox : HeaderedItemsControl
{
    public static readonly DirectProperty<RibbonGroupBox, ICommand?> CommandProperty;
    public static readonly StyledProperty<object?> CommandParameterProperty = AvaloniaProperty.Register<RibbonGroupBox, object?>(nameof(CommandParameter));
    public static readonly StyledProperty<GroupDisplayMode> DisplayModeProperty = AvaloniaProperty.Register<RibbonGroupBox, GroupDisplayMode>(nameof(DisplayMode));
    public static readonly StyledProperty<Orientation> OrientationProperty = AvaloniaProperty.Register<Ribbon, Orientation>(nameof(Orientation));

    private ICommand? _command;

    static RibbonGroupBox()
    {
        AffectsMeasure<RibbonGroupBox>(DisplayModeProperty, OrientationProperty);

        CommandProperty = AvaloniaProperty.RegisterDirect<RibbonGroupBox, ICommand?>(nameof(Command), button => button.Command,
            (button, command) => button.Command = command, enableDataValidation: true);
    }

    protected override Type StyleKeyOverride { get; } = typeof(RibbonGroupBox);


    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    public GroupDisplayMode DisplayMode
    {
        get => GetValue(DisplayModeProperty);
        set
        {
            SetValue(DisplayModeProperty, value);

            switch (value)
            {
                case GroupDisplayMode.Large:
                    Items.Cast<IRibbonControl>().ForEach(c => c.Size = c.MaxSize);
                    break;
                case GroupDisplayMode.Small:
                    Items.Cast<IRibbonControl>().ForEach(c => c.Size = c.MinSize);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public ICommand? Command
    {
        get => _command;
        set => SetAndRaise(CommandProperty, ref _command, value);
    }
}