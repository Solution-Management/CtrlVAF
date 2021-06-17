using CtrlVAF.BackgroundOperations;
using CtrlVAF.Core;
using CtrlVAF.Validation;

using MFilesAPI;

namespace CtrlVAF.Events.Handlers
{
    public abstract class EventHandler<TConfig, TCommand> : EventHandler, IEventHandler<TConfig, TCommand>
        where TConfig : class, new()
        where TCommand : EventCommand
    {
        public TConfig Configuration { get; internal set; }

        public abstract void Handle(TCommand command);
    }

    public abstract class EventHandler : ICommandHandler
    {
        public Vault PermanentVault { get; internal set; }
        public ValidationResults ValidationResults { get; internal set; } = new ValidationResults();

        public OnDemandBackgroundOperations OnDemandBackgroundOperations { get; internal set; }

        public RecurringBackgroundOperations RecurringBackgroundOperations { get; internal set; }
    }
}