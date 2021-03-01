# Validators

## How to

To use this module in you VaultApplication, you need to override the **CustomValidation()** method of the VaultApplication class. Here you can use the **ValidationDispatcher** to dispatch a **ValidationCommand**.

```c#
protected override IEnumerable<ValidationFinding> CustomValidation(Vault vault, Configuration config)
{
    var command = new ValidationCommand(vault);
    
    IEnumerable<ValidationFinding> findings = dispatcher.Dispatch();
    
    //The Dispatch method can return null and this causes warnings in the admin tools.
    if (findings == null)
    	return base.CustomValidation(vault, config);
	return results;
}
```

This will find all types that have inherited from **CustomValidator\<TConfig, TCommand>** and use them to validate the configuration.

You can declare your custom validator as such:

```c#
class MyConfigurationValidator : CustomValidator<Configuration, ValidationCommand>
{
	protected override IEnumerable<ValidationFinding> Validate(ValidationCommand command)
    {
        yield return new ValidationFinding(
        	//
        );
        
        //...
    }
}
```

### Child Configurations

The CustomValidator class's TConfig argument can also be any class that can be found somewhere in the configuration tree. For example:

```c#
[DataContract]
public class Configuration {
    
    [DataMember]
    public ChildConfiguration ChildConfig {get; set;}
    
    //...
}

[DataContract]
public class ChildConfiguration {
    
    [DataMember]
    public string Name {get; set;}
    
    //...
}

public VaultApplication : ConfigurableVaultApplicationBase<Configuration>{
    //override CustomValidation
}

class MyChildConfigurationValidator : CustomValidator<ChildConfiguration>
{
	protected override IEnumerable<ValidationFinding> Validate(Vault vault, ChildConfiguration configuration)
    {
        if(string.IsNullOrWhiteSpace(configuration.Name))
        	yield return new ValidationFinding(
        		ValidationFindingType.Error,
            	"Source",
            	"Name was not set"
        	);
        
        //...
    }
}
```



## Validation Results

The results of the ValidatorDispatcher's Dispatch() calls are stored in a property '**ValidationResults**' exposed by the VaultApplication class in the form of a Dictionary\<Type, ValidationResults>. The keys for this dictionary are the configuration types that were validated. The ValidationResults class itself is a collection of enumerable ValidationFindings, but exposes some useful methods for common Linq queries. 

```c#
//Checks if any erorrs occured
ValidationResults.HasErrors();
    
//Gets all erorrs
ValidationResults.GetErrors();
    
//Checks if any warnings occured
ValidationResults.HasWarnings();
    
//Gets all warnings
ValidationResults.GetWarnings();
```

These ValidationResults are also exposed as a property on the **EventHandler** and **BackgroundTaskHandler** classes. 

```c#
[EventCommandHandler(MFEventHandlerType.MFEventBeforeCheckInChanges)]
public class BeforeCheckInChangesHandler : EventHandler<Configuration, EventCommand>
{
    public override void Handle(EventCommand command)
    {
        if(ValidationResults.HasErrors())
        {
            //
            //return;?
        }
    }
}
```

Furthermore the ValidationResults exposed here only contains the results for the keys which are either the TConfig argument itself (Configuration in this case) or any of it's member types. So for this example we would get the results for any validators of Configuration but also those of ChildConfiguration.