using CtrlVAF.BackgroundOperations;
using CtrlVAF.Core;
using CtrlVAF.Models;

using MFiles.VAF.Configuration;

using MFilesAPI;

using System.Collections.Generic;

namespace CtrlVAF.Validation
{
    public abstract class CustomValidator<TConfig, TCommand>: CustomValidator, ICustomValidator<TConfig, TCommand> 
        where TConfig: class, new()
        where TCommand: ValidationCommand
    {
        public TConfig Configuration { get; internal set; }

        public abstract IEnumerable<ValidationFinding> Validate(TCommand command);
    }

    public abstract class CustomValidator: ICommandHandler
    {
        public Vault PermanentVault { get; internal set; }
        public ValidationResults ValidationResults { get; internal set; }

        public OnDemandBackgroundOperations OnDemandBackgroundOperations { get; internal set; } 

        public RecurringBackgroundOperations RecurringBackgroundOperations { get; internal set; }
    }
}
