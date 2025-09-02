# ScheduleIt

Need a Job to be executed later?  
Then **Schedule It**!

Job Scheduler for .Net
  
## Setup
```csharp
TaskServer.Setup(c =>
{
    c.UseLogger(new TaskServerLogger(logger));
    // Create a own ActivationContainer
    c.UseActivationContainer(new TaskServerActivationContainer());
}).SetDefault();
```
  
## Create Tasks
```csharp
public class SomeTask : IBackgroundTask
{
    public void Execute(ExecutionContext context)
    {
        ...
    }
}
```
  
## Execute Tasks
```csharp
TaskServer.Instance.StartNew(new SomeTask());
```

## Schedule a Task/Action
```csharp
server.Schedule(() => { }, s => s.Now(), "Test");
```

## Schedule recurring Tasks
```csharp
TaskServer.Instance.Schedule(new SomeTask(), s => s.Every(60).Seconds());
```

```csharp
TaskServer.Instance.Schedule(new SomeTask(), s => s.Now().AndEvery(60).Seconds());
```


