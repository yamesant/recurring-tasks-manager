namespace RTM.Core;

public class Task
{
    public Guid Id { get; protected set; }
    public string Name { get; protected set; }
    public int CompletionCount { get; protected set; }
    public DateTime TaskCreationDate { get; protected set; }
    public DateTime? LastCompletionDate { get; protected set; }
    public int IntervalTarget { get; protected set; }

    protected Task()
    {
        
    }
    public Task(string name, int intervalTarget)
    {
        Id = Guid.NewGuid();
        Name = name;
        CompletionCount = 0;
        TaskCreationDate = DateTime.UtcNow;
        LastCompletionDate = null;
        IntervalTarget = intervalTarget;
    }
    public Task(string name, int completionCount, DateTime taskCreationDate,
        DateTime? lastCompletionDate, int intervalTarget)
    {
        Id = Guid.NewGuid();
        Name = name;
        CompletionCount = completionCount;
        TaskCreationDate = taskCreationDate;
        LastCompletionDate = lastCompletionDate;
        IntervalTarget = intervalTarget;
    }
    public int DaysSinceCreation => (DateTime.UtcNow - TaskCreationDate).Days;
    public int DaysSinceLastCompletion => LastCompletionDate is null ? DaysSinceCreation : (DateTime.UtcNow - LastCompletionDate.Value).Days;
    public int LowerTarget => Math.Max(1, (int)(IntervalTarget * 0.9));
    public int UpperTarget => Math.Max(LowerTarget + 1, (int)(IntervalTarget * 1.1));
    public TaskSchedulingStatus SchedulingStatus
    {
        get
        {
            if (UpperTarget <= DaysSinceLastCompletion)
            {
                return TaskSchedulingStatus.Overdue;
            }

            if (LowerTarget <= DaysSinceLastCompletion)
            {
                return TaskSchedulingStatus.Ready;
            }

            return TaskSchedulingStatus.Scheduled;
        }
    }
    public void Complete()
    {
        CompletionCount++;
        IntervalTarget = (int)(((double)DaysSinceCreation + IntervalTarget) / (CompletionCount+1) + 0.5);
        LastCompletionDate = DateTime.Now;
    }
}