using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using RTM.Core;
using RTM.UI.Data;

namespace RTM.UI.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly TaskContext _taskContext = new();
    public ObservableCollection<TaskViewModel> Tasks { get; } = [];
    private TaskViewModel? _selectedTask;
    public TaskViewModel? SelectedTask
    {
        get => _selectedTask;
        set => this.RaiseAndSetIfChanged(ref _selectedTask, value);
    }
    public ICommand CompleteTaskCommand { get; }
    public ICommand AddTaskCommand { get; }
    public Interaction<AddTaskViewModel, TaskViewModel?> ShowAddTask { get; } = new();
    public MainWindowViewModel()
    {
        CompleteTaskCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            if (SelectedTask is null)
            {
                return;
            }

            Task task = SelectedTask.GetTask();
            int index = Tasks.IndexOf(SelectedTask);
            SelectedTask = null;
            Tasks.RemoveAt(index);
            task.Complete();
            AddTask(new TaskViewModel(task));
            _taskContext.SaveChanges();
        });
        AddTaskCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            AddTaskViewModel addTaskViewModel = new();
            TaskViewModel? result = await ShowAddTask.Handle(addTaskViewModel);
            if (result is null)
            {
                return;
            }
            AddTask(result);
            _taskContext.Tasks.Add(result.GetTask());
            _taskContext.SaveChanges();
        });
        RxApp.MainThreadScheduler.Schedule(LoadItems);
    }
    
    private async void LoadItems()
    {
        Tasks.Clear();

        List<Task> tasks = await _taskContext.Tasks.ToListAsync();

        foreach (Task task in tasks)
        {
            AddTask(new TaskViewModel(task));
        }
    }
    
    private void AddTask(TaskViewModel task)
    {
        for (int i = 0; i < Tasks.Count; i++)
        {
            if (task.SchedulingStatus > Tasks[i].SchedulingStatus)
            {
                Tasks.Insert(i, task);
                return;
            }

            if (task.SchedulingStatus == Tasks[i].SchedulingStatus && task.DaysToTarget <= Tasks[i].DaysToTarget)
            {
                Tasks.Insert(i, task);
                return;
            }
        }

        Tasks.Add(task);
    }
}