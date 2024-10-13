using System;
using System.Collections;
using System.Linq;
using System.Timers;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.Threading;

namespace AvaloniaUI.Ribbon;

public class RibbonMenu : ItemsControl, IRibbonMenu
{
    private static readonly FuncTemplate<Panel> DefaultPanel = new(() => new StackPanel());
    
    public static readonly StyledProperty<object> ContentProperty = ContentControl.ContentProperty.AddOwner<RibbonMenu>();
    public static readonly StyledProperty<bool> IsMenuOpenProperty = AvaloniaProperty.Register<RibbonMenu, bool>(nameof(IsMenuOpen));
    public static readonly StyledProperty<object> SelectedSubItemsProperty = AvaloniaProperty.Register<RibbonMenu, object>(nameof(SelectedSubItems));
    public static readonly StyledProperty<bool> HasSelectedItemProperty = AvaloniaProperty.Register<RibbonMenu, bool>(nameof(HasSelectedItem));
    public static readonly StyledProperty<string> RightColumnHeaderProperty = AvaloniaProperty.Register<RibbonMenu, string>(nameof(RightColumnHeader));
    public static readonly DirectProperty<RibbonMenu, IEnumerable> RightColumnItemsProperty =
        AvaloniaProperty.RegisterDirect<RibbonMenu, IEnumerable>(nameof(RightColumnItems), o => o.RightColumnItems, (o, v) => o.RightColumnItems = v);
    public static readonly StyledProperty<ITemplate<Panel>> RightColumnItemsPanelProperty =
        AvaloniaProperty.Register<RibbonMenu, ITemplate<Panel>>(nameof(RightColumnItemsPanel), DefaultPanel);
    public static readonly StyledProperty<IDataTemplate> RightColumnItemTemplateProperty =
        AvaloniaProperty.Register<RibbonMenu, IDataTemplate>(nameof(RightColumnItemTemplate));
    
    private IDisposable _disposable;
    private RibbonMenuItem _previousSelectedItem;
    private IEnumerable _rightColumnItems = new AvaloniaList<object>();


    static RibbonMenu()
    {
        IsMenuOpenProperty.Changed.AddClassHandler<RibbonMenu>((sender, e) =>
        {
            if (!(bool)e.NewValue)
            {
                sender.SelectedSubItems = null;
                sender.HasSelectedItem = false;

                if (sender._previousSelectedItem != null)
                    sender._previousSelectedItem.IsSelected = false;
            }
        });
    }

    public object Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public object SelectedSubItems
    {
        get => GetValue(SelectedSubItemsProperty);
        set => SetValue(SelectedSubItemsProperty, value);
    }

    public bool HasSelectedItem
    {
        get => GetValue(HasSelectedItemProperty);
        set => SetValue(HasSelectedItemProperty, value);
    }

    public string RightColumnHeader
    {
        get => GetValue(RightColumnHeaderProperty);
        set => SetValue(RightColumnHeaderProperty, value);
    }

    public IEnumerable RightColumnItems
    {
        get => _rightColumnItems;
        set => SetAndRaise(RightColumnItemsProperty, ref _rightColumnItems, value);
    }

    public ITemplate<Panel> RightColumnItemsPanel
    {
        get => GetValue(RightColumnItemsPanelProperty);
        set => SetValue(RightColumnItemsPanelProperty, value);
    }

    public IDataTemplate RightColumnItemTemplate
    {
        get => GetValue(RightColumnItemTemplateProperty);
        set => SetValue(RightColumnItemTemplateProperty, value);
    }

    public bool IsMenuOpen
    {
        get => GetValue(IsMenuOpenProperty);
        set => SetValue(IsMenuOpenProperty, value);
    }


    protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnAttachedToLogicalTree(e);

        _disposable = this.GetPropertyChangedObservable(ItemsSourceProperty).Subscribe(args => { ResetItemHoverEvents(); });

        Items.GetWeakCollectionChangedObservable().Subscribe(args => { ResetItemHoverEvents(); });
    }

    protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        _disposable.Dispose();

        base.OnDetachedFromLogicalTree(e);
    }


    private void ResetItemHoverEvents()
    {
        foreach (var item in Items.OfType<RibbonMenuItem>())
        {
            item.PointerEntered -= Item_PointerEnter;
            item.PointerEntered += Item_PointerEnter;
        }
    }

    private void Item_PointerEnter(object sender, PointerEventArgs e)
    {
        if (sender is RibbonMenuItem item)
        {
            var counter = 0;
            var timer = new Timer(1);
            timer.Elapsed += (sneder, args) =>
            {
                if (counter < 25)
                {
                    counter++;
                }
                else
                {
                    Dispatcher.UIThread.Post(() =>
                    {
                        if (item.IsPointerOver)
                        {
                            if (item.HasItems)
                            {
                                SelectedSubItems = item.Items;
                                HasSelectedItem = true;

                                item.IsSelected = true;

                                if (_previousSelectedItem != null)
                                    _previousSelectedItem.IsSelected = false;

                                _previousSelectedItem = item;
                            }
                            else
                            {
                                SelectedSubItems = null;
                                HasSelectedItem = false;

                                if (_previousSelectedItem != null)
                                    _previousSelectedItem.IsSelected = false;
                            }
                        }
                    });

                    timer.Stop();
                }
            };
            timer.Start();
        }
    }
}