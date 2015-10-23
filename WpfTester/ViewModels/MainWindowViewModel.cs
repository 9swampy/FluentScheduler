namespace WpfTester.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.ComponentModel;
  using FluentScheduler;
  using Microsoft.Practices.Prism.Commands;
  using Microsoft.Practices.Prism.Mvvm;

  public class MainViewModel : BindableBase
  {
    public event PropertyChangedEventHandler PropertyChanged;

    private DelegateCommand addCommand;

    private ObservableCollection<TaskViewModel> taskList;

    private string name;

    private int interval;

    private TimeInterval timeInterval;

    private int startHour;

    private int startMinute;

    private int endHour;

    private int endMinute;

    public MainViewModel()
    {
      TaskManager.Stop();
      this.TaskList.Add(new TaskViewModel(new WpfTester.Task("Runner1", 1, TimeInterval.Second)));
      this.TaskList.Add(new TaskViewModel(new WpfTester.Task("Runner2", 2, TimeInterval.Second)));
      TaskManager.Start();
    }

    public DelegateCommand DelayCommand
    {
      get
      {
        if (this.addCommand == null)
        {
          this.addCommand = new DelegateCommand(this.Add, this.CanAdd);
        }
        return this.addCommand;
      }
    }
    
    private void InvalidateCommands()
    {
      this.addCommand.RaiseCanExecuteChanged();
    }

    private void Add()
    {
      var task = new Task(this.Name, this.Interval, this.TimeInterval, this.StartHour, this.StartMinute, this.EndHour, this.EndMinute);
      this.TaskList.Add(new TaskViewModel(task));
      this.InvalidateCommands();
    }

    private bool CanAdd()
    {
      return true;
    }

    public ObservableCollection<TaskViewModel> TaskList
    {
      get
      {
        if (this.taskList == null)
        {
          this.taskList = new ObservableCollection<TaskViewModel>();
        }
        return this.taskList;
      }
    }

    public string Name { get; set; }

    public int Interval { get; set; }

    public TimeInterval TimeInterval { get; set; }

    public int StartHour { get; set; }

    public int StartMinute { get; set; }

    public int EndHour { get; set; }

    public int EndMinute { get; set; }
  }
}
