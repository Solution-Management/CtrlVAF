using CtrlVAF.BackgroundOperations;

using MFilesAPI;

namespace CtrlVAF.Core
{
    public interface ICommandHandler<TConfiguration>: ICommandHandler
        where TConfiguration: class, new()
    {
        TConfiguration Configuration { get; }
    }

    public interface ICommandHandler
    {
        Vault PermanentVault { get; }
        OnDemandBackgroundOperations OnDemandBackgroundOperations { get; } 
        RecurringBackgroundOperations RecurringBackgroundOperations { get; }
    }
}
