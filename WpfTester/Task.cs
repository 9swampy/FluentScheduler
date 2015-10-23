namespace WpfTester
{
  using System;
  using FluentScheduler;
  using FluentScheduler.Model;
  using FluentScheduler.Extensions;

  public class Task
  {
    private readonly int interval;
    private readonly string name;

    public Task(string name, int interval, TimeInterval timeInterval)
      : this(name, interval, timeInterval, 0, 0, 23, 59)
    { }

    public Task(string name, int interval, TimeInterval timeInterval, int startHour, int startMinute, int endHour, int endMinute)
    {
      this.name = name;
      this.interval = interval;

      TaskManager.Stop();
      TaskManager.AddTask(this.Execute, x =>
      {
        TimeUnit repeatingUnit = x.WithName(this.name).ToRunEvery(this.interval);
        ITimeRestrictableUnit restrictableUnit;
        switch (timeInterval)
        {
          case TimeInterval.Hour:
            restrictableUnit = repeatingUnit.Hours();
            break;
          case TimeInterval.Minute:
            restrictableUnit = repeatingUnit.Minutes();
            break;
          case TimeInterval.Second:
            restrictableUnit = repeatingUnit.Seconds();
            break;
          default:
            throw new InvalidOperationException();
        }
        restrictableUnit.Between(startHour, startMinute, endHour, endMinute);
      });
      TaskManager.Start();

      this.Schedule.Disable();
    }

    public Schedule Schedule
    {
      get
      {
        return TaskManager.GetSchedule(this.name);
      }
    }

    public string Name
    {
      get
      {
        return this.name;
      }
    }

    public int SecondsDelay
    {
      get
      {
        return this.interval;
      }
    }

    public void Execute()
    {
      Console.WriteLine("{0} {1}", this.Name, DateTime.Now);
    }
  }
}