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
    class Child_ConfigurationValidator : CustomValidator<Child_Configuration, ValidatorCommand>
    {
        public override IEnumerable<ValidationFinding> Validate(ValidatorCommand command)
        {
            if (Configuration == null)
                yield break;

            if (string.IsNullOrEmpty(Configuration.Name))
            {
                yield return new ValidationFinding(
                    ValidationFindingType.Error,
                    "Source",
                    "No Name"
                    );
            }
        }
    }
}
