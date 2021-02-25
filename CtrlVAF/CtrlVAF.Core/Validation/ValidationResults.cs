using MFiles.VAF.Configuration;

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Validation
{
    public class ValidationResults: IEnumerable<ValidationFinding>
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
