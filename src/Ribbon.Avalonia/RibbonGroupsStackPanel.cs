using System;
using System.Collections.Specialized;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.LogicalTree;
using Avalonia.Threading;
using Ribbon.Avalonia.Enums;

namespace Ribbon.Avalonia;

public class RibbonGroupsStackPanel : StackPanel
{
    static RibbonGroupsStackPanel()
    {
        ParentProperty.Changed.AddClassHandler<RibbonGroupsStackPanel>((sender, e) =>
        {
            Dispatcher.UIThread.Post(sender.SizeControls);
        });

        BoundsProperty.Changed.AddClassHandler<RibbonGroupsStackPanel>((sender, e) =>
        {
            if (e.NewValue is Rect newRect)
                sender.SizeControls(newRect.Size);
        });
    }

    protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnAttachedToLogicalTree(e);

        SizeControls();
    }

    protected override void LogicalChildrenCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        base.LogicalChildrenCollectionChanged(sender, e);
        
        //var size = new Size(double.PositiveInfinity, double.PositiveInfinity);
        //Measure(size);
        //SizeControls(size);
        SizeControls();
    }

    private void SizeControls()
    {
        SizeControls(Bounds.Size);
    }

    private void SizeControls(Size newSize)
    {
        if (Orientation == Orientation.Vertical)
        {
            if (GetChildrenTotalHeight() >= newSize.Height)
                foreach (var child in Children.Reverse().OfType<RibbonGroupBox>().Where(x => x.DisplayMode == GroupDisplayMode.Large))
                {
                    UpdateGroupBoxDisplayMode(child, GroupDisplayMode.Small);
                    if (GetChildrenTotalHeight() < newSize.Height)
                        break;
                }
            else
                foreach (var child in Children.OfType<RibbonGroupBox>().Where(x => x.DisplayMode == GroupDisplayMode.Small))
                {
                    UpdateGroupBoxDisplayMode(child, GroupDisplayMode.Large);

                    var totalWidth = GetChildrenTotalHeight();
                    if (totalWidth <= newSize.Height)
                        break;

                    if (GetChildrenTotalHeight() > newSize.Height)
                    {
                        UpdateGroupBoxDisplayMode(child, GroupDisplayMode.Small);
                        break;
                    }
                }
        }
        else
        {
            if (GetChildrenTotalWidth() >= newSize.Width)
                foreach (var child in Children.Reverse().OfType<RibbonGroupBox>().Where(x => x.DisplayMode == GroupDisplayMode.Large))
                {
                    UpdateGroupBoxDisplayMode(child, GroupDisplayMode.Small);
                    if (GetChildrenTotalWidth() < newSize.Width)
                        break;
                }
            else
                foreach (var child in Children.OfType<RibbonGroupBox>().Where(x => x.DisplayMode == GroupDisplayMode.Small))
                {
                    UpdateGroupBoxDisplayMode(child, GroupDisplayMode.Large);

                    var totalWidth = GetChildrenTotalWidth();
                    if (totalWidth <= newSize.Width)
                        break;

                    if (GetChildrenTotalWidth() > newSize.Width)
                    {
                        UpdateGroupBoxDisplayMode(child, GroupDisplayMode.Small);
                        break;
                    }
                }
        }

        void UpdateGroupBoxDisplayMode(RibbonGroupBox child, GroupDisplayMode displayMode)
        {
            child.DisplayMode = displayMode;
            child.InvalidateMeasure();
            child.Measure(newSize);
        }
    }

    private double GetChildrenTotalWidth()
    {
        var desiredSize = Orientation == Orientation.Vertical
            ? DesiredSize.WithWidth(Bounds.Width)
            : DesiredSize.WithHeight(Bounds.Height);

        Arrange(new Rect(desiredSize));
        Measure(desiredSize);

        return Children.OfType<RibbonGroupBox>().Sum(groupBox => Math.Max(0, groupBox.Bounds.Width));
    }

    private double GetChildrenTotalHeight()
    {
        var children = Children.OfType<RibbonGroupBox>();
        double totalHeight = 0;

        Arrange(new Rect(DesiredSize));
        Measure(DesiredSize);
        
        for (var i = 0; i < children.Count(); i++)
        {
            var newSize = children.ElementAt(i).Bounds.Y - totalHeight;
            if (newSize <= 0)
                totalHeight += children.ElementAt(i).Bounds.Height + newSize;
        }
        
        return totalHeight;
    }
}