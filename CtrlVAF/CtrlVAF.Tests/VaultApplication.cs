using CtrlVAF.BackgroundOperations;
using CtrlVAF.Core;
using CtrlVAF.Core.Attributes;
using CtrlVAF.Events;
using CtrlVAF.Validation;

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
    class VaultApplication<TConfig>: ConfigurableVaultApplicationBase<TConfig>
        where TConfig: class, new()
    {
        public LicenseContentBase licenseContent = null;

        public VaultApplication(): base()
        {

        }

        public VaultApplication(LicenseContentBase content): base()
        {
            licenseContent = content;
        }

        public void SetConfig(TConfig config)
        {
            Configuration = config;
        }

        public override void StartOperations(Vault vaultPersistent)
        {
            BackgroundDispatcher = new BackgroundDispatcher<TConfig>(this);

            EventDispatcher = new EventDispatcher<TConfig>(this);

            ValidatorDispatcher = new ValidatorDispatcher<TConfig>(this);

            if (this.GetType().IsDefined(typeof(UseLicensingAttribute)))
            {
                BackgroundDispatcher = new LicensedDispatcher(BackgroundDispatcher, licenseContent);

                EventDispatcher = new LicensedDispatcher(EventDispatcher, licenseContent);

                ValidatorDispatcher = new LicensedDispatcher<IEnumerable<ValidationFinding>>(ValidatorDispatcher, licenseContent);
            }
        }


    }
}
