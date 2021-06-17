using CtrlVAF.Core;

namespace CtrlVAF.Events.Handlers
{
    public interface IEventHandler<TConfig, TCommand> : ICommandHandler<TConfig>
        where TConfig : class, new()
        where TCommand : EventCommand
    {
        void Handle(TCommand command);
    }
}