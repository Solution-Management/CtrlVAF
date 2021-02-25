using CtrlVAF.Core;
using CtrlVAF.Models;

using MFiles.VAF.Configuration;

using MFilesAPI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Validators
{
    public interface ICustomValidator<TConfig, TCommand>: ICommandHandler<TConfig>
        where TConfig: class, new()
        where TCommand: ValidatorCommand
    {
        IEnumerable<ValidationFinding> Validate(TCommand command);
    }

    
}
