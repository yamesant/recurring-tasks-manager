using RTM.Core;

namespace RTM.UI.ViewModels;

public class TaskViewModel(Task task) : ViewModelBase
{
    public string Name => task.Name;
}