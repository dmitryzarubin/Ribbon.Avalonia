using System.ComponentModel;

namespace AvaloniaUI.Ribbon;

public interface IRibbonMenuItem : INotifyPropertyChanged
{
    RibbonMenuItemPlacement Placement { get; set; }
}