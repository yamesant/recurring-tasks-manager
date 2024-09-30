using System;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using RTM.Core;

namespace RTM.UI.ViewModels;

public class AddTaskViewModel : ViewModelBase
{
    private string _name;
    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }
    private int _intervalTarget;
    public int IntervalTarget
    {
        get => _intervalTarget;
        set => this.RaiseAndSetIfChanged(ref _intervalTarget, value);
    }

    private IObservable<bool> CanAdd => Observable.CombineLatest(
        this.WhenAnyValue(vm => vm.Name).Select(name => !string.IsNullOrWhiteSpace(name)),
        this.WhenAnyValue(vm => vm.IntervalTarget).Select(interval => interval > 0),
        (isNameValid, isIntervalValid) => isNameValid && isIntervalValid);
    public ReactiveCommand<Unit, TaskViewModel> AddCommand { get; }
    public AddTaskViewModel()
    {
        _name = "";
        AddCommand = ReactiveCommand.Create(() => new TaskViewModel(new Task(Name, IntervalTarget)), CanAdd);
    }
}