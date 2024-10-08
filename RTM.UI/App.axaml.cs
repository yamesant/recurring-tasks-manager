using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using RTM.UI.Data;
using RTM.UI.ViewModels;
using RTM.UI.Views;

namespace RTM.UI;

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

        TaskContext context = new();
        context.Database.EnsureCreated();
        DbInitializer.Initialize(context);

        base.OnFrameworkInitializationCompleted();
    }
}