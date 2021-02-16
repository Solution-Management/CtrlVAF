using MFiles.VAF.Configuration;
using MFilesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Validators
{
    public interface ICustomValidator
    {
        IEnumerable<ValidationFinding> Validate(Vault vault, object configuration);
    }
}
