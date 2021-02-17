# Background Operations

## Motivation

Adding a separate property to the **VaultApplication** class for every background operation seemed hard to manage. So inspired by [**CtrlVAF.Commands**](https://github.com/Solution-Management/CtrlVAF/tree/main/CtrlVAF/CtrlVAF.Commands), this project uses reflection to track down suitable classes where it can extract a method to be called as a background operation.

This project depends on MFiles.VAF but also MFiles.VAF.Extensions for task queue background operation management.

## How To

To mark a class as a background task it needs to 

1. Inherit from **BackgroundTask<TConfig, TDirective>**, where `TConfig` can be the smallest subclass of the configuration necessary for the task to run and `TDirective` is the implemented `TaskQueueDirective` for this background operation.
2. The **[BackgroundOperation(Name)]** attribute where a unique name for the background operation is specified

```c#
[BackgroundOperation(Name: "CustomBackgroundOperation")]
class CustomBackgroundTask : BackgroundTask<CustomConfiguration, CustomTaskQueueDirective>
{
    public override void Task(TaskProcessorJob job, CustomTaskQueueDirective directive)
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

The `BackgroundTask` will expose a **Configuration** property of the type CustomConfiguration. This can be the `TSecureConfiguration` used in the `VaultApplication` declaration or the type of any member of it. For example:

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

If you specify `CustomConfiguration` the member **CustomConfig** will be accessible through **BackgroundTask.Configuration**.

### Registering to the VaultApplication

To load this background task into the **VaultApplication** it just needs to inherit from `CtrVAF.BackgroundOperations.ConfigurableVaultApplicationBase`. This class implements the **StartApplication()** function to load the classes specified following the above method.

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

It's possible to have a recurring background operation. To mark a task as recurring, add the **Recurring** attribute

```c#
[Recurring(IntervalInMinutes: 10)]
[BackgroundOperation(Name: "CustomBackgroundOperation")]
class CustomBackgroundTask : BackgroundTask<CustomConfiguration, CustomTaskQueueDirective>
{
    public void Task(TaskProcessorJob job, CustomTaskQueueDirective directive)
    {
        //Execute logic
    }
}

class CustomTaskQueueDirective : TaskQueueDirective 
{
    //Custom data
}
```

All recurring background tasks are stored in the **RecurringBackgroundOperations** property.

## Other

The **BackgroundOperations** properties also implement `IEnumerable<string>` and iterate over the task names specified in the attribute.

At the time of writing all background tasks need to be implemented in the same assembly as the **VaultApplication** class.