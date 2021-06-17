using CtrlVAF.BackgroundOperations;
using CtrlVAF.Validation;

using MFilesAPI;

namespace CtrlVAF.Core
{
    public interface ICommandHandler<TConfiguration> : ICommandHandler
        where TConfiguration : class, new()
    {
        TConfiguration Configuration { get; }
    }

    public interface ICommandHandler
    {
        Vault PermanentVault { get; }
        ValidationResults ValidationResults { get; }
        OnDemandBackgroundOperations OnDemandBackgroundOperations { get; }
        RecurringBackgroundOperations RecurringBackgroundOperations { get; }
    }
}