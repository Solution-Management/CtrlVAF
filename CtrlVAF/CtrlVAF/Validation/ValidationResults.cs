using MFiles.VAF.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CtrlVAF.Validation
{
    public class ValidationResults : IEnumerable<ValidationFinding>
    {
        private IEnumerable<ValidationFinding> validationFindings;

        public ValidationResults()
        {
            validationFindings = new ValidationFinding[0];
        }

        public ValidationResults(IEnumerable<ValidationFinding> validationFindings)
        {
            this.validationFindings = validationFindings;
        }

        public ValidationResults AddResults(IEnumerable<ValidationFinding> findings)
        {
            validationFindings = validationFindings.Concat(findings).Distinct();
            return this;
        }

        public bool HasErrors()
        {
            return validationFindings.Any(finding => finding.Type == ValidationFindingType.Error);
        }

        public IEnumerable<ValidationFinding> GetErrors()
        {
            return validationFindings.Where(finding => finding.Type == ValidationFindingType.Error);
        }

        public bool HasWarnings()
        {
            return validationFindings.Any(finding => finding.Type == ValidationFindingType.Warning);
        }

        public IEnumerable<ValidationFinding> GetWarnings()
        {
            return validationFindings.Where(finding => finding.Type == ValidationFindingType.Warning);
        }

        public IEnumerator<ValidationFinding> GetEnumerator()
        {
            return validationFindings.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}