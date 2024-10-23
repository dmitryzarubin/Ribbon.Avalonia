using System.ComponentModel;

namespace Ribbon.Avalonia;

public interface IRibbonMenuItem : INotifyPropertyChanged
{
    RibbonMenuItemPlacement Placement { get; set; }
}