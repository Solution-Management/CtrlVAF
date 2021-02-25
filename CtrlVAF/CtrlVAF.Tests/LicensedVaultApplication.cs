using CtrlVAF.BackgroundOperations;
using CtrlVAF.Core;
using CtrlVAF.Core.Attributes;
using CtrlVAF.Events;
using CtrlVAF.Validation;

using MFiles.VAF;
using MFiles.VAF.Common;
using MFiles.VAF.Configuration;

using MFilesAPI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Tests
{
    [UseLicensing]
    class LicensedVaultApplication<TConfig>: VaultApplication<TConfig>
        where TConfig: class, new()
    {
        public LicensedVaultApplication() : base()
        {

        }

        public LicensedVaultApplication(LicenseContentBase content) : base(content)
        {
            
        }
    }
}
