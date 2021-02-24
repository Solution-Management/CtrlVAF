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
    class ConfigurationValidator : CustomValidator<ValidatorCommand<Configuration>>
    {
        public override IEnumerable<ValidationFinding> Validate(ValidatorCommand<Configuration> command)
        {
            var configuration = command.Configuration;

            if (string.IsNullOrWhiteSpace(configuration.Name))
                yield return new ValidationFinding(
                    ValidationFindingType.Error,
                    "Name",
                    "No Value"
                    );

            if (configuration.ID == -1)
                yield return new ValidationFinding(
                    ValidationFindingType.Error,
                    "ID",
                    "Not Found"
                    );
        }
    }
}
