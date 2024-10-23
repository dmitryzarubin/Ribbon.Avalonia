using Avalonia.Controls.Templates;

namespace Ribbon.Avalonia;

public interface ICanAddToQuickAccess
{
    IControlTemplate QuickAccessTemplate { get; set; }

    bool CanAddToQuickAccess { get; set; }
}