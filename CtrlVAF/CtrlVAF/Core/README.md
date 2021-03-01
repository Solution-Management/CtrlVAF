# Core



## ConfigurableVaultApplicationBase\<TSecureConfiguration>

To make things easier on the end user, we felt the need to implement a new **ConfigurableVaultApplicationBase** class from which the VaultApplication class can inherit. This class already takes care of some boilerplate code and exposes some new properties for use. Be careful when overriding methods mentioned here to call the base method at then end of the method body. Otherwise the VaultApplication may behave unexpectedly.

### Dispatchers

The three kinds of dispatchers are initialized in the **StartOperations**() method and exposed as the following properties:

- **EventDispatcher**
- ValidatorDispatcher
- **BackgroundDispatcher**

When using the [UseLicensing] attribute on your VaultApplication class, these dispatchers are automatically wrapped as Licensed dispatchers.

```c#
//Contents of the StartOperations method
public override void StartOperations(Vault vaultPersistent)
{
    BackgroundDispatcher = new BackgroundDispatcher<TSecureConfiguration>(this);
    EventDispatcher = new EventDispatcher<TSecureConfiguration>(this);
    ValidatorDispatcher = new ValidatorDispatcher<TSecureConfiguration>(this);
    if (this.GetType().IsDefined(typeof(UseLicensingAttribute)))
    {
        var content = License?.Content<LicenseContentBase>();
        BackgroundDispatcher = new LicensedDispatcher(BackgroundDispatcher, content);
        EventDispatcher = new LicensedDispatcher(EventDispatcher, content);
        ValidatorDispatcher = new LicensedDispatcher<IEnumerable<ValidationFinding>>(ValidatorDispatcher, content);
    }
    base.StartOperations(vaultPersistent);
}
```



### Background Operations

In the **StartApplication**() method, the background dispatcher is called. This registers all BackgroundTaskHandlers and exposes them in the VaultApplication properties '**OnDemandBackgroundOperations**' and '**RecurringBackgroundOperations**'. For more information on how to interact with these, see [BackgroundOperations](https://github.com/Solution-Management/CtrlVAF/tree/main/CtrlVAF/CtrlVAF.Core/BackgroundOperations).

```c#
//Contents of the StartApplication method
protected override void StartApplication()
{
    TaskQueueBackgroundOperationManager = new TaskQueueBackgroundOperationManager(
        this,
        this.GetType().FullName.Replace(".", "-") + " - BackgroundOperations"
        );
    try
    {
        BackgroundDispatcher.Dispatch();
    }
    catch(Exception e)
    {
        SysUtils.ReportErrorMessageToEventLog("Could not dispatch the background operations.", e);
        return;
    }
    base.StartApplication();
}
```



### Custom Validation

Lastly this base already calls the **CustomValidation**() method where the ValidatorDispatcher is called to handle the CustomValidator classes. The results from these handler classes are stored in **ValidationResults** where they can be accessed by configuration type. These results are cleared in the **OnConfigurationUpdated**() method.

```c#
//Contents of the CustomValidation method
protected override IEnumerable<ValidationFinding> CustomValidation(Vault vault, TSecureConfiguration config)
{
    var command = new ValidationCommand(vault);
    var customCommand = AddCustomValidationCommand(vault);
    var findings =  ValidatorDispatcher.Dispatch(command, customCommand);
    if (findings == null)
        return base.CustomValidation(vault, config);
    return findings.Concat(base.CustomValidation(vault, config));
}
```

The AddCustomValidationCommand can be overridden to add your own custom ValidationCommand to the dispatch call.

```c#
//Contents of the OnConfigurationUpdated method
protected override void OnConfigurationUpdated(IConfigurationRequestContext context, 
                                               ClientOperations clientOps, 
                                               TSecureConfiguration oldConfiguration)
{
    ValidatorDispatcher.ClearCache();
    ValidationResults = new ConcurrentDictionary<Type, ValidationResults>();

    base.OnConfigurationUpdated(context, clientOps, oldConfiguration);
}
```



## Multiple Assemblies

Sometimes you will want to split your solution into multiple projects. In this case every project is compiled into it's own assembly, but the dispatchers will not know to look there. To alert the dispatchers of these assemblies you will have to override the **StartOperations**() method and call **IncludeAsemblies**() on each of them.

```c#
public override void StartOperations(Vault vaultPersistent)
{
    BackgroundDispatcher = new BackgroundDispatcher<TSecureConfiguration>(this);
    EventDispatcher = new EventDispatcher<TSecureConfiguration>(this);
    ValidatorDispatcher = new ValidatorDispatcher<TSecureConfiguration>(this);
    if (this.GetType().IsDefined(typeof(UseLicensingAttribute)))
    {
        var content = License?.Content<LicenseContentBase>();
        BackgroundDispatcher = new LicensedDispatcher(BackgroundDispatcher, content);
        EventDispatcher = new LicensedDispatcher(EventDispatcher, content);
        ValidatorDispatcher = new LicensedDispatcher<IEnumerable<ValidationFinding>>(ValidatorDispatcher, content);
    }
    //Added assemblies
    BackgroundDispatcher.IncludeAssemblies(typeof(CustomBackgroundTaskHandler1), typeof(CustomBackgroundTaskHandler2));
    EventDispatcher.IncludeAssemblies(typeof(BeforeCheckInChangesHandler));
    ValidationDispatcher.IncludeAssemblies(typeof(CustomValidator_1));
    
    base.StartOperations(vaultPersistent);
}
```





