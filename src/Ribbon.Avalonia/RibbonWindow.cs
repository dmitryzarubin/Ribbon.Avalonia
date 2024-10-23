using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;

namespace Ribbon.Avalonia;

public class RibbonWindow : Window
{
    public static readonly StyledProperty<IBrush> TitleBarBackgroundProperty = AvaloniaProperty.Register<RibbonWindow, IBrush>(nameof(TitleBarBackground));
    public static readonly StyledProperty<IBrush> TitleBarForegroundProperty = AvaloniaProperty.Register<RibbonWindow, IBrush>(nameof(TitleBarForeground));

    public static readonly StyledProperty<Orientation> OrientationProperty = StackPanel.OrientationProperty.AddOwner<RibbonWindow>();

    public static readonly StyledProperty<bool> LeftSideCaptionButtonsProperty =
        AvaloniaProperty.Register<RibbonWindow, bool>(nameof(LeftSideCaptionButtons), UseLeftSideCaptionButtons());

    public static readonly StyledProperty<Ribbon> RibbonProperty = AvaloniaProperty.Register<RibbonWindow, Ribbon>(nameof(Ribbon));

    public static readonly StyledProperty<QuickAccessToolbar> QuickAccessToolbarProperty =
        AvaloniaProperty.Register<RibbonWindow, QuickAccessToolbar>(nameof(QuickAccessToolbar));
    
    static RibbonWindow()
    {
        OrientationProperty.OverrideDefaultValue<RibbonWindow>(Orientation.Horizontal);

        RibbonProperty.Changed.AddClassHandler<RibbonWindow>((sender, e) => sender.RefreshRibbon(e.OldValue, e.NewValue));
        QuickAccessToolbarProperty.Changed.AddClassHandler<RibbonWindow>((sender, e) => sender.RefreshQat(e.OldValue, e.NewValue));
    }

    public RibbonWindow()
    {
        RefreshRibbon(null, Ribbon);
        RefreshQat(null, QuickAccessToolbar);
    }

    public IBrush TitleBarBackground
    {
        get => GetValue(TitleBarBackgroundProperty);
        set => SetValue(TitleBarBackgroundProperty, value);
    }

    public IBrush TitleBarForeground
    {
        get => GetValue(TitleBarForegroundProperty);
        set => SetValue(TitleBarForegroundProperty, value);
    }

    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    public bool LeftSideCaptionButtons
    {
        get => GetValue(LeftSideCaptionButtonsProperty);
        set => SetValue(LeftSideCaptionButtonsProperty, value);
    }

    public Ribbon Ribbon
    {
        get => GetValue(RibbonProperty);
        set => SetValue(RibbonProperty, value);
    }

    public QuickAccessToolbar QuickAccessToolbar
    {
        get => GetValue(QuickAccessToolbarProperty);
        set => SetValue(QuickAccessToolbarProperty, value);
    }


    protected override Type StyleKeyOverride { get; } = typeof(RibbonWindow);

    private static bool UseLeftSideCaptionButtons()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return true;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            //TODO: See if there's any sane way of getting  the user's Window manager/decorator/etc and its configuration, and deciding or guessing based on that
            return false;
        //on Windows
        return false;
    }

    private void RefreshRibbon(object oldValue, object newValue)
    {
        if (oldValue is Ribbon oldRibbon)
        {
            //oldRibbon.QuickAccessToolbar = null;
            oldRibbon.ClearValue(Ribbon.OrientationProperty);
        }


        if (newValue is Ribbon newRibbon)
        {
            //newRibbon.QuickAccessToolbar = QuickAccessToolbar;
            newRibbon[!Ribbon.OrientationProperty] = this[!OrientationProperty];

            if (QuickAccessToolbar != null)
                QuickAccessToolbar.Ribbon = newRibbon;
        }
        else if (QuickAccessToolbar != null)
        {
            QuickAccessToolbar.Ribbon = null;
        }
    }

    private void RefreshQat(object oldValue, object newValue)
    {
        // if (oldValue is QuickAccessToolbar oldQat)
        //     oldQat.Ribbon = null;
        //
        //
        // if (newValue is QuickAccessToolbar newQat)
        // {
        //     newQat.Ribbon = Ribbon;
        //
        //     if (Ribbon != null)
        //         Ribbon.QuickAccessToolbar = newQat;
        // }
        // else if (Ribbon != null)
        // {
        //     Ribbon.QuickAccessToolbar = null;
        // }
    }


    private void SetupSide(string name, StandardCursorType cursor, WindowEdge edge, ref TemplateAppliedEventArgs e)
    {
        //var control = e.NameScope.Get<Control>(name);
        //control.Cursor = new Cursor(cursor);
        // control.PointerPressed += (object sender, PointerPressedEventArgs ep) =>
        // {
        //     ((Window)this.GetVisualRoot()).PlatformImpl?.BeginResizeDrag(edge, ep);
        // };
    }

    private T GetControl<T>(TemplateAppliedEventArgs e, string name) where T : class
    {
        return e.NameScope.Get<T>(name);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        var window = this;
        try
        {
            var titleBar = GetControl<Control>(e, "PART_TitleBar");

            // titleBar.PointerPressed += (object sender, PointerPressedEventArgs ep) =>
            // {
            //     if (_titlebarSecondClick)
            //         window.WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            //     else
            //         window.PlatformImpl?.BeginMoveDrag(ep);
            //     
            //     DragDrop.
            //     
            //
            //     if (!_titlebarSecondClick)
            //     {
            //         _titlebarSecondClick = true;
            //
            //         Timer secondClickTimer = new Timer(250);
            //         secondClickTimer.Elapsed += (sneder, e) =>
            //         {
            //             _titlebarSecondClick = false;
            //             secondClickTimer.Stop();
            //         };
            //         secondClickTimer.Start();
            //     }
            // };

            try
            {
                SetupSide("Left_top", StandardCursorType.LeftSide, WindowEdge.West, ref e);
                SetupSide("Left_mid", StandardCursorType.LeftSide, WindowEdge.West, ref e);
                SetupSide("Left_bottom", StandardCursorType.LeftSide, WindowEdge.West, ref e);
                SetupSide("Right_top", StandardCursorType.RightSide, WindowEdge.East, ref e);
                SetupSide("Right_mid", StandardCursorType.RightSide, WindowEdge.East, ref e);
                SetupSide("Right_bottom", StandardCursorType.RightSide, WindowEdge.East, ref e);
                SetupSide("Top", StandardCursorType.TopSide, WindowEdge.North, ref e);
                SetupSide("Bottom", StandardCursorType.BottomSide, WindowEdge.South, ref e);
                SetupSide("TopLeft", StandardCursorType.TopLeftCorner, WindowEdge.NorthWest, ref e);
                SetupSide("TopRight", StandardCursorType.TopRightCorner, WindowEdge.NorthEast, ref e);
                SetupSide("BottomLeft", StandardCursorType.BottomLeftCorner, WindowEdge.SouthWest, ref e);
                SetupSide("BottomRight", StandardCursorType.BottomRightCorner, WindowEdge.SouthEast, ref e);
            }
            catch
            {
            }

            GetControl<Button>(e, "PART_MinimizeButton").Click += delegate { window.WindowState = WindowState.Minimized; };
            GetControl<Button>(e, "PART_MaximizeButton").Click += delegate
            {
                window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            };
            GetControl<Button>(e, "PART_CloseButton").Click += delegate { window.Close(); };
        }
        catch (KeyNotFoundException)
        {
        }
    }
}