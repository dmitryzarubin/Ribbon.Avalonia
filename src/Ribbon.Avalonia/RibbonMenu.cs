using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.ReactiveUI;
using DynamicData;
using DynamicData.Binding;

namespace Ribbon.Avalonia;

public class RibbonMenu : ItemsControl
{
    public static readonly StyledProperty<IControlTemplate> IconProperty =
        AvaloniaProperty.Register<RibbonMenuButtonItem, IControlTemplate>(nameof(Icon));

    public static readonly StyledProperty<bool> IsMenuOpenProperty = AvaloniaProperty.Register<RibbonMenu, bool>(nameof(IsMenuOpen));
    public static readonly StyledProperty<Size> PopupSizeProperty = AvaloniaProperty.Register<RibbonMenu, Size>(nameof(PopupSize), new Size(800, 500));
    public static readonly StyledProperty<int> LeftMenuWidthProperty = AvaloniaProperty.Register<RibbonMenu, int>(nameof(LeftMenuWidth), 200);

    public static readonly StyledProperty<Orientation> OrientationProperty = AvaloniaProperty.Register<Ribbon, Orientation>(nameof(Orientation));
    
    public static readonly DirectProperty<RibbonMenu, RibbonMenuTabItem?> CurrentTabProperty =
        AvaloniaProperty.RegisterDirect<RibbonMenu, RibbonMenuTabItem?>(nameof(CurrentTab), o => o.CurrentTab, (o, v) => o.CurrentTab = v);
    
    public static readonly DirectProperty<RibbonMenu, ReadOnlyObservableCollection<IRibbonMenuItem>> TopListProperty =
        AvaloniaProperty.RegisterDirect<RibbonMenu, ReadOnlyObservableCollection<IRibbonMenuItem>>(nameof(TopList), o => o._topList);

    public static readonly DirectProperty<RibbonMenu, ReadOnlyObservableCollection<IRibbonMenuItem>> BottomListProperty =
        AvaloniaProperty.RegisterDirect<RibbonMenu, ReadOnlyObservableCollection<IRibbonMenuItem>>(nameof(BottomList), o => o._bottomList);

    private readonly ReadOnlyObservableCollection<IRibbonMenuItem> _bottomList;
    private readonly ReadOnlyObservableCollection<IRibbonMenuItem> _topList;
    private RibbonMenuTabItem? _currentTab;


    static RibbonMenu()
    {
        // SelectedItemProperty.Changed.AddClassHandler<RibbonMenu>((menu, args) =>
        // {
        //     if (args.NewValue is RibbonMenuTabItem tabItem)
        //         menu.CurrentTab = tabItem;
        // });

        ItemsSourceProperty.Changed.AddClassHandler<RibbonMenu>((menu, _) => { menu.TrySetDefaultTab(); });

        IsMenuOpenProperty.Changed.AddClassHandler<RibbonMenu>((menu, args) =>
        {
            if ((bool?)args.NewValue == false) menu.TrySetDefaultTab();
        });
    }


    public RibbonMenu()
    {
        Items.GetWeakCollectionChangedObservable().Subscribe(_ => TrySetDefaultTab());

        Items
            .ToObservableChangeSet<ItemCollection, object?>()
            .Transform(x => x as IRibbonMenuItem)
            .Filter(x => x != null)
            .Transform(x => x!)
            .AutoRefresh(x => x.Placement)
            .Filter(x => x.Placement == RibbonMenuItemPlacement.Top)
            .ObserveOn(AvaloniaScheduler.Instance)
            .Bind(out _topList)
            .Subscribe();

        Items
            .ToObservableChangeSet<ItemCollection, object?>()
            .Transform(x => x as IRibbonMenuItem)
            .Filter(x => x != null)
            .Transform(x => x!)
            .AutoRefresh(x => x.Placement)
            .Filter(x => x.Placement == RibbonMenuItemPlacement.Bottom)
            .ObserveOn(AvaloniaScheduler.Instance)
            .Bind(out _bottomList)
            .Subscribe();
        
        Items
            .ToObservableChangeSet<ItemCollection, object?>()
            .Transform(x => x as RibbonMenuTabItem)
            .Filter(x => x != null)
            .Transform(x => x!)
            .AutoRefresh(x => x.IsChecked)
            .ObserveOn(AvaloniaScheduler.Instance)
            .Subscribe(set =>
            {
                foreach (var change in set)
                    if (change.Reason == ListChangeReason.Add || change.Reason == ListChangeReason.Refresh)
                    {
                        var ribbonMenuTabItem = change.Item.Current;
                        if (ribbonMenuTabItem.IsChecked == true)
                        {
                            Items
                                .OfType<RibbonMenuTabItem>()
                                .Where(x => x != ribbonMenuTabItem)
                                .ForEach(x => x.IsChecked = false);
                            
                            CurrentTab = ribbonMenuTabItem;
                        }
                    }
            });
        
        this.AddHandler(Button.ClickEvent, OnMenuButtonClick, RoutingStrategies.Tunnel | RoutingStrategies.Bubble);
    }

    private void OnMenuButtonClick(object? sender, RoutedEventArgs e)
    {
        if (e.Source is RibbonMenuButtonItem)
        {
            IsMenuOpen = false;
        }
    }

    public ReadOnlyObservableCollection<IRibbonMenuItem> TopList => GetValue(TopListProperty);

    public ReadOnlyObservableCollection<IRibbonMenuItem> BottomList => GetValue(BottomListProperty);

    protected override Type StyleKeyOverride { get; } = typeof(RibbonMenu);

    public Size PopupSize
    {
        get => GetValue(PopupSizeProperty);
        set => SetValue(PopupSizeProperty, value);
    }
    
    public int LeftMenuWidth
    {
        get => GetValue(LeftMenuWidthProperty);
        set => SetValue(LeftMenuWidthProperty, value);
    }
    
    public IControlTemplate Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }
    
    public RibbonMenuTabItem? CurrentTab
    {
        get => _currentTab;
        set => SetAndRaise(CurrentTabProperty, ref _currentTab, value);
    }

    public bool IsMenuOpen
    {
        get => GetValue(IsMenuOpenProperty);
        set => SetValue(IsMenuOpenProperty, value);
    }

    private void TrySetDefaultTab()
    {
        var defaultTab = Items.OfType<RibbonMenuTabItem>().FirstOrDefault();
        if (defaultTab != null) 
            defaultTab.IsChecked = true;
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        
        var border = e.NameScope.Find<Border>("PART_MenuButtonBorder");
        border.PointerReleased += (sender, args) =>
        {
            IsMenuOpen = !IsMenuOpen;
        };
    }
}