using Avalonia.Controls.Templates;

namespace AvaloniaUI.Ribbon;

public interface ICanAddToQuickAccess
{
    IControlTemplate QuickAccessTemplate { get; set; }

    bool CanAddToQuickAccess { get; set; }
}