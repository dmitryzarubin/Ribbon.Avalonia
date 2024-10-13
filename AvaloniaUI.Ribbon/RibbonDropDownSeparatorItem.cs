using System;
using Avalonia.Controls.Primitives;

namespace AvaloniaUI.Ribbon;

public class RibbonDropDownSeparatorItem : TemplatedControl
{
    protected override Type StyleKeyOverride { get; } = typeof(RibbonDropDownSeparatorItem);
}