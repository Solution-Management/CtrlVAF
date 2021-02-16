using MFiles.VAF.Configuration;
using MFilesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Validators
{
    public abstract class CustomValidator<T>: ICustomValidator
    {
        public IEnumerable<ValidationFinding> Validate(Vault vault, object configuration)
        {
            if (configuration.GetType() != typeof(T))
                return new ValidationFinding[] { };

            else
                return Validate(vault, (T)configuration);

        }

        protected abstract IEnumerable<ValidationFinding> Validate(Vault vault, T configuration);
    }
}
