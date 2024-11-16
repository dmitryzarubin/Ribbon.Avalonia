﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;

namespace Ribbon.Avalonia;

public class Ribbon : TabControl, IKeyTipHandler
{
    public static readonly StyledProperty<Orientation> OrientationProperty =
        AvaloniaProperty.Register<Ribbon, Orientation>(nameof(Orientation));
    
    public static readonly DirectProperty<Ribbon, ObservableCollection<RibbonGroupBox>> SelectedGroupsProperty =
        AvaloniaProperty.RegisterDirect<Ribbon, ObservableCollection<RibbonGroupBox>>(nameof(SelectedGroups), o => o.SelectedGroups, (o, v) => o.SelectedGroups = v);

    public static readonly DirectProperty<Ribbon, ObservableCollection<Control>> TabsProperty =
        AvaloniaProperty.RegisterDirect<Ribbon, ObservableCollection<Control>>(nameof(Tabs), o => o.Tabs, (o, v) => o.Tabs = v);

    
    public static readonly StyledProperty<bool> IsCollapsedProperty = AvaloniaProperty.Register<Ribbon, bool>(nameof(IsCollapsed));
    
    public static readonly StyledProperty<RibbonMenu> MenuProperty = AvaloniaProperty.Register<Ribbon, RibbonMenu>(nameof(Menu));
    public static readonly StyledProperty<bool> IsMenuOpenProperty = AvaloniaProperty.Register<Ribbon, bool>(nameof(IsMenuOpen));


    public static readonly DirectProperty<Ribbon, ICommand> HelpButtonCommandProperty =
        AvaloniaProperty.RegisterDirect<Ribbon, ICommand>(nameof(HelpButtonCommand), o => o.HelpButtonCommand, (o, v) => o.HelpButtonCommand = v);
    
    public static readonly DirectProperty<Ribbon, bool> IsOpenProperty = MenuBase.IsOpenProperty.AddOwner<Ribbon>(o => o.IsOpen); //, (o, v) => o.IsOpen = v

    public static readonly RoutedEvent<RoutedEventArgs> MenuClosedEvent = RoutedEvent.Register<Ribbon, RoutedEventArgs>(nameof(MenuClosed), RoutingStrategies.Bubble);

    public static readonly RoutedEvent<RoutedEventArgs> RibbonKeyTipsOpenedEvent =
        RoutedEvent.Register<MenuBase, RoutedEventArgs>("RibbonKeyTipsOpened", RoutingStrategies.Bubble);

    //private ContextMenu _ctxMenu;
    private ItemsControl _groupsHost;
    private ICommand _helpCommand;

    private bool _isOpen;
    private ItemsPresenter _itemHeadersPresenter;
    private IInputElement _prevFocusedElement;
    private RibbonTab _prevSelectedTab;
    private ICanAddToQuickAccess _rightClicked;
    private ObservableCollection<RibbonGroupBox> _selectedGroups = new();
    private ObservableCollection<Control> _tabs = new();

    static Ribbon()
    {
        AffectsMeasure<ToolBar>(OrientationProperty);
        
        TabsProperty.Changed.AddClassHandler<Ribbon>((sender, e) => sender.RefreshTabs());
        SelectedIndexProperty.Changed.AddClassHandler<Ribbon>((x, e) => x.RefreshSelectedGroups());
        
        OrientationProperty.Changed.AddClassHandler<Ribbon, Orientation>((ribbon, args) =>
        {
            var orientation = args.NewValue.Value;
            ribbon.ChangeGroupsOrientation(orientation);
            if (ribbon.Menu != null) ribbon.Menu.Orientation = orientation;
            ribbon.RefreshSelectedGroups();
        });
        
        // AccessKeyHandler.AccessKeyPressedEvent.AddClassHandler<Ribbon>((sender, e) =>
        // {
        // if (e.Source is Control ctrl)
        // (sender as Ribbon).HandleKeyTipControl(ctrl);
        // });

        KeyTip.ShowChildKeyTipKeysProperty.Changed.AddClassHandler<Ribbon>((sender, args) =>
        {
            var isOpen = (bool)args.NewValue;
            if (isOpen)
                sender.Focus();
            sender.SetChildKeyTipsVisibility(isOpen);
        });
    }


    protected override Type StyleKeyOverride { get; } = typeof(Ribbon);

    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }
    
    public ICommand HelpButtonCommand
    {
        get => _helpCommand;
        set => SetAndRaise(HelpButtonCommandProperty, ref _helpCommand, value);
    }

    public ObservableCollection<RibbonGroupBox> SelectedGroups
    {
        get => _selectedGroups;
        set => SetAndRaise(SelectedGroupsProperty, ref _selectedGroups, value);
    }

    public ObservableCollection<Control> Tabs
    {
        get => _tabs;
        set => SetAndRaise(TabsProperty, ref _tabs, value);
    }

    public bool IsCollapsed
    {
        get => GetValue(IsCollapsedProperty);
        set => SetValue(IsCollapsedProperty, value);
    }
    

    public RibbonMenu Menu
    {
        get => GetValue(MenuProperty);
        set => SetValue(MenuProperty, value);
    }

    public bool IsMenuOpen
    {
        get => GetValue(IsMenuOpenProperty);
        set => SetValue(IsMenuOpenProperty, value);
    }
    
    public bool IsOpen
    {
        get => _isOpen;
        protected set => SetAndRaise(IsOpenProperty, ref _isOpen, value);
    }

    public void ActivateKeyTips(Ribbon ribbon, IKeyTipHandler prev)
    {
        foreach (RibbonTab t in Items)
            KeyTip.GetKeyTipKeys(t);

        if (Menu != null)
            KeyTip.GetKeyTipKeys(Menu as Control);
    }

    public bool HandleKeyTipKeyPress(Key key)
    {
        var retVal = false;
        if (IsOpen)
        {
            var tabKeyMatched = false;
            foreach (RibbonTab t in Items)
                if (KeyTip.HasKeyTipKey(t, key))
                {
                    SelectedItem = t;
                    tabKeyMatched = true;
                    retVal = true;
                    t.ActivateKeyTips(this, this);
                    break;
                }

            if (!tabKeyMatched && Menu != null)
                if (KeyTip.HasKeyTipKey(Menu as Control, key))
                {
                    IsMenuOpen = true;
                    if (Menu is IKeyTipHandler handler) handler.ActivateKeyTips(this, this);
                    retVal = true;
                }
        }

        return retVal;
    }

    private void RefreshSelectedGroups()
    {
        if (SelectedItem is not RibbonTab tab) 
            return;
        
        SelectedGroups.Clear();
        tab.Groups.ForEach(t => SelectedGroups.Add(t));
    }

    private void RefreshTabs()
    {
        Items.Clear();
        var tabs = Tabs.AsEnumerable();
        if (Orientation == Orientation.Vertical)
            tabs = tabs.Reverse();
        
        foreach (var ctrl in tabs)
            switch (ctrl)
            {
                case RibbonContextualTabGroup ctx:
                    ctx.Items.OfType<RibbonTab>().ForEach(tab => Items.Add(tab));
                    break;
                case RibbonTab tab:
                    Items.Add(tab);
                    break;
            }
        
        SelectedItem = Tabs.FirstOrDefault();
    }

    private void ChangeGroupsOrientation(Orientation orientation)
    {
        Tabs.OfType<RibbonTab>().SelectMany(t => t.Groups).ForEach(g => g.Orientation = orientation);
    }

    private void SetChildKeyTipsVisibility(bool open)
    {
        foreach (RibbonTab t in Items) KeyTip.GetKeyTip(t).IsOpen = open;
        if (Menu != null)
            KeyTip.GetKeyTip(Menu as Control).IsOpen = open;
    }

    protected override void OnLostFocus(RoutedEventArgs e)
    {
        base.OnLostFocus(e);
        KeyTip.SetShowChildKeyTipKeys(this, false);
    }

    public event EventHandler<RoutedEventArgs> MenuClosed
    {
        add => AddHandler(MenuClosedEvent, value);
        remove => RemoveHandler(MenuClosedEvent, value);
    }

    /*
    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        var newIndex = SelectedIndex;

        if (ItemCount > 1)
        {
            if ((Orientation == Orientation.Horizontal && e.Delta.Y > 0) || (Orientation == Orientation.Vertical && e.Delta.Y < 0))
                /*while (newIndex > 0)
                {
                    newIndex--;
                    var newTab = Items.OfType<RibbonTab>().ElementAt(newIndex);
                    if (newTab.IsEffectivelyVisible && newTab.IsEnabled)
                    {
                        switchTabs = true;
                        break;
                    }
                }#1#
                CycleTabs(false);
            else if ((Orientation == Orientation.Horizontal && e.Delta.Y < 0) || (Orientation == Orientation.Vertical && e.Delta.Y > 0))
                /*while (newIndex < (ItemCount - 1))
                {
                    newIndex++;
                    var newTab = Items.OfType<RibbonTab>().ElementAt(newIndex);
                    if (newTab.IsEffectivelyVisible && newTab.IsEnabled)
                    {
                        switchTabs = true;
                        break;
                    }
                }#1#
                CycleTabs(true);
        }
        /*if (switchTabs)
            SelectedIndex = newIndex;#1#

        base.OnPointerWheelChanged(e);
    }
    */

    public void CycleTabs(bool forward)
    {
        var switchTabs = false;
        //var tabs = ((AvaloniaList<object>)Items).OfType<RibbonTab>().Where(x => x.IsEffectivelyVisible && x.IsEnabled);
        var newIndex = SelectedIndex;
        Action stepIndex;
        Func<bool> verifyIndex;

        if (forward)
        {
            stepIndex = () => newIndex++;
            verifyIndex = () => newIndex < ItemCount - 1;
        }
        else
        {
            stepIndex = () => newIndex--;
            verifyIndex = () => newIndex > 0;
        }


        /*while (newIndex < ((AvaloniaList<object>)Items).Count)
        {
            step();
            RibbonTab newSel = (RibbonTab)(((AvaloniaList<object>)Items).ElementAt(newIndex));
            bool contextualVisible = true;
            if (newSel.IsContextual)
                contextualVisible = (newSel.Parent as RibbonContextualTabGroup).IsVisible;
            if (newSel.IsVisible && newSel.IsEnabled && contextualVisible)
            {
                SelectedIndex = newIndex;
                break;
            }
        }*/
        while (verifyIndex())
        {
            stepIndex();
            var newTab = Items.OfType<RibbonTab>().ElementAt(newIndex);

            var contextualVisible = true;
            if (newTab.IsContextual)
                contextualVisible = (newTab.Parent as RibbonContextualTabGroup).IsVisible;
            if (newTab.IsEffectivelyVisible && newTab.IsEnabled && contextualVisible)
            {
                switchTabs = true;
                break;
            }
        }

        if (switchTabs)
            SelectedIndex = newIndex;
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        
        // if (inputRoot?.AccessKeyHandler != null)
        // inputRoot.AccessKeyHandler.MainMenu = this;

        if (e.Root is WindowBase wnd)
            wnd.Deactivated += InputRoot_Deactivated;

        RefreshTabs();
        ChangeGroupsOrientation(Orientation);
        if (Menu != null) Menu.Orientation = Orientation;
        RefreshSelectedGroups();
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        
        if (e.Root is WindowBase wnd)
            wnd.Deactivated -= InputRoot_Deactivated;
    }

    private void InputRoot_Deactivated(object sender, EventArgs e)
    {
        Close();
    }

    public void Close()
    {
        if (!IsOpen)
            return;

        KeyTip.SetShowChildKeyTipKeys(this, false);
        IsOpen = false;
        _prevFocusedElement.Focus();
        //FocusManager.Instance.Focus(_prevFocusedElement);

        RaiseEvent(new RoutedEventArgs
        {
            RoutedEvent = MenuClosedEvent,
            Source = this
        });
    }

    public void Open()
    {
        if (IsOpen)
            return;

        IsOpen = true;
        _prevFocusedElement = TopLevel.GetTopLevel(this).FocusManager.GetFocusedElement();
        //_prevFocusedElement = FocusManager.Instance.Current;

        Focus();
        KeyTip.SetShowChildKeyTipKeys(this, true);

        RaiseEvent(new RoutedEventArgs
        {
            RoutedEvent = RibbonKeyTipsOpenedEvent,
            Source = this
        });
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (IsFocused)
        {
            if (e.Key == Key.LeftAlt || e.Key == Key.RightAlt || e.Key == Key.F10 || e.Key == Key.Escape)
                Close();
            else
                HandleKeyTipKeyPress(e.Key);
        }
    }

    // private void HandleKeyTipControl(Control item)
    // {
    //     item.RaiseEvent(new RoutedEventArgs(PointerPressedEvent));
    //     item.RaiseEvent(new RoutedEventArgs(PointerReleasedEvent));
    // }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _groupsHost = e.NameScope.Find<ItemsControl>("PART_SelectedGroupsHost");
        _itemHeadersPresenter = e.NameScope.Find<ItemsPresenter>("PART_ItemsPresenter");

        var secondClick = false;

        /*_itemHeadersPresenter.PointerReleased += (sneder, args) =>
        {
            if (IsCollapsed)
            {
                RibbonTab mouseOverItem = null;
                foreach (RibbonTab tab in Items)
                    if (tab.IsPointerOver)
                    {
                        mouseOverItem = tab;
                        break;
                    }

                if (mouseOverItem != null)
                {
                    if (SelectedItem != mouseOverItem)
                        SelectedItem = mouseOverItem;
                    if (secondClick)
                        secondClick = false;
                }
            }
            else
            {
                foreach (RibbonTab tab in Items)
                    if (tab.IsPointerOver && tab.IsContextual)
                    {
                        SelectedItem = tab;
                        break;
                    }
            }
        };*/
        
        /*_itemHeadersPresenter.DoubleTapped += (sneder, args) =>
        {
            if (IsCollapsed)
            {
                if (IsCollapsedPopupOpen)
                    IsCollapsedPopupOpen = false;
                IsCollapsed = false;
            }
            else
            {
                IsCollapsed = true;
                secondClick = true;
            }
        };*/

        // var pinToQat = e.NameScope.Find<MenuItem>("PART_PinLastHoveredControlToQuickAccess");
        // pinToQat.Click += (sneder, args) =>
        // {
        //     if (_rightClicked != null)
        //         QuickAccessToolbar?.AddItem(_rightClicked);
        // };

        //_ctxMenu = e.NameScope.Find<ContextMenu>("PART_ContentAreaContextMenu");
        // e.NameScope.Find<MenuItem>("PART_CollapseRibbon").Click += (sneder, args) =>
        // {
        //     IsCollapsed = !IsCollapsed;
        // };

        /*_groupsHost.PointerExited += (sneder, args) =>
        {
            if (!_ctxMenu.IsOpen)
                _rightClicked = null;
        };

        _groupsHost.AddHandler(PointerReleasedEvent,
            (sneder, args) =>
            {
                if (args.Source != null)
                {
                    var ctrl = VisualExtensions.FindAncestorOfType<ICanAddToQuickAccess>(args.Source as Visual);

                    _rightClicked = ctrl;

                    if (QuickAccessToolbar != null)
                        pinToQat.IsEnabled = _rightClicked is { CanAddToQuickAccess: true } && !QuickAccessToolbar.ContainsItem(_rightClicked);
                    else
                        pinToQat.IsEnabled = false;
                }
            }, handledEventsToo: true);*/
    }


}