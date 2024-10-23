using System;
using Avalonia.Controls.Primitives;

namespace Ribbon.Avalonia;

public class RibbonDropDownSeparatorItem : TemplatedControl
{
    protected override Type StyleKeyOverride { get; } = typeof(RibbonDropDownSeparatorItem);
}