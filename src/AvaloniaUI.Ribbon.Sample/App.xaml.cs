using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaUI.Ribbon.Sample.Views;

namespace AvaloniaUI.Ribbon.Sample
{
    public class App : Application
    {
        internal static IClassicDesktopStyleApplicationLifetime Lifetime => Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime)
            {
                Lifetime.MainWindow = new MainWindow2();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
