using CtrlVAF.Core;

using MFiles.VAF.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Tests
{
    static class Helpers
    {
        public static VaultApplication<TConfig> InitializeTestVA<TConfig>(TConfig configuration, LicenseContentBase licenseContent = null)
            where TConfig: class, new()
        {
            var vaultApplication = new VaultApplication<TConfig>();
            var vault = new MFilesAPI.Vault();

            vaultApplication.SetConfig(configuration);
            vaultApplication.licenseContent = licenseContent;
            vaultApplication.StartOperations(vault);
            vaultApplication.SetPermanentVault(vault);

            return vaultApplication;
        }

        public static LicensedVaultApplication<TConfig> InitializeLicensedTestVA<TConfig>(TConfig configuration, LicenseContentBase licenseContent = null)
            where TConfig : class, new()
        {
            var vaultApplication = new LicensedVaultApplication<TConfig>();
            var vault = new MFilesAPI.Vault();

            vaultApplication.SetConfig(configuration);
            vaultApplication.licenseContent = licenseContent;
            vaultApplication.StartOperations(vault);
            vaultApplication.SetPermanentVault(vault);

            return vaultApplication;
        }
    }
}
