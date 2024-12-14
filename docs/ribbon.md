### Ribbon Component

The Ribbon consists of two main parts:

- **Menu**: Contains a set of dropdown-style elements that provide access to various parts of the application.
- **Tabs**: Represents different functional areas, each containing groups of commands and buttons.

In addition, the `Ribbon` control supports both **horizontal** and **vertical** orientations. This allows you to choose the layout that best fits your application's design. By default, the `Ribbon` is horizontally oriented, but you can switch to vertical by setting the `Orientation` property.

---

#### Horizontal Ribbon (default):

```xaml
<DockPanel>
    <!-- Menu declaration -->
    <ribbon:Ribbon DockPanel.Dock="Top" Orientation="Horizontal">
        <ribbon:Ribbon.Menu>
            <ribbon:RibbonMenu Icon="{DynamicResource MenuIcon}">
                <ribbon:RibbonMenuButtonItem>New</ribbon:RibbonMenuButtonItem>
                <ribbon:RibbonMenuButtonItem>Open</ribbon:RibbonMenuButtonItem>
                <ribbon:RibbonMenuButtonItem>Save</ribbon:RibbonMenuButtonItem>
                <ribbon:RibbonMenuButtonItem>Exit</ribbon:RibbonMenuButtonItem>
            </ribbon:RibbonMenu>
        </ribbon:Ribbon.Menu>
        
        <!-- Tabs declaration -->
        <ribbon:Ribbon.Tabs>
            <ribbon:RibbonTab Header="Home">
                <ribbon:RibbonTab.Groups>
                    <ribbon:RibbonGroupBox Header="Group 1">
                        <ribbon:RibbonButton Content="Button 1" />
                        <ribbon:RibbonButton Content="Button 2" />
                    </ribbon:RibbonGroupBox>
                    <ribbon:RibbonGroupBox Header="Group 2">
                        <ribbon:RibbonButton Content="Button 3" />
                    </ribbon:RibbonGroupBox>
                </ribbon:RibbonTab.Groups>
            </ribbon:RibbonTab>
        </ribbon:Ribbon.Tabs>
    </ribbon:Ribbon>
</DockPanel>
```
---
#### Vertical Ribbon:

```xaml
<ribbon:Ribbon Orientation="Vertical">
    <!-- Ribbon content here -->
</ribbon:Ribbon>
```