using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Collections.ObjectModel;
using VisualExtensions = Avalonia.VisualTree.VisualExtensions;

namespace Ribbon.Avalonia;

internal static class Extensions
{
    public static T Do<T>(this T source,  Action<T> action)
    {
        action(source);
        return source;
    }
}
    
    
public class QuickAccessToolbar : ItemsControl
{
    public static readonly StyledProperty<Ribbon> RibbonProperty = AvaloniaProperty.Register<QuickAccessToolbar, Ribbon>(nameof(Ribbon));
    public Ribbon Ribbon
    {
        get => GetValue(RibbonProperty);
        set => SetValue(RibbonProperty, value);
    }
        
    public static readonly DirectProperty<QuickAccessToolbar, ObservableCollection<QuickAccessRecommendation>> RecommendedItemsProperty = AvaloniaProperty.RegisterDirect<QuickAccessToolbar, ObservableCollection<QuickAccessRecommendation>>(nameof(RecommendedItems), o => o.RecommendedItems, (o, v) => o.RecommendedItems = v);
    private ObservableCollection<QuickAccessRecommendation> _recommendedItems = new();
    public ObservableCollection<QuickAccessRecommendation> RecommendedItems
    {
        get => _recommendedItems;
        set => SetAndRaise(RecommendedItemsProperty, ref _recommendedItems, value);
    }


    public static readonly AttachedProperty<bool> IsCheckedProperty = AvaloniaProperty.RegisterAttached<QuickAccessToolbar, MenuItem, bool>("IsChecked");

    public static bool GetIsChecked(MenuItem element)
    {
        return element.GetValue(IsCheckedProperty);
    }

    public static void SetIsChecked(MenuItem element, bool value)
    {
        element.SetValue(IsCheckedProperty, value);
    }

    static readonly string FIXED_ITEM_CLASS = "quickAccessFixedItem";
        
    static QuickAccessToolbar()
    {
        RibbonProperty.Changed.AddClassHandler<QuickAccessToolbar>((sender, e) => {
            if (sender.Ribbon != null)
                sender._collapseRibbonItem[!IsCheckedProperty] = sender.Ribbon[!Ribbon.IsCollapsedProperty];
            else
                SetIsChecked(sender._collapseRibbonItem, false);
        });
    }


    private readonly MenuItem _collapseRibbonItem = new MenuItem().Do(x => x.Classes.Add(FIXED_ITEM_CLASS));
        
        
        
    public QuickAccessToolbar() : base()
    {
        //_collapseRibbonItem.Header = new DynamicResourceExtension("AvaloniaRibbon.MinimizeRibbon"); // "Minimize the Ribbon";
        _collapseRibbonItem[!MenuItem.HeaderProperty] = _collapseRibbonItem.GetResourceObservable("AvaloniaRibbon.MinimizeRibbon").ToBinding();
        _collapseRibbonItem[!IsEnabledProperty] = this.GetObservable(RibbonProperty).Select(x => x != null).ToBinding();
        _collapseRibbonItem.Click += (sneder, e) =>
        {
            if (Ribbon != null)
                Ribbon.IsCollapsed = !Ribbon.IsCollapsed;
        };
    }

    protected override Type StyleKeyOverride { get; } = typeof(QuickAccessToolbar);
        
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        var more = e.NameScope.Find<ToggleButton>("PART_MoreButton");
        var morCtx = more.ContextMenu;

        MenuItem moreCmdItem = new MenuItem()
        {
            //Header =  new DynamicResourceExtension()., //"More commands...",
            IsEnabled = false, //[!IsEnabledProperty] = this.GetObservable(RibbonProperty).Select(x => x != null).ToBinding(),
        };
        moreCmdItem.Classes.Clear();
        moreCmdItem.Classes.Add(FIXED_ITEM_CLASS);
            
        moreCmdItem[!MenuItem.HeaderProperty] = moreCmdItem.GetResourceObservable("AvaloniaRibbon.MoreQATCommands").ToBinding();

            
            

        morCtx.Opened += (sneder, a) => 
        {
            if (more.IsChecked != true)
                more.IsChecked = true;

            ObservableCollection<object> morCtxItems = new ObservableCollection<object>();
            foreach (QuickAccessRecommendation rcm in RecommendedItems)
            {
                rcm.IsChecked = ContainsItem(rcm.Item);
                morCtxItems.Add(rcm);
            }

            morCtxItems.Add(new Separator());
            morCtxItems.Add(moreCmdItem);
            morCtxItems.Add(_collapseRibbonItem);
            morCtx.Items.Clear();
            foreach (var item in morCtxItems)
            {
                morCtx.Items.Add(item);
            }
        };

        morCtx.Closed += (sneder, a) =>
        {
            if (more.IsChecked == true)
                more.IsChecked = false;
        };

        more.Checked += (sneder, a) => morCtx.Open(more);
        more.Unchecked += (sneder, a) => morCtx.Close();
    }

    /*protected override void ItemsChanged(AvaloniaPropertyChangedEventArgs e)
    {
        base.ItemsChanged(e);
        RefreshItems();
    }

    protected override void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        base.ItemsCollectionChanged(sender, e);
        RefreshItems();
    }

    void RefreshItems()
    {
        panel.Children.Clear();

        foreach (Control itm in ((AvaloniaList<object>)Items).OfType<Control>())
            panel.Children.Add(itm);
    }*/

    // protected override IItemContainerGenerator CreateItemContainerGenerator()
    // {
    //     return new ItemContainerGenerator<QuickAccessItem>(this, QuickAccessItem.ItemProperty, QuickAccessItem.ContentTemplateProperty);
    // }

    public bool ContainsItem(ICanAddToQuickAccess item) => ContainsItem(item, out object result);
    public bool ContainsItem(ICanAddToQuickAccess item, out object result)
    {
        if (Items.OfType<ICanAddToQuickAccess>().Contains(item))
        {
            result = Items.OfType<ICanAddToQuickAccess>().First();
            return true;
        }
        else if (Items.OfType<QuickAccessItem>().Any(x => x.Item == item))
        {
            result = Items.OfType<QuickAccessItem>().First(x => x.Item == item);
            return true;
        }
        else
        {
            result = null;
            return false;
        }
    }

    public bool AddItem(ICanAddToQuickAccess item)
    {
        bool contains = ContainsItem(item, out object obj);
        if (item == null || contains)
            return false;
        else
        {
            ICanAddToQuickAccess itm = item;
            if (obj is QuickAccessItem qai)
                itm = qai.Item;
                
            if (itm.CanAddToQuickAccess && !Items.IsReadOnly)
            {
                Items.Add(item);
                return true;
            }
        }
            
        return false;
    }

    public bool RemoveItem(ICanAddToQuickAccess item)
    {
        bool contains = ContainsItem(item, out object obj);
        if (item == null || !contains)
            return false;
        else
        {
            var items = Items.OfType<object>().ToList();
            items.Remove(items.First(x => 
            {
                if (x == item)
                    return true;
                else if (x is QuickAccessItem itm && itm.Item == item)
                    return true;
                    
                return false;
            }));
            foreach (var x in items)
            {
                Items.Add(x);
            }
            return true;
        }
    }

    public void MoreFlyoutMenuItemCommand(object parameter)
    {
        if (parameter is ICanAddToQuickAccess item)
        {
            if (!AddItem(item))
                RemoveItem(item);
        }
        else if (parameter is Action cmd)
            cmd();
    }
}

    
public class QuickAccessItem : ContentControl, IStyleable
{
    public static readonly StyledProperty<ICanAddToQuickAccess> ItemProperty = AvaloniaProperty.Register<QuickAccessItem, ICanAddToQuickAccess>(nameof(Item), null);
    public ICanAddToQuickAccess Item
    {
        get => GetValue(ItemProperty);
        set => SetValue(ItemProperty, value);
    }

    Type IStyleable.StyleKey => typeof(QuickAccessItem);

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        e.NameScope.Find<MenuItem>("PART_RemoveFromQuickAccessToolbar").Click += (sneder, args) => VisualExtensions.FindAncestorOfType<QuickAccessToolbar>(this)?.RemoveItem(Item);
    }
}

public class QuickAccessRecommendation : AvaloniaObject//INotifyPropertyChanged
{
    public static readonly StyledProperty<ICanAddToQuickAccess> ItemProperty = QuickAccessItem.ItemProperty.AddOwner<QuickAccessRecommendation>();
    public ICanAddToQuickAccess Item
    {
        get => GetValue(ItemProperty);
        set => SetValue(ItemProperty, value);
    }
        
    public static readonly StyledProperty<bool?> IsCheckedProperty = ToggleButton.IsCheckedProperty.AddOwner<QuickAccessRecommendation>();
    public bool? IsChecked
    {
        get => GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    /*void NotifyPropertyChanged([CallerMemberName]string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    public event PropertyChangedEventHandler PropertyChanged;*/
}