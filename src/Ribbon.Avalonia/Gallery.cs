using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Ribbon.Avalonia.Enums;

namespace Ribbon.Avalonia;

public class Gallery : ListBox, IRibbonControl
{
    public static readonly AvaloniaProperty<RibbonControlSize> SizeProperty;
    public static readonly AvaloniaProperty<RibbonControlSize> MinSizeProperty;
    public static readonly AvaloniaProperty<RibbonControlSize> MaxSizeProperty;
    public static readonly StyledProperty<double> ItemHeightProperty = AvaloniaProperty.Register<Gallery, double>(nameof(ItemHeight));
    public static readonly StyledProperty<bool> IsDropDownOpenProperty;

    private ContentControl _flyoutPresenter;
    private ItemsPresenter _itemsPresenter;
    private ContentControl _mainPresenter;

    static Gallery()
    {
        IsDropDownOpenProperty = ComboBox.IsDropDownOpenProperty.AddOwner<Gallery>();
        // IsDropDownOpenProperty.Changed.AddClassHandler(new Action<Gallery, AvaloniaPropertyChangedEventArgs>((sneder, args) =>
        // {
        //     sneder.UpdatePresenterLocation((bool)args.NewValue);
        // }));

        RibbonControlHelper<Gallery>.SetProperties(out SizeProperty, out MinSizeProperty, out MaxSizeProperty);
    }

    protected override Type StyleKeyOverride { get; } = typeof(Gallery);

    public double ItemHeight
    {
        get => GetValue(ItemHeightProperty);
        set => SetValue(ItemHeightProperty, value);
    }
    
    public bool IsDropDownOpen
    {
        get => GetValue(IsDropDownOpenProperty);
        set => SetValue(IsDropDownOpenProperty, value);
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

        _itemsPresenter = e.NameScope.Find<ItemsPresenter>("PART_ItemsPresenter");
        _mainPresenter = e.NameScope.Find<ContentControl>("PART_ItemsPresenterHolder");
        _flyoutPresenter = e.NameScope.Find<ContentControl>("PART_FlyoutItemsPresenterHolder");
        
        var presenter = e.NameScope.Find<GalleryScrollContentPresenter>("PART_ScrollContentPresenter");
        var repeatUpButton = e.NameScope.Find<RepeatButton>("PART_UpButton");
        var repeatDownButton = e.NameScope.Find<RepeatButton>("PART_DownButton");
        var flyoutRoot = e.NameScope.Find<Control>("PART_FlyoutRoot");
        
        repeatUpButton.Click += (sneder, args) => presenter.Offset = presenter.Offset.WithY(Math.Max(0, presenter.Offset.Y - ItemHeight));
        repeatDownButton.Click += (sneder, args) => presenter.Offset = presenter.Offset.WithY(Math.Min(presenter.Offset.Y + ItemHeight, _mainPresenter.Bounds.Height - presenter.Bounds.Height));
        flyoutRoot.PointerExited += (sneder, a) => IsDropDownOpen = false;

        //_flyoutPresenter.PointerWheelChanged += (s, a) => { a.Handled = true; };
        
        UpdatePresenterLocation(IsDropDownOpen);
    }

    private void UpdatePresenterLocation(bool intoFlyout)
    {
        if (_itemsPresenter.Parent is ContentPresenter presenter)
            presenter.Content = null;
        else if (_itemsPresenter.Parent is ContentControl control)
            control.Content = null;
        else if (_itemsPresenter.Parent is Panel panel)
            panel.Children.Remove(_itemsPresenter);

        if (intoFlyout)
            _flyoutPresenter.Content = _itemsPresenter;
        else
            _mainPresenter.Content = _itemsPresenter;
    }
}