using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using RTM.UI.ViewModels;

namespace RTM.UI.Views;

public partial class AddTaskWindow : ReactiveWindow<AddTaskViewModel>
{
    public AddTaskWindow()
    {
        InitializeComponent();
        this.WhenActivated(action => action(ViewModel!.AddCommand.Subscribe(Close)));
    }
}