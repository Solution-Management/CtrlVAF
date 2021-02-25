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
    class ValidatorResults: IEnumerable<KeyValuePair<Type, IEnumerable<ValidationFinding>>>
    {
        internal ConcurrentDictionary<Type, IEnumerable<ValidationFinding>> ResultsCache { get; set; }

        public IEnumerable<ValidationFinding> GetValidationResults(Type configurationType)
        {
            if(ResultsCache.TryGetValue(configurationType, out IEnumerable<ValidationFinding> findings))
                return findings;
            
            return default;
        }

        public IEnumerable<ValidationFinding> GetValidationResults(object configuration)
        {
            return GetValidationResults(configuration.GetType());
        }

        public IEnumerator<KeyValuePair<Type, IEnumerable<ValidationFinding>>> GetEnumerator()
        {
            return ResultsCache.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
