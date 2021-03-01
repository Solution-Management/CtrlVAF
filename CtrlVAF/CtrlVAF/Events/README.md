# Events

## How to

To use the dispatcher, simply create a single event handler method in the VaultApplication class that 'handles' all events you wish to react to. Inside this EventHandler we create an **EventCommand** containing the **EventHandlerEnvironment** and dispatch that command with the **ConfigurableVaultApplicationBase.EventDispatcher**.

````c#
public partial class VaultApplication: CtrlVAF.Core.ConfigurableVaultApplicationBase<Configuration>
{
    [EventHandler(MFEventHandlerType.MFEventHandlerBeforeCreateNewObjectFinalize)]
    [EventHandler(MFEventHandlerType.MFEventHandlerBeforeCheckInChanges)]
    [EventHandler(MFEventHandlerType.MFEventHandlerAfterCheckInChanges)]
    public void MyEventHandler(EventHandlerEnvironment env)
    {
        var command = new EventCommand(env);
        
        EventDispatcher.Dispatch(command);
    }
}
````

The EventCommand contains the event that called it. The dispatcher will use this to search for EventHandler classes that are marked with the correct attribute and execute their **Handle**() method.

````c#
[EventCommandHandler(MFEventHandlerType.MFEventHandlerBeforeCheckInChanges)]
public class BeforeCheckInChanges: EventHandler<Configuration, EventCommand>
{
    public override void Handle(EventCommand command)
    {
        //
    }
}
````

The command exposes the EventHandlerEnvironment used to initialize it as **command.Env**.

Inside this EventHandler class you will have access to many properties you would also find in the VaultApplication class such as

- PermanentVault
- OnDemandBackgroundOperations
- RecurringBackgroundOperations

Furthermore you have access to the Configuration property. You can even control how much of the Configuration will be available to you. When working across separate projects, you might not have a reference to the top-level Configuration class but a reference to one of it's member classes. In this case you simply pass the member type as the first generic argument instead of the Configuration type. For example:

```c#
public class Configuration
{
    public string ParentName {get; set;}
    
    public ChildConfiguration Child {get; set;}
}

public class ChildConfiguration
{
    public string ChildName {get; set;}
}

[EventCommandHandler(MFEventHandlerType.MFEventHandlerBeforeCheckInChanges)]
public class BeforeCheckInChanges: EventHandler<ChildConfiguration, EventCommand>
{
    public override void Handle(EventCommand command)
    {
        if(command.Env.ObjVerEx.Title == Configuration.ChildName)
        {
            //
        }
    }
}
```

Lastly the EventHandler class also exposes the results for all the **CustomValidator**s which validate this type of configuration or any of it's members as **ValidationResults**. This collection class exposes some useful methods and is enumerable so you can easily check you validation results without having to recalculate them on every event. For more information see **Validation**.

## Custom Commands

You can also create a custom EventCommand so you can pass along any data not contained in the ones availabe through the EventHandler class. For example:

````c#
public class CustomCommand: EventCommand 
{
    public CustomCommand(EventHandlerEnvironment env): base(env)
    {
    }
    
    public EventLog EventLog {get; set;}
}
````

You can pass this command along instead of the standard EventCommand class when calling the dispatch method.

```c#
public partial class VaultApplication: CtrlVAF.Core.ConfigurableVaultApplicationBase<Configuration>
{
    [EventHandler(MFEventHandlerType.MFEventHandlerBeforeCreateNewObjectFinalize)]
    [EventHandler(MFEventHandlerType.MFEventHandlerBeforeCheckInChanges)]
    [EventHandler(MFEventHandlerType.MFEventHandlerAfterCheckInChanges)]
    public void MyEventHandler(EventHandlerEnvironment env)
    {
        var command = new CustomCommand(env);
        
        EventDispatcher.Dispatch(command);
    }
}
```

Lastly you will have to change the generic `TCommand` argument for the EventHandler class to make that class respond  to CustomCommand calls.

```c#
[EventCommandHandler(MFEventHandlerType.MFEventHandlerBeforeCheckInChanges)]
public class BeforeCheckInChanges: EventHandler<Configuration, CustomCommand>
{
    public override void Handle(CustomCommand command)
    {
        //
    }
}
```

You can make as many command types as you want and pass them all along in the dispatch call. Just be aware that handlers will only respond to the command type specified for `TCommand`.

```c#
public partial class VaultApplication: CtrlVAF.Core.ConfigurableVaultApplicationBase<Configuration>
{
    [EventHandler(MFEventHandlerType.MFEventHandlerBeforeCreateNewObjectFinalize)]
    [EventHandler(MFEventHandlerType.MFEventHandlerBeforeCheckInChanges)]
    [EventHandler(MFEventHandlerType.MFEventHandlerAfterCheckInChanges)]
    public void MyEventHandler(EventHandlerEnvironment env)
    {
        var command = new EventCommand(env);
        var customcommand = new CustomCommand(env);
        
        EventDispatcher.Dispatch(command, customcommand);
    }
}
```

```c#
[EventCommandHandler(MFEventHandlerType.MFEventHandlerBeforeCheckInChanges)]
public class BeforeCheckInChanges: EventHandler<Configuration, CustomCommand>
{
    public override void Handle(CustomCommand command)
    {
        //
    }
}

[EventCommandHandler(MFEventHandlerType.MFEventHandlerBeforeCheckInChanges)]
public class BeforeCheckInChanges: EventHandler<Configuration, EventCommand>
{
    public override void Handle(EventCommand command)
    {
        //
    }
}
```

