## Overview

The `Ribbon` component for Avalonia is a UI control designed to provide users with a familiar tab-based navigation experience, similar to the Ribbon found in popular applications like Microsoft Word or Excel. It is composed of **tabs**, **groups**, and **buttons** to organize various commands and actions in a clear, hierarchical structure.

### Active Development

This version of the `Ribbon.Avalonia` is actively under development, and its public API is subject to change. Developers using this component should be aware that future updates may introduce breaking changes or improvements as the library evolves.

The project is based on an earlier version created by **Splitwirez**, whose work can be found in the [AvaloniaRibbon repository](https://github.com/Splitwirez/AvaloniaRibbon). Splitwirez's version was initially built upon the work of **Alban Mazerolles**, whose repository can be found [here](https://github.com/AlbanMazerolles/AvaloniaRibbon).

### Key Features:
- **Tabs**: Each tab groups related functionalities, providing easy access to different sections of an application.
- **Groups**: Within each tab, actions are further organized into groups to categorize buttons and controls logically.
- **Buttons**: Each group contains buttons that trigger specific actions, which can be commands, toggles, or dropdown menus.
- **Customizable**: The Ribbon component is highly customizable, allowing developers to define tabs, groups, and buttons based on their application's needs.
- **Flexible Layout**: The component adapts to different window sizes, ensuring that the most important controls are always accessible.
- **Theme Support**: The Ribbon supports both **light** and **dark** themes, allowing the application to seamlessly adapt to different user preferences and environments.
- **Style Options**: The Ribbon can be styled using two predefined styles: **Default** and **Fluent**, giving developers flexibility to choose between a classic or more modern look.


### License

The AvaloniaRibbon library is licensed under the **MIT** License, allowing developers to freely use, modify, and distribute the software.

---

## Installation

To use the `Ribbon.Avalonia` in your Avalonia application, follow these steps:

1. Install the `Ribbon.Avalonia` package via NuGet:

[![Nuget](https://img.shields.io/nuget/v/Ribbon.Avalonia.svg?style=flat-square)](https://www.nuget.org/packages/Ribbon.Avalonia)
```bash
dotnet add package MyRibbonAvalonia
```
2. Add the appropriate styles and resources to your application's XAML depending on the theme you're using.

For Fluent Theme:
```xaml
<StyleInclude Source="avares://Ribbon.Avalonia/Styles/Fluent/Ribbon.Avalonia.xaml" />
```

For Default Theme:
```xaml
<StyleInclude Source="avares://Ribbon.Avalonia/Styles/Default/Ribbon.Avalonia.xaml" />
```

3. Include localized text resources (this applies to both themes):
```xaml
<ResourceInclude Source="avares://Ribbon.Avalonia/Locale/en-ca.xaml" />
```

After completing these steps, the Ribbon control will be integrated into your application, and you'll be able to use it with the selected theme and localization support.







![Fluent-Light theme preview, horizontal orientation](/ReadmeImages/Ribbon-FluentLight-Horizontal.png)
![Fluent-Dark theme preview, horizontal orientation](/ReadmeImages/Ribbon-FluentDark-Horizontal.png)

![Fluent-Light theme preview, vertical orientation](/ReadmeImages/Ribbon-FluentLight-Vertical.png)
![Fluent-Dark theme preview, vertical orientation](/ReadmeImages/Ribbon-FluentDark-Vertical.png)
