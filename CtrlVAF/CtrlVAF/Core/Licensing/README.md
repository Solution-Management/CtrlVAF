# Licensing

## Licensed Dispatcher

Licensing can also be integrated easily into any dispatcher. Simply wrap an existing dispatcher into a **LicensedDispatcher** like so:

```c#
Dispatcher<IEnumerable<ValidationFinding>> dispatcher = new ValidatorDispatcher(vault, config);

LicenseContentBase licenseContent = License?.Content<LicenseContentBase>();

dispatcher = new LicensedDispatcher<IEnumerable<ValidationFinding>>(dispatcher, licenseContent);

dispatcher.Dispatch();
```

Note that this is done for you when inheriting from our `ConfigurableVaultApplicationBase` class and using the [UseLicensing] attribute on your VaultApplication

The LicensedDispatcher class works in tandem with the **[LicenseRequired]** attribute. When using a licensed dispatcher, the dispatcher checks for a valid license. When the license is invalid, every type with the [LicenseRequired] attribute is skipped.

Furthermore, you can specify modules in the attribute that may be present on the license. For example:

```c#
[LicenseRequired(Modules = new string[] { "Basic" })]
class MyConfigurationValidator : CustomValidator<Configuration>
{
    //    
}
```

When a module is specified, the class will only be used if the license is valid, the license specifies modules AND the license contains one of the modules specified in the attribute. When the license specified modules, but the attribute does not, it is still loaded. In other words a [LicenseRequired] attribute that does not specify modules is treated as part of all modules.

