using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using DotNetFrameworkVaultStarter;

namespace DotNetFrameworkVaultStarter.Tests
{
    internal static class Program
    {
        private static int Main()
        {
            var tests = new Action[]
            {
                OptionsLoadingUsesDefaults,
                EnvironmentVariablesTakePriority,
                AppConfigFallbackIsUsed,
                MissingRequiredSettingsThrowClearException,
                SecretValuesAreMasked,
                DiagnosticsReportDoesNotLeakSecretValues,
                TokenAuthSettingsLoadFromEnvironment,
                AppRoleAuthSettingsLoadFromEnvironment,
                UnsupportedAuthMethodThrowsClearException,
                DiagnosticsShowEmptySecretIdWithoutLeakingValues
            };

            foreach (var test in tests)
            {
                test();
                Console.WriteLine("PASS " + test.Method.Name);
            }

            return 0;
        }

        private static void OptionsLoadingUsesDefaults()
        {
            var appSettings = new NameValueCollection
            {
                { VaultOptions.ConfigAddress, "https://vault.example.invalid" },
                { VaultOptions.ConfigToken, "__DUMMY_LOCAL_TOKEN__" }
            };

            var options = VaultOptions.Load(appSettings, MissingEnvironment);

            AssertEqual("https://vault.example.invalid", options.Address, "Address should come from App.config.");
            AssertEqual("secret", options.MountPoint, "Mount point should default to secret.");
            AssertEqual(VaultAuthMethod.Token, options.AuthMethod, "Auth method should default to Token.");
        }

        private static void EnvironmentVariablesTakePriority()
        {
            var appSettings = new NameValueCollection
            {
                { VaultOptions.ConfigAddress, "https://config-vault.example.invalid" },
                { VaultOptions.ConfigMountPoint, "config-secret" },
                { VaultOptions.ConfigAuthMethod, "Token" },
                { VaultOptions.ConfigToken, "__DUMMY_CONFIG_TOKEN__" }
            };

            var environment = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { VaultOptions.EnvAddress, "https://env-vault.example.invalid" },
                { VaultOptions.EnvMountPoint, "env-secret" },
                { VaultOptions.EnvAuthMethod, "AppRole" },
                { VaultOptions.EnvRoleId, "__DUMMY_ENV_ROLE_ID__" },
                { VaultOptions.EnvSecretId, "__DUMMY_ENV_SECRET_ID__" }
            };

            var options = VaultOptions.Load(appSettings, name => GetEnvironment(environment, name));

            AssertEqual("https://env-vault.example.invalid", options.Address, "Address should prefer environment.");
            AssertEqual("env-secret", options.MountPoint, "Mount point should prefer environment.");
            AssertEqual(VaultAuthMethod.AppRole, options.AuthMethod, "Auth method should prefer environment.");
            AssertEqual("__DUMMY_ENV_ROLE_ID__", options.RoleId, "RoleId should prefer environment.");
            AssertEqual("__DUMMY_ENV_SECRET_ID__", options.SecretId, "SecretId should prefer environment.");
        }

        private static void AppConfigFallbackIsUsed()
        {
            var appSettings = new NameValueCollection
            {
                { VaultOptions.ConfigAddress, "https://config-vault.example.invalid" },
                { VaultOptions.ConfigMountPoint, "legacy-kv" },
                { VaultOptions.ConfigAuthMethod, "AppRole" },
                { VaultOptions.ConfigRoleId, "__DUMMY_CONFIG_ROLE_ID__" },
                { VaultOptions.ConfigSecretId, "__DUMMY_CONFIG_SECRET_ID__" }
            };

            var options = VaultOptions.Load(appSettings, MissingEnvironment);

            AssertEqual("https://config-vault.example.invalid", options.Address, "Address should fall back to App.config.");
            AssertEqual("legacy-kv", options.MountPoint, "Mount point should fall back to App.config.");
            AssertEqual(VaultAuthMethod.AppRole, options.AuthMethod, "Auth method should fall back to App.config.");
            AssertEqual("__DUMMY_CONFIG_ROLE_ID__", options.RoleId, "RoleId should fall back to App.config.");
            AssertEqual("__DUMMY_CONFIG_SECRET_ID__", options.SecretId, "SecretId should fall back to App.config.");
        }

        private static void SecretValuesAreMasked()
        {
            AssertEqual("MASKED", VaultDiagnostics.MaskSecretValue("__DUMMY_SECRET_VALUE__"), "Non-empty secret should be masked.");
            AssertEqual("EMPTY", VaultDiagnostics.MaskSecretValue(string.Empty), "Empty secret should be marked empty.");
            AssertEqual("MISSING", VaultDiagnostics.MaskSecretValue(null), "Missing secret should be marked missing.");

            var masked = VaultDiagnostics.MaskSecretDictionary(new Dictionary<string, string>
            {
                { "connectionString", "__DUMMY_CONNECTION_STRING_THAT_SHOULD_BE_MASKED__" }
            });

            AssertEqual("MASKED", masked["connectionString"], "Dictionary values should be masked.");
        }

        private static void MissingRequiredSettingsThrowClearException()
        {
            var exception = AssertThrows<VaultSecretException>(
                delegate
                {
                    VaultSecretProviderFactory.Create(new VaultOptions
                    {
                        AuthMethod = VaultAuthMethod.AppRole,
                        MountPoint = "secret",
                        Address = "https://vault.example.invalid",
                        RoleId = "__DUMMY_ROLE_ID__"
                    });
                },
                "Missing AppRole SecretId should throw.");

            AssertEqual(VaultFailureCategory.MissingConfiguration, exception.Category, "Missing SecretId should be categorized as configuration.");
            AssertContains("secret_id is missing", exception.Message, "Exception should name the missing AppRole value.");
            AssertDoesNotContain("__DUMMY_ROLE_ID__", exception.Message, "Exception should not include configured RoleId.");
        }

        private static void DiagnosticsReportDoesNotLeakSecretValues()
        {
            var appSettings = new NameValueCollection
            {
                { VaultOptions.ConfigAddress, "https://vault.example.invalid" },
                { VaultOptions.ConfigAuthMethod, "Token" },
                { VaultOptions.ConfigToken, "config-token-should-not-leak" }
            };

            var environment = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { VaultOptions.EnvToken, "env-token-should-not-leak" }
            };

            var options = VaultOptions.Load(appSettings, name => GetEnvironment(environment, name));
            var report = VaultDiagnostics.CreateReport(options, appSettings, name => GetEnvironment(environment, name));

            AssertDoesNotContain("config-token-should-not-leak", report, "Diagnostics should not include App.config token.");
            AssertDoesNotContain("env-token-should-not-leak", report, "Diagnostics should not include environment token.");
            AssertContains(VaultOptions.EnvToken + "=SET", report, "Diagnostics should include status for token.");
        }

        private static void TokenAuthSettingsLoadFromEnvironment()
        {
            var environment = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { VaultOptions.EnvAddress, "https://vault.example.invalid" },
                { VaultOptions.EnvMountPoint, "secret" },
                { VaultOptions.EnvAuthMethod, "Token" },
                { VaultOptions.EnvToken, "__DUMMY_TOKEN_FOR_TEST__" }
            };

            var options = VaultOptions.Load(new NameValueCollection(), name => GetEnvironment(environment, name));

            AssertEqual(VaultAuthMethod.Token, options.AuthMethod, "Token auth should load from environment.");
            AssertEqual("__DUMMY_TOKEN_FOR_TEST__", options.Token, "Token should load from environment.");
            AssertEqual(0, options.Validate().Count, "Token auth environment settings should validate.");
        }

        private static void AppRoleAuthSettingsLoadFromEnvironment()
        {
            var environment = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { VaultOptions.EnvAddress, "https://vault.example.invalid" },
                { VaultOptions.EnvMountPoint, "secret" },
                { VaultOptions.EnvAuthMethod, "AppRole" },
                { VaultOptions.EnvRoleId, "__DUMMY_ROLE_ID_FOR_TEST__" },
                { VaultOptions.EnvSecretId, "__DUMMY_SECRET_ID_FOR_TEST__" }
            };

            var options = VaultOptions.Load(new NameValueCollection(), name => GetEnvironment(environment, name));

            AssertEqual(VaultAuthMethod.AppRole, options.AuthMethod, "AppRole auth should load from environment.");
            AssertEqual("__DUMMY_ROLE_ID_FOR_TEST__", options.RoleId, "RoleId should load from environment.");
            AssertEqual("__DUMMY_SECRET_ID_FOR_TEST__", options.SecretId, "SecretId should load from environment.");
            AssertEqual(0, options.Validate().Count, "AppRole auth environment settings should validate.");
        }

        private static void UnsupportedAuthMethodThrowsClearException()
        {
            var appSettings = new NameValueCollection
            {
                { VaultOptions.ConfigAddress, "https://vault.example.invalid" },
                { VaultOptions.ConfigAuthMethod, "WindowsIntegratedAuth" },
                { VaultOptions.ConfigToken, "__DUMMY_TOKEN_FOR_TEST__" }
            };

            var exception = AssertThrows<VaultSecretException>(
                delegate { VaultOptions.Load(appSettings, MissingEnvironment); },
                "Unsupported AuthMethod should throw.");

            AssertEqual(VaultFailureCategory.MissingConfiguration, exception.Category, "Unsupported auth method should be configuration failure.");
            AssertContains("Unsupported Vault authentication method", exception.Message, "Exception should explain supported auth methods.");
            AssertDoesNotContain("__DUMMY_TOKEN_FOR_TEST__", exception.Message, "Exception should not include token value.");
        }

        private static void DiagnosticsShowEmptySecretIdWithoutLeakingValues()
        {
            var appSettings = new NameValueCollection
            {
                { VaultOptions.ConfigAddress, "https://vault.example.invalid" },
                { VaultOptions.ConfigAuthMethod, "AppRole" },
                { VaultOptions.ConfigRoleId, "__DUMMY_ROLE_ID_SHOULD_NOT_LEAK__" },
                { VaultOptions.ConfigSecretId, string.Empty }
            };

            var options = VaultOptions.Load(appSettings, MissingEnvironment);
            var report = VaultDiagnostics.CreateReport(options, appSettings, MissingEnvironment);

            AssertContains(VaultOptions.ConfigSecretId + "=EMPTY", report, "Diagnostics should show EMPTY SecretId.");
            AssertDoesNotContain("__DUMMY_ROLE_ID_SHOULD_NOT_LEAK__", report, "Diagnostics should not include RoleId value.");
            AssertDoesNotContain("SecretId=\"", report, "Diagnostics should not include SecretId assignment text.");
        }

        private static string MissingEnvironment(string name)
        {
            return null;
        }

        private static string GetEnvironment(IDictionary<string, string> environment, string name)
        {
            string value;
            return environment.TryGetValue(name, out value) ? value : null;
        }

        private static void AssertEqual<T>(T expected, T actual, string message)
        {
            if (!object.Equals(expected, actual))
            {
                throw new InvalidOperationException(message + " Expected: " + expected + ". Actual: " + actual + ".");
            }
        }

        private static void AssertContains(string expected, string actual, string message)
        {
            if (actual == null || actual.IndexOf(expected, StringComparison.Ordinal) < 0)
            {
                throw new InvalidOperationException(message + " Missing: " + expected + ".");
            }
        }

        private static void AssertDoesNotContain(string unexpected, string actual, string message)
        {
            if (actual != null && actual.IndexOf(unexpected, StringComparison.Ordinal) >= 0)
            {
                throw new InvalidOperationException(message + " Found: " + unexpected + ".");
            }
        }

        private static TException AssertThrows<TException>(Action action, string message)
            where TException : Exception
        {
            try
            {
                action();
            }
            catch (TException exception)
            {
                return exception;
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException(
                    message + " Expected: " + typeof(TException).Name + ". Actual: " + exception.GetType().Name + ".",
                    exception);
            }

            throw new InvalidOperationException(message + " Expected exception was not thrown.");
        }
    }
}
