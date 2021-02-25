using CtrlVAF.Models;

using MFilesAPI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Validation
{
    public class ValidationCommand: ICtrlVAFCommand
    {
        public ValidationCommand(Vault vault)
        {
            Vault = vault;
        }

        public Vault Vault { get; }

    }

}
