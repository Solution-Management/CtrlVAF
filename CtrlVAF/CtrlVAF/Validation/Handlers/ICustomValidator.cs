using CtrlVAF.Core;

using MFiles.VAF.Configuration;
using System.Collections.Generic;

namespace CtrlVAF.Validation
{
    public interface ICustomValidator<TConfig, TCommand> : ICommandHandler<TConfig>
        where TConfig : class, new()
        where TCommand : ValidationCommand
    {
        IEnumerable<ValidationFinding> Validate(TCommand command);
    }
}