# Core

## ConfigurableVaultApplicationBase\<TSecureConfiguration>

To make things easier on the end user, we felt the need to implement a new **ConfigurableVaultApplicationBase** class from which the VaultApplication class can inherit. This class already takes care of some boilerplate code and exposes some new properties for use. Be careful when overriding methods mentioned here to call the base method at then end of the method body. Otherwise the VaultApplication may behave unexpectedly.

### Dispatchers

The three kinds of dispatchers are initialized in the **StartOperations**() method and exposed as the following properties:

- **EventDispatcher**
- ValidatorDispatcher
- **BackgroundDispatcher**

When using the [UseLicensing] attribute on your VaultApplication class, these dispatchers are automatically wrapped as Licensed dispatchers.

### Background Operations

In the **StartApplication**() method, the background dispatcher is called. This registers all BackgroundTaskHandlers and exposes them in the VaultApplication properties '**OnDemandBackgroundOperations**' and '**RecurringBackgroundOperations**'. For more information on how to interact with these, see [BackgroundOperations](https://github.com/Solution-Management/CtrlVAF/tree/main/CtrlVAF/CtrlVAF.Core/BackgroundOperations).

### Custom Validation

Lastly this base already calls the **CustomValidation**() method where the ValidatorDispatcher is called to handle the CustomValidator classes. The results from these handler classes are stored in **ValidationResults** where they can be accessed by configuration type. These results are cleared in the **OnConfigurationUpdated**() method.



