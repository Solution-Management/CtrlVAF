﻿using CtrlVAF.BackgroundOperations;
using CtrlVAF.Core;
using CtrlVAF.Validation;

using MFilesAPI;

namespace CtrlVAF.Events.Handlers
{
    public abstract class ConfigurationChangedHandler<TConfig, TCommand> : ConfigurationChangedHandler
        where TConfig : class, new()
        where TCommand : ConfigurationChangedCommand
    {
        public TConfig Configuration { get; internal set; }
        public TConfig OldConfiguration { get; internal set; }

        public abstract void Handle(TCommand command);
    }

    public abstract class ConfigurationChangedHandler : ICommandHandler
    {
        public Vault PermanentVault { get; internal set; }
        public ValidationResults ValidationResults { get; internal set; } = new ValidationResults();

        public OnDemandBackgroundOperations OnDemandBackgroundOperations { get; internal set; }

        public RecurringBackgroundOperations RecurringBackgroundOperations { get; internal set; }
    }
}