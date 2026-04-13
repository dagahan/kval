// Вариант 02
using Avalonia.Markup.Xaml;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using DEMO.Views;
using DEMO.ViewModels;

namespace DEMO;

public partial class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
        {
            base.OnFrameworkInitializationCompleted();
            return;
        }

        desktop.MainWindow = new MainWindow
        {
            DataContext = new MainWindowViewModel(),
        };

        base.OnFrameworkInitializationCompleted();
    }
}
