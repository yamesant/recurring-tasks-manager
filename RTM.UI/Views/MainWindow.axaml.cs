using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using RTM.UI.ViewModels;

namespace RTM.UI.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
        this.WhenActivated(action => 
            action(ViewModel!.ShowAddTask.RegisterHandler(DoShowAddTaskAsync)));
    }
    
    private async Task DoShowAddTaskAsync(InteractionContext<AddTaskViewModel, TaskViewModel?> interaction)
    {
        AddTaskWindow dialog = new();
        dialog.DataContext = interaction.Input;

        TaskViewModel? result = await dialog.ShowDialog<TaskViewModel?>(this);
        interaction.SetOutput(result);
    }
}