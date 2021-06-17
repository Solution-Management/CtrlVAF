using CtrlVAF.Models;

using MFilesAPI;

namespace CtrlVAF.Validation
{
    public class ValidationCommand : ICtrlVAFCommand
    {
        public ValidationCommand(Vault vault)
        {
            Vault = vault;
        }

        public Vault Vault { get; }
    }
}