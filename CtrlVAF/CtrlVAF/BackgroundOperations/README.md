# Background Operations

CtrlVAF allows VAF application engineers to automatically dispatch new background operations without any additional bootstrapping required. Both run-once and recurring background operations are supported.

## How To

To mark a class as a background task it needs to:

1. Inherit from **BackgroundTaskHandler<TConfig, TDirective>**, where `TConfig` can be the smallest subclass of the configuration necessary for the task to run and `TDirective` is the implemented `TaskQueueDirective` for this background operation.
2. Have the **[BackgroundOperation(Name)]** attribute where a unique name for the background operation is specified

```c#
[BackgroundOperation(Name: "CustomBackgroundOperation")]
class CustomBackgroundTask : BackgroundTaskHandler<Configuration, CustomTaskQueueDirective>
{
    public override void Task(TaskProcessorJob job, CustomTaskQueueDirective directive, Action<string, MFTaskState> progressFunction)
    {
        //Execute logic
    }
}

class CustomTaskQueueDirective : TaskQueueDirective 
{
    //Custom data
}
```

### Passing Configuration to the task

The `BackgroundTask` will expose a **Configuration** property of the type Configuration (in this example). This can be the `TSecureConfiguration` used in the `VaultApplication` declaration or the type of any member of it. For example:

```c#
public  class VaultApplication
        : CtrlVAF.BackgroundOperations.ConfigurableVaultApplicationBase<Configuration>
{
	//            
}

[DataContract]
public class Configuration
{
    [DataMember]
    public CustomConfiguration CustomConfig { get; set; }
    
    //
}

[DataContract]
public class CustomConfiguration 
{
    //
}
```

If you specify `CustomConfiguration` as the generic arguement for the BackgroundTaskHandler, the member **CustomConfig** will be accessible through **BackgroundTask.Configuration**.

### Registering to the VaultApplication

To load this background task into the **VaultApplication** it just needs to inherit from `CtrVAF.Core.ConfigurableVaultApplicationBase`. This class implements the **StartApplication()** function to load the classes specified following the above method.

When overriding this method be sure to keep the call to `base.StartApplication()`.

### Calling a background operation

To call the background operation on demand, the **ConfigurableVaultApplicationBase** exposes a property '**OnDemandBackgroundOperations**', where you can call a background operation by specifying it's name.

#### Run once

```c#
OnDemandBackgroundOperations.RunOnce(
    "CustomBackgroundOperation", 
    runAt: DateTime.Now.AddMinutes(10), 
    directive: new CustomTaskQueueDirective
    {
        // 
    }
);
```

#### Run at intervals

```c#
OnDemandBackgroundOperations.RunAtIntervals(
    "CustomBackgroundOperation", 
    runAt: DateTime.Now.AddMinutes(10), 
    directive: new CustomTaskQueueDirective
    {
        // 
    }
);

OnDemandBackgroundOperations.StopRunningAtIntervals("CustomTaskQueueDirective");
```

At the time of writing these calls are **not type-safe**, so make sure to use the correct `TaskQueueDirective` implementation.

### Recurring background operations

It's possible to have a recurring background operation. To mark a task as recurring, add the **Recurring** attribute to the class:

```c#
[Recurring(interval: 5, intervalKind: IntervalKind.Minutes)]
[BackgroundOperation(Name: "CustomBackgroundOperation")]
class CustomBackgroundTask : BackgroundTaskHandler<CustomConfiguration, CustomTaskQueueDirective>
{
    public void Task(TaskProcessorJob job, CustomTaskQueueDirective directive, Action<string, MFTaskState> progressFunction)
    {
        //Execute logic
    }
}

class CustomTaskQueueDirective : TaskQueueDirective 
{
    //Custom data
}
```

The interval and intervalKind in the example above for example schedules the recurring background operation to run ever 5 minutes. All recurring background tasks are stored in the **RecurringBackgroundOperations** property.

## Long-running background operations

For any background operations that run for longer than a minute, it is mandatory to have the operation report progress back to the VAF Background Operation Manager. This is to prevent the job from being launched multiple times, which may be detrimental to vault performance and may mess things up. The Task method exposes a progressFunction Action, which can be used to report progress:

```c#
[Recurring(interval: 5, intervalKind: IntervalKind.Minutes)]
[BackgroundOperation(Name: "CustomBackgroundOperation")]
class CustomBackgroundTask : BackgroundTaskHandler<CustomConfiguration, CustomTaskQueueDirective>
{
    public void Task(TaskProcessorJob job, CustomTaskQueueDirective directive, Action<string, MFTaskState> progressFunction)
    {
        progressFunction("Started", MFTaskState.MFTaskStateInProgress);
	someLongRunningFunction();
	progressFunction("Still going", MFTaskState.MFTaskStateInProgress);
	someOtherFunction();
	progressFunction("Finished!", MFTaskState.MFTaskStateCompleted);
    }
}
```

## Other

The **BackgroundOperations** properties also implement `IEnumerable<string>` and iterate over the task names specified in the attribute.

There is no option for custom commands for this dispatcher type. Instead, pass custom data through the TaskQueueDirective.
