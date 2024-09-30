using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using RTM.Core;

namespace RTM.UI.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
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
        });
        RxApp.MainThreadScheduler.Schedule(LoadItems);
    }
    
    private async void LoadItems()
    {
        Tasks.Clear();

        List<Task> tasks = [
            new("Check up on friends or family",4,
                DateTime.Now.AddDays(-200), DateTime.Now.AddDays(-30), 40),
            new("Ensure email inbox is empty", 2,
                DateTime.Now.AddDays(-21), DateTime.Now.AddDays(-7), 7),
            new("Get dental checkup", 0,
                DateTime.Now, null, 180),
            new("Review finances", 2,
                DateTime.Now.AddDays(-100), DateTime.Now.AddDays(-20), 30),
            new("Go for a walk", 10,
                DateTime.Now.AddDays(-30), DateTime.Now.AddDays(-1), 3),
            new("Dedicate a day to reading a book", 2,
                DateTime.Now.AddDays(-60), DateTime.Now.AddDays(-15), 20),
            new("Enjoy a leisurely morning", 1,
                DateTime.Now.AddDays(-21), DateTime.Now.AddDays(-11), 10),
            new("Clean the house", 10,
                DateTime.Now.AddDays(-150), DateTime.Now.AddDays(-23), 15),
            new("Review career goals", 5,
                DateTime.Now.AddDays(-500), DateTime.Now.AddDays(-80), 90),
        ];

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