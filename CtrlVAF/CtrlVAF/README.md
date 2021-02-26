# CtrVAF

This package was created in order to move logic out of the standard VaultApplication class into seperate classes that can handle these tasks for the VaultApplication class.

The design is based on the dispatcher/command pattern. 

At this time there are three different dispatchers:

1. **Event** dispatcher that seeks out classes that react to M-Files events. These classes would replace the event handler functions on the VaultApplication class.
2. **Validation** dispatcher which seeks out classes that do custom validation on the configuration set in the admin tools. These classes would replace the logic happening inside the overrideable `CustomValidation()` method of the VaultApplication class
3. **Background** dispatcher which seeks out classes with a method that can be called as a background operation

Furthermore, every dispatcher can be decorated with a **Licensed** dispatcher to enable easy marking of license requirements on the handler classes.

All this minimizes the times the VaultApplication class needs to be reopened and edited. Perfect open/close on the VaultApplication class is not (yet) possible we think, but this is a step in that direction. 

For more information on how each dispatcher works and how to implement the handler classes, please take a look at the information in the relevant folder.