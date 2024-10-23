using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace Ribbon.Avalonia;

public class GalleryItem : ListBoxItem
{
    public static readonly StyledProperty<IControlTemplate> IconProperty = RibbonButton.IconProperty.AddOwner<GalleryItem>();

    public IControlTemplate Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
}