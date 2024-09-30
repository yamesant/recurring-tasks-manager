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
}