﻿using Avalonia.Collections;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;
using System;
using System.Linq;
using System.Collections;

namespace Avalonia.Controls.Ribbon
{
    public class Ribbon : TabControl, IStyleable
    {
        

        public Orientation Orientation
        {
            get { return GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly StyledProperty<Orientation> OrientationProperty;
        public static readonly StyledProperty<IBrush> HeaderBackgroundProperty;
        public static readonly StyledProperty<IBrush> HeaderForegroundProperty;
        public static readonly StyledProperty<bool> IsCollapsedProperty;
        public static readonly StyledProperty<RibbonMenu> MenuProperty = AvaloniaProperty.Register<Ribbon, RibbonMenu>(nameof(Menu));
        public static readonly StyledProperty<bool> IsMenuOpenProperty;
        public static readonly DirectProperty<Ribbon, IEnumerable> SelectedGroupsProperty = AvaloniaProperty.RegisterDirect<Ribbon, IEnumerable>(nameof(SelectedGroups), o => o.SelectedGroups, (o, v) => o.SelectedGroups = v);

        static Ribbon()
        {
            OrientationProperty = StackLayout.OrientationProperty.AddOwner<Ribbon>();
            OrientationProperty.OverrideDefaultValue<Ribbon>(Orientation.Horizontal);
            HeaderBackgroundProperty = AvaloniaProperty.Register<Ribbon, IBrush>(nameof(HeaderBackground));
            HeaderForegroundProperty = AvaloniaProperty.Register<Ribbon, IBrush>(nameof(HeaderForeground));
            IsCollapsedProperty = AvaloniaProperty.Register<Ribbon, bool>(nameof(IsCollapsed));
            IsMenuOpenProperty = AvaloniaProperty.Register<Ribbon, bool>(nameof(IsMenuOpen));

            SelectedIndexProperty.Changed.AddClassHandler<Ribbon>((x, e) =>
            {
                if (((int)e.NewValue >= 0) && (x.SelectedItem != null) && (x.SelectedItem is RibbonTab tab))
                    x.SelectedGroups = tab.Groups;
                else
                    x.SelectedGroups = new AvaloniaList<object>();
            });
        }

        private IEnumerable _selectedGroups = new AvaloniaList<object>();
        public IEnumerable SelectedGroups
        {
            get { return _selectedGroups; }
            set { SetAndRaise(SelectedGroupsProperty, ref _selectedGroups, value); }
        }

        Type IStyleable.StyleKey => typeof(Ribbon);

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

        public IBrush HeaderBackground
        {
            get { return GetValue(HeaderBackgroundProperty); }
            set { SetValue(HeaderBackgroundProperty, value); }
        }

        public IBrush HeaderForeground
        {
            get { return GetValue(HeaderForegroundProperty); }
            set { SetValue(HeaderForegroundProperty, value); }
        }

        protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
        {
            int oldIndex = SelectedIndex;
            int newIndex = SelectedIndex;
            if (ItemCount > 1)
            {
                if (((Orientation == Orientation.Horizontal) && (e.Delta.Y > 0)) || ((Orientation == Orientation.Vertical) && (e.Delta.Y < 0)))
                {
                    if (newIndex > 0)
                        newIndex--;
                }
                else if (((Orientation == Orientation.Horizontal) && (e.Delta.Y < 0)) || ((Orientation == Orientation.Vertical) && (e.Delta.Y > 0)))
                {
                    if (newIndex < (ItemCount - 1))
                        newIndex++;
                }
            }
            SelectedIndex = newIndex;
            if ((SelectedItem is RibbonTab tab) && (!tab.IsEnabled))
                SelectedIndex = oldIndex;
            base.OnPointerWheelChanged(e);
        }
    }
}
