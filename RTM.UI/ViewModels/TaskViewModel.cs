using System;
using RTM.Core;

namespace RTM.UI.ViewModels;

public class TaskViewModel(Task task) : ViewModelBase
{
    public string Name => task.Name;
    public TaskSchedulingStatus SchedulingStatus => task.SchedulingStatus;
    public bool IsOverdue => SchedulingStatus == TaskSchedulingStatus.Overdue;
    public bool IsReady => SchedulingStatus == TaskSchedulingStatus.Ready;
    public bool IsScheduled => SchedulingStatus == TaskSchedulingStatus.Scheduled;
    public string CompletionInfo => $"Completion count: {task.CompletionCount}";
    public string IntervalInfo => $"Interval target: {task.IntervalTarget}";
    public string ReadyInfo => $"Days until ready: {Math.Max(0, task.LowerTarget - task.DaysSinceLastCompletion)}";
    public string OverdueInfo => $"Days until overdue: {Math.Max(0, task.UpperTarget - task.DaysSinceLastCompletion)}";
}