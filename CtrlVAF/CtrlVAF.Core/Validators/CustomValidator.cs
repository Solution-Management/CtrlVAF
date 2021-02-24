using CtrlVAF.Models;

using MFiles.VAF.Configuration;

using MFilesAPI;

using System.Collections.Generic;

namespace CtrlVAF.Validators
{
    public abstract class CustomValidator<TConfig, TCommand>: ICustomValidator<TConfig, TCommand> 
        where TConfig: class, new()
        where TCommand: ValidatorCommand
    {
        public TConfig Configuration { get; internal set; }

        public abstract IEnumerable<ValidationFinding> Validate(TCommand command);
        
    }
}
