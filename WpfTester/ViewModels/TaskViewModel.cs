namespace WpfTester.ViewModels
{
  using System;
  using FluentScheduler;
  using Microsoft.Practices.Prism.Commands;
  using Microsoft.Practices.Prism.Mvvm;

  public class TaskViewModel : BindableBase
  {
    private readonly Task task;

    private DelegateCommand enableCommand;

    private DelegateCommand disableCommand;

    private DelegateCommand delayCommand;

    private DateTime lastRun;

    public TaskViewModel(Task task)
    {
      this.task = task;

      TaskManager.TaskEnd += TaskManager_TaskEnd;
    }

    void TaskManager_TaskEnd(FluentScheduler.Model.TaskEndScheduleInformation sender, EventArgs e)
    {
      if (sender.Name == this.task.Name)
      {
        this.LastRun = DateTime.Now;
        this.OnPropertyChanged(() => this.NextRun);
      }
    }

    public DelegateCommand EnableCommand
    {
      get
      {
        if (this.enableCommand == null)
        {
          this.enableCommand = new DelegateCommand(this.Enable, this.CanEnable);
        }
        return this.enableCommand;
      }
    }

    public DelegateCommand DisableCommand
    {
      get
      {
        if (this.disableCommand == null)
        {
          this.disableCommand = new DelegateCommand(this.Disable, this.CanDisable);
        }
        return this.disableCommand;
      }
    }

    public DelegateCommand DelayCommand
    {
      get
      {
        if (this.delayCommand == null)
        {
          this.delayCommand = new DelegateCommand(this.Delay, this.CanDelay);
        }
        return this.delayCommand;
      }
    }

    private void Enable()
    {
      Console.WriteLine("{0} {1}", this.task.Name, DateTime.Now);
      this.task.Schedule.Enable();
      this.InvalidateCommands();
    }

    private void InvalidateCommands()
    {
      this.delayCommand.RaiseCanExecuteChanged();
      this.disableCommand.RaiseCanExecuteChanged();
      this.enableCommand.RaiseCanExecuteChanged();
    }

    private bool CanEnable()
    {
      return this.task.Schedule.Disabled;
    }

    private void Disable()
    {
      this.task.Schedule.Disable();
      this.InvalidateCommands();
    }

    private bool CanDisable()
    {
      return !this.task.Schedule.Disabled;
    }

    private void Delay()
    {
      this.task.Schedule.NextRunTime = DateTime.Now.AddSeconds(5);
      this.InvalidateCommands();
      this.OnPropertyChanged(() => this.NextRun);
    }

    private bool CanDelay()
    {
      return true;
    }

    public DateTime LastRun
    {
      get { return this.lastRun; }
      set
      {
        this.lastRun = value;
        this.OnPropertyChanged(() => this.LastRun);
      }
    }

    public DateTime NextRun
    {
      get { return this.task.Schedule.NextRunTime; }
    }
  }
}
