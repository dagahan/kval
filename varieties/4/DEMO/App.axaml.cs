// Вариант 04
using DEMO.ViewModels;
using DEMO.Views;
using Avalonia.Markup.Xaml;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;

namespace DEMO;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
