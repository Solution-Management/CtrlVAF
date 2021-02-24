using CtrlVAF.BackgroundOperations;
using CtrlVAF.Core;
using CtrlVAF.Core.Attributes;
using CtrlVAF.Events;
using CtrlVAF.Validators;

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
    class VaultApplication: ConfigurableVaultApplicationBase<Configuration>
    {
        private LicenseContentBase licenseContent = null;

        public VaultApplication(): base()
        {

        }

        public VaultApplication(LicenseContentBase content): base()
        {
            licenseContent = content;
        }

        public void SetConfig(Configuration config)
        {
            Configuration = config;
        }

        public override void StartOperations(Vault vaultPersistent)
        {
            BackgroundDispatcher = new BackgroundDispatcher<Configuration>(this);

            EventDispatcher = new EventDispatcher();

            ValidatorDispatcher = new ValidatorDispatcher<Configuration>(this);

            if (this.GetType().IsDefined(typeof(UseLicensingAttribute)))
            {
                BackgroundDispatcher = new LicensedDispatcher(BackgroundDispatcher, licenseContent);

                EventDispatcher = new LicensedDispatcher(EventDispatcher, licenseContent);

                ValidatorDispatcher = new LicensedDispatcher<IEnumerable<ValidationFinding>>(ValidatorDispatcher, licenseContent);
            }
        }
    }
}
