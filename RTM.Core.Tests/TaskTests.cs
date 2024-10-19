using Microsoft.Extensions.Time.Testing;

namespace RTM.Core.Tests;

public class TaskTests
{
    private readonly FakeTimeProvider _timeProvider = new();

    [SetUp]
    public void SetUp()
    {
        Task.TimeProvider = _timeProvider;
    }

    [Test]
    [InlineAutoData(new int[] { })]
    [InlineAutoData(new[] { 20 })]
    [InlineAutoData(new[] { 3, 4, 3 })]
    public void WhenTimePasses_CreationDateDoesNotChange(int[] daysBetweenCompletions, string taskName, int intervalTarget)
    {
        // Arrange
        DateOnly creationDate = DateOnly.FromDateTime(_timeProvider.GetUtcNow().Date);
        Task task = new(taskName, intervalTarget);
        foreach (int daysPassed in daysBetweenCompletions)
        {
            _timeProvider.Advance(TimeSpan.FromDays(daysPassed));
            task.Complete();
        }
        
        // Act
        DateOnly actual = task.TaskCreationDate;
        
        // Assert
        actual.Should().Be(creationDate);
    }
    
    [Test]
    [InlineAutoData(new int[] { }, 0)]
    [InlineAutoData(new[] { 20 }, 20)]
    [InlineAutoData(new[] { 8, 5, 3 }, 16)]
    public void WhenTimePasses_DaysSinceCreationUpdates(int[] daysBetweenCompletions, int daysSinceCreation, string taskName, int intervalTarget)
    {
        // Arrange
        Task task = new(taskName, intervalTarget);
        foreach (int daysPassed in daysBetweenCompletions)
        {
            _timeProvider.Advance(TimeSpan.FromDays(daysPassed));
            task.Complete();
        }
        
        // Act
        int actual = task.DaysSinceCreation;
        
        // Assert
        actual.Should().Be(daysSinceCreation);
    }
    
    [Test]
    [InlineAutoData(new int[] { }, 0)]
    [InlineAutoData(new int[] { }, 1)]
    [InlineAutoData(new[] { 14 }, 15)]
    [InlineAutoData(new[] { 1, 1, 2 }, 2)]
    public void WhenTimePasses_DaysSinceLastCompletionUpdates(int[] daysBetweenCompletions, int daysSinceLastCompletion, string taskName, int intervalTarget)
    {
        // Arrange
        Task task = new(taskName, intervalTarget);
        foreach (int daysPassed in daysBetweenCompletions)
        {
            _timeProvider.Advance(TimeSpan.FromDays(daysPassed));
            task.Complete();
        }
        _timeProvider.Advance(TimeSpan.FromDays(daysSinceLastCompletion));
        
        // Act
        int actual = task.DaysSinceLastCompletion;
        
        // Assert
        actual.Should().Be(daysSinceLastCompletion);
    }
    
    [Test]
    [InlineAutoData(new int[] { })]
    [InlineAutoData(new[] { 2 })]
    [InlineAutoData(new[] { 12, 9, 11 })]
    public void WhenCompleted_CompletionCountIncrements(int[] daysBetweenCompletions, string taskName, int intervalTarget)
    {
        // Arrange
        Task task = new(taskName, intervalTarget);
        int numberOfCompletions = daysBetweenCompletions.Length;
        foreach (int daysPassed in daysBetweenCompletions)
        {
            _timeProvider.Advance(TimeSpan.FromDays(daysPassed));
            task.Complete();
        }
        
        // Act
        int actual = task.CompletionCount;
        
        // Assert
        actual.Should().Be(numberOfCompletions);
    }
    
    [Test]
    [InlineAutoData(new int[] { })]
    [InlineAutoData(new[] { 2 })]
    [InlineAutoData(new[] { 12, 9, 11 })]
    public void WhenCompleted_DaysSinceLastCompletionResets(int[] daysBetweenCompletions, string taskName, int intervalTarget)
    {
        // Arrange
        Task task = new(taskName, intervalTarget);
        foreach (int daysPassed in daysBetweenCompletions)
        {
            _timeProvider.Advance(TimeSpan.FromDays(daysPassed));
            task.Complete();
        }
        
        // Act
        int actual = task.DaysSinceLastCompletion;
        
        // Assert
        actual.Should().Be(0);
    }
    
    [Test]
    [InlineAutoData(new[] { 5 }, 10)]
    [InlineAutoData(new[] { 10, 5 }, 10)]
    public void WhenCompletedVeryEarly_IntervalTargetDecreases(int[] daysBetweenCompletions, int intervalTarget, string taskName)
    {
        // Arrange
        Task task = new(taskName, intervalTarget);
        foreach (int daysPassed in daysBetweenCompletions)
        {
            _timeProvider.Advance(TimeSpan.FromDays(daysPassed));
            task.Complete();
        }
        
        // Act
        int actual = task.IntervalTarget;
        
        // Assert
        actual.Should().BeLessThan(intervalTarget);
    }
    
    [Test]
    [InlineAutoData(new[] { 20 }, 10)]
    [InlineAutoData(new[] { 10, 20 }, 10)]
    public void WhenCompletedVeryLate_IntervalTargetIncreases(int[] daysBetweenCompletions, int intervalTarget, string taskName)
    {
        // Arrange
        Task task = new(taskName, intervalTarget);
        foreach (int daysPassed in daysBetweenCompletions)
        {
            _timeProvider.Advance(TimeSpan.FromDays(daysPassed));
            task.Complete();
        }
        
        // Act
        int actual = task.IntervalTarget;
        
        // Assert
        actual.Should().BeGreaterThan(intervalTarget);
    }
    
    [Test]
    [InlineAutoData(new[] { 9 }, 10)]
    [InlineAutoData(new[] { 10, 9 }, 10)]
    public void WhenCompletedEarly_IntervalTargetDoesNotIncrease(int[] daysBetweenCompletions, int intervalTarget, string taskName)
    {
        // Arrange
        Task task = new(taskName, intervalTarget);
        foreach (int daysPassed in daysBetweenCompletions)
        {
            _timeProvider.Advance(TimeSpan.FromDays(daysPassed));
            task.Complete();
        }
        
        // Act
        int actual = task.IntervalTarget;
        
        // Assert
        actual.Should().BeLessThanOrEqualTo(intervalTarget);
    }
    
    [Test]
    [InlineAutoData(new[] { 11 }, 10)]
    [InlineAutoData(new[] { 10, 11 }, 10)]
    public void WhenCompletedLate_IntervalTargetDoesNotDecrease(int[] daysBetweenCompletions, int intervalTarget, string taskName)
    {
        // Arrange
        Task task = new(taskName, intervalTarget);
        foreach (int daysPassed in daysBetweenCompletions)
        {
            _timeProvider.Advance(TimeSpan.FromDays(daysPassed));
            task.Complete();
        }
        
        // Act
        int actual = task.IntervalTarget;
        
        // Assert
        actual.Should().BeGreaterThanOrEqualTo(intervalTarget);
    }
    
    [Test]
    [InlineAutoData(new int[] { }, 10)]
    [InlineAutoData(new[] { 10 }, 10)]
    [InlineAutoData(new[] { 10, 10 }, 10)]
    public void WhenCompletedOnTime_IntervalTargetDoesNotChange(int[] daysBetweenCompletions, int intervalTarget, string taskName)
    {
        // Arrange
        Task task = new(taskName, intervalTarget);
        foreach (int daysPassed in daysBetweenCompletions)
        {
            _timeProvider.Advance(TimeSpan.FromDays(daysPassed));
            task.Complete();
        }
        
        // Act
        int actual = task.IntervalTarget;
        
        // Assert
        actual.Should().Be(intervalTarget);
    }
    
    [Test]
    [InlineAutoData(new[] { 8, 6 }, 7, 5, TaskSchedulingStatus.Scheduled)]
    [InlineAutoData(new[] { 8, 6 }, 7, 6, TaskSchedulingStatus.Ready)]
    [InlineAutoData(new[] { 8, 6 }, 7, 7, TaskSchedulingStatus.Overdue)]
    public void TaskSchedulingStatusIsCorrect(int[] daysBetweenCompletions, int intervalTarget, int daysSinceLastCompletion,
        TaskSchedulingStatus expectedStatus, string taskName)
    {
        // Arrange
        Task task = new(taskName, intervalTarget);
        foreach (int daysPassed in daysBetweenCompletions)
        {
            _timeProvider.Advance(TimeSpan.FromDays(daysPassed));
            task.Complete();
        }

        _timeProvider.Advance(TimeSpan.FromDays(daysSinceLastCompletion));

        // Act
        TaskSchedulingStatus actual = task.SchedulingStatus;

        // Assert
        actual.Should().Be(expectedStatus);
    }
}