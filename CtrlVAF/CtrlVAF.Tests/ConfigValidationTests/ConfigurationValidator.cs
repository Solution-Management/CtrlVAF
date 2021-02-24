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
    class ConfigurationValidator : CustomValidator<Configuration, ValidatorCommand>
    {
        public override IEnumerable<ValidationFinding> Validate(ValidatorCommand command)
        {
            if (string.IsNullOrWhiteSpace(Configuration.Name))
                yield return new ValidationFinding(
                    ValidationFindingType.Error,
                    "Name",
                    "No Value"
                    );

            if (Configuration.ID == -1)
                yield return new ValidationFinding(
                    ValidationFindingType.Error,
                    "ID",
                    "Not Found"
                    );
        }
    }
}
