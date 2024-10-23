using Ribbon.Avalonia.Enums;

namespace Ribbon.Avalonia;

public interface IRibbonControl
{
    RibbonControlSize Size { get; set; }

    RibbonControlSize MinSize { get; set; }

    RibbonControlSize MaxSize { get; set; }
}