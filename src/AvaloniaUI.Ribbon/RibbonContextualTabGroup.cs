using Avalonia;
using Avalonia.Collections;
using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.LogicalTree;
using VisualExtensions = Avalonia.VisualTree.VisualExtensions;

namespace AvaloniaUI.Ribbon;

public class RibbonContextualTabGroup : HeaderedItemsControl
{
    static RibbonContextualTabGroup()
    {
        IsVisibleProperty.Changed.AddClassHandler<RibbonContextualTabGroup>((sender, e) =>
        {
            if (e.NewValue is bool and false)
                sender.SwitchToNextVisibleTab();
        });
    }

    void SwitchToNextVisibleTab()
    {
        Ribbon rbn = VisualExtensions.FindAncestorOfType<Ribbon>(this, true);
        if (rbn != null && ((IAvaloniaList<object>)Items).Contains(rbn.SelectedItem))
        {
            int selIndex = rbn.SelectedIndex;
                
            rbn.CycleTabs(false);
                
            if (selIndex == rbn.SelectedIndex)
                rbn.CycleTabs(true);
        }
        /*var selectableItems = ((IAvaloniaList<object>)rbn.Items).OfType<RibbonTab>().Where(x => x.IsVisible && x.IsEnabled);
        RibbonTab targetTab = null;
        foreach (RibbonTab tab in selectableItems)
        {
            if (((IAvaloniaList<object>)Items).Contains(tab))
                break;

            targetTab = tab;
        }

        if (targetTab == null)
        {
            selectableItems = selectableItems.Reverse();

            foreach (RibbonTab tab in selectableItems)
            {
                if (((IAvaloniaList<object>)Items).Contains(tab))
                    break;

                targetTab = tab;
            }
        }
        int index = ((IAvaloniaList<object>)rbn.Items).IndexOf(targetTab);
        rbn.SelectedIndex = index;
        //if (index > 0)
        */
    }

    protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnAttachedToLogicalTree(e);

        Items.GetWeakCollectionChangedObservable().Subscribe(e =>
        {
            if (e.OldItems != null)
            {
                foreach (RibbonTab tab in e.OldItems.OfType<RibbonTab>())
                    tab.IsContextual = false;
            }

            if (e.NewItems != null)
            {
                foreach (RibbonTab tab in e.NewItems.OfType<RibbonTab>())
                    tab.IsContextual = true;
            }
        });
    }

    protected override Type StyleKeyOverride { get; } = typeof(RibbonContextualTabGroup);
}