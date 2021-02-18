# Validators

## How to

To use this module in you VaultApplication, you need to override the **CustomValidation()** method of the VaultApplication class. Here you can create an instance of the **ValidatorDispatcher** class and call its **Dispatch()** method.

```c#
protected override IEnumerable<ValidationFinding> CustomValidation(Vault vault, Configuration config)
{
    Dispatcher<IEnumerable<ValidationFinding>> dispatcher = new ValidatorDispatcher(vault, config);
    
    IEnumerable<ValidationFinding> findings = dispatcher.Dispatch();
    
    //The Dispatch() method returns default in case no suitable types were found.
    if (findings == null)
    	return new ValidationFinding[0];
	return results;
}
```

This will find all types that have inherited from **CustomValidator\<TConfig>** and use them to validate the configuration.

You can declare your custom validator as such:

```c#
class MyConfigurationValidator : CustomValidator<Configuration>
{
	protected override IEnumerable<ValidationFinding> Validate(Vault vault, Configuration configuration)
    {
        yield return new ValidationFinding(...);
        
        //...
    }
}
```

### Child Configurations

The custom validator class can also be initialized with any class that can be found somewhere in the configuration tree. For example:

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



 

