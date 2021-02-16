using CtrlVAF.Validators;

using MFiles.VAF.Configuration;

using MFilesAPI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Tests.ConfigValidationTests
{
    class Child_ConfigurationValidator: CustomValidator<Child_Configuration>
    {
        protected override IEnumerable<ValidationFinding> Validate(Vault vault, Child_Configuration configuration)
        {
            if (string.IsNullOrWhiteSpace(configuration.Name))
                yield return new ValidationFinding(
                    ValidationFindingType.Error,
                    "Name",
                    "No Value"
                    );
        }
    }
}
