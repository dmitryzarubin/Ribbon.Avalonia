namespace AvaloniaUI.Ribbon;

public interface IRibbonControl
{
    RibbonControlSize Size { get; set; }

    RibbonControlSize MinSize { get; set; }

    RibbonControlSize MaxSize { get; set; }
}