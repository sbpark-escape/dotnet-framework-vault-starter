using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace DotNetFrameworkVaultStarter
{
    public sealed class VaultOptions
    {
        public const string EnvAddress = "VAULT_ADDR";
        public const string EnvMountPoint = "VAULT_MOUNT_POINT";
        public const string EnvAuthMethod = "VAULT_AUTH_METHOD";
        public const string EnvToken = "VAULT_TOKEN";
        public const string EnvRoleId = "VAULT_ROLE_ID";
        public const string EnvSecretId = "VAULT_SECRET_ID";

        public const string ConfigAddress = "Vault:Address";
        public const string ConfigMountPoint = "Vault:MountPoint";
        public const string ConfigAuthMethod = "Vault:AuthMethod";
        public const string ConfigToken = "Vault:Token";
        public const string ConfigRoleId = "Vault:RoleId";
        public const string ConfigSecretId = "Vault:SecretId";

        public VaultOptions()
        {
            MountPoint = "secret";
            AuthMethod = VaultAuthMethod.Token;
        }

        public string Address { get; set; }

        public string MountPoint { get; set; }

        public VaultAuthMethod AuthMethod { get; set; }

        public string Token { get; set; }

        public string RoleId { get; set; }

        public string SecretId { get; set; }

        public static VaultOptions Load()
        {
            return Load(ConfigurationManager.AppSettings, Environment.GetEnvironmentVariable);
        }

        public static VaultOptions Load(NameValueCollection appSettings, Func<string, string> getEnvironmentVariable)
        {
            if (appSettings == null)
            {
                appSettings = new NameValueCollection();
            }

            if (getEnvironmentVariable == null)
            {
                getEnvironmentVariable = Environment.GetEnvironmentVariable;
            }

            var options = new VaultOptions
            {
                Address = FirstNonWhiteSpace(getEnvironmentVariable(EnvAddress), appSettings[ConfigAddress]),
                MountPoint = FirstNonWhiteSpace(getEnvironmentVariable(EnvMountPoint), appSettings[ConfigMountPoint], "secret"),
                Token = FirstNonWhiteSpace(getEnvironmentVariable(EnvToken), appSettings[ConfigToken]),
                RoleId = FirstNonWhiteSpace(getEnvironmentVariable(EnvRoleId), appSettings[ConfigRoleId]),
                SecretId = FirstNonWhiteSpace(getEnvironmentVariable(EnvSecretId), appSettings[ConfigSecretId])
            };

            var authMethodText = FirstNonWhiteSpace(getEnvironmentVariable(EnvAuthMethod), appSettings[ConfigAuthMethod], "Token");
            options.AuthMethod = ParseAuthMethod(authMethodText);

            return options;
        }

        public void ThrowIfInvalid()
        {
            var errors = Validate();
            if (errors.Count > 0)
            {
                throw new VaultSecretException(string.Join("; ", errors), VaultFailureCategory.MissingConfiguration);
            }
        }

        public IList<string> Validate()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(Address))
            {
                errors.Add("Vault address is missing. Set VAULT_ADDR or App.config key Vault:Address.");
            }

            if (string.IsNullOrWhiteSpace(MountPoint))
            {
                errors.Add("Vault KV mount point is missing. Set VAULT_MOUNT_POINT or App.config key Vault:MountPoint.");
            }

            if (AuthMethod == VaultAuthMethod.Token && string.IsNullOrWhiteSpace(Token))
            {
                errors.Add("Vault token is missing. Set VAULT_TOKEN or App.config key Vault:Token for local token authentication.");
            }

            if (AuthMethod == VaultAuthMethod.AppRole)
            {
                if (string.IsNullOrWhiteSpace(RoleId))
                {
                    errors.Add("Vault AppRole role_id is missing. Set VAULT_ROLE_ID or App.config key Vault:RoleId.");
                }

                if (string.IsNullOrWhiteSpace(SecretId))
                {
                    errors.Add("Vault AppRole secret_id is missing. Set VAULT_SECRET_ID or App.config key Vault:SecretId.");
                }
            }

            return errors;
        }

        public static VaultAuthMethod ParseAuthMethod(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return VaultAuthMethod.Token;
            }

            if (value.Equals("token", StringComparison.OrdinalIgnoreCase))
            {
                return VaultAuthMethod.Token;
            }

            if (value.Equals("approle", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("app-role", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("app_role", StringComparison.OrdinalIgnoreCase))
            {
                return VaultAuthMethod.AppRole;
            }

            throw new VaultSecretException(
                "Unsupported Vault authentication method. Use Token or AppRole.",
                VaultFailureCategory.MissingConfiguration);
        }

        private static string FirstNonWhiteSpace(params string[] values)
        {
            foreach (var value in values)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value.Trim();
                }
            }

            return null;
        }
    }
}
