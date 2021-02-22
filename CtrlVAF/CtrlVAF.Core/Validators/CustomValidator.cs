using CtrlVAF.Models;

using MFiles.VAF.Configuration;

using MFilesAPI;

using System.Collections.Generic;

namespace CtrlVAF.Validators
{
    public abstract class CustomValidator<TCommand>: ICustomValidator where TCommand: class, new()
    {
        public IEnumerable<ValidationFinding> Validate(ICtrlVAFCommand command)
        {
            if (command.GetType() != typeof(TCommand))
                return new ValidationFinding[0];

            else return Validate(command as TCommand);
        }

        public abstract IEnumerable<ValidationFinding> Validate(TCommand command);
        
    }
}
