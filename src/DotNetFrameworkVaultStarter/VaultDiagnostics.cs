using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Security.Principal;
using System.Text;

namespace DotNetFrameworkVaultStarter
{
    public static class VaultDiagnostics
    {
        private static readonly string[] EnvironmentNames =
        {
            VaultOptions.EnvAddress,
            VaultOptions.EnvMountPoint,
            VaultOptions.EnvAuthMethod,
            VaultOptions.EnvToken,
            VaultOptions.EnvRoleId,
            VaultOptions.EnvSecretId
        };

        public static string CreateReport()
        {
            return CreateReport(VaultOptions.Load(), ConfigurationManager.AppSettings, Environment.GetEnvironmentVariable);
        }

        public static string CreateReport(
            VaultOptions options,
            NameValueCollection appSettings,
            Func<string, string> getEnvironmentVariable)
        {
            if (options == null)
            {
                options = new VaultOptions();
            }

            if (appSettings == null)
            {
                appSettings = new NameValueCollection();
            }

            if (getEnvironmentVariable == null)
            {
                getEnvironmentVariable = Environment.GetEnvironmentVariable;
            }

            var builder = new StringBuilder();
            builder.AppendLine("Vault diagnostics");
            builder.AppendLine("Process user: " + GetCurrentProcessUser());
            builder.AppendLine("Auth method: " + options.AuthMethod);
            builder.AppendLine("Configured address: " + Status(options.Address));
            builder.AppendLine("Configured mount point: " + Status(options.MountPoint));
            builder.AppendLine("Environment variables:");

            foreach (var name in EnvironmentNames)
            {
                builder.AppendLine("  " + name + "=" + Status(getEnvironmentVariable(name)));
            }

            builder.AppendLine("App.config keys:");
            builder.AppendLine("  " + VaultOptions.ConfigAddress + "=" + Status(appSettings[VaultOptions.ConfigAddress]));
            builder.AppendLine("  " + VaultOptions.ConfigMountPoint + "=" + Status(appSettings[VaultOptions.ConfigMountPoint]));
            builder.AppendLine("  " + VaultOptions.ConfigAuthMethod + "=" + Status(appSettings[VaultOptions.ConfigAuthMethod]));
            builder.AppendLine("  " + VaultOptions.ConfigToken + "=" + Status(appSettings[VaultOptions.ConfigToken]));
            builder.AppendLine("  " + VaultOptions.ConfigRoleId + "=" + Status(appSettings[VaultOptions.ConfigRoleId]));
            builder.AppendLine("  " + VaultOptions.ConfigSecretId + "=" + Status(appSettings[VaultOptions.ConfigSecretId]));

            var validationMessages = options.Validate();
            if (validationMessages.Count > 0)
            {
                builder.AppendLine("Configuration warnings:");
                foreach (var message in validationMessages)
                {
                    builder.AppendLine("  " + message);
                }
            }

            return builder.ToString();
        }

        public static string Status(string value)
        {
            if (value == null)
            {
                return "MISSING";
            }

            return value.Trim().Length == 0 ? "EMPTY" : "SET";
        }

        public static string MaskSecretValue(string value)
        {
            if (value == null)
            {
                return "MISSING";
            }

            return value.Trim().Length == 0 ? "EMPTY" : "MASKED";
        }

        public static IDictionary<string, string> MaskSecretDictionary(IDictionary<string, string> values)
        {
            var masked = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            if (values == null)
            {
                return masked;
            }

            foreach (var item in values)
            {
                masked[item.Key] = MaskSecretValue(item.Value);
            }

            return masked;
        }

        public static string DescribeException(Exception exception)
        {
            if (exception == null)
            {
                return "No exception was provided.";
            }

            var category = ClassifyException(exception);
            return "Vault error category: " + category + ". " + GuidanceFor(category);
        }

        public static VaultFailureCategory ClassifyException(Exception exception)
        {
            if (exception == null)
            {
                return VaultFailureCategory.Unknown;
            }

            var webException = exception as WebException;
            if (webException != null)
            {
                return VaultFailureCategory.Network;
            }

            var statusCode = TryReadStatusCode(exception);
            if (statusCode == 403)
            {
                return VaultFailureCategory.PermissionDenied;
            }

            if (statusCode == 404)
            {
                return VaultFailureCategory.NotFound;
            }

            if (statusCode == 400 || statusCode == 401)
            {
                return VaultFailureCategory.Authentication;
            }

            var text = exception.ToString().ToLowerInvariant();
            if (text.Contains("permission denied"))
            {
                return VaultFailureCategory.PermissionDenied;
            }

            if (text.Contains("no value found") || text.Contains("not found") || text.Contains("404"))
            {
                return VaultFailureCategory.NotFound;
            }

            if (text.Contains("mount") || text.Contains("unsupported path") || text.Contains("invalid path"))
            {
                return VaultFailureCategory.MountPointOrPath;
            }

            return VaultFailureCategory.VaultApiError;
        }

        private static string GetCurrentProcessUser()
        {
            try
            {
                var identity = WindowsIdentity.GetCurrent();
                if (identity != null && !string.IsNullOrWhiteSpace(identity.Name))
                {
                    return identity.Name;
                }
            }
            catch
            {
                // Ignore identity lookup failures; diagnostics must never break application startup.
            }

            return Environment.UserDomainName + "\\" + Environment.UserName;
        }

        private static int? TryReadStatusCode(Exception exception)
        {
            var current = exception;
            while (current != null)
            {
                var property = current.GetType().GetProperty("StatusCode");
                if (property != null)
                {
                    var value = property.GetValue(current, null);
                    if (value is int)
                    {
                        return (int)value;
                    }

                    if (value is HttpStatusCode)
                    {
                        return (int)(HttpStatusCode)value;
                    }
                }

                current = current.InnerException;
            }

            return null;
        }

        private static string GuidanceFor(VaultFailureCategory category)
        {
            switch (category)
            {
                case VaultFailureCategory.PermissionDenied:
                    return "Check Vault policy, AppRole binding, and the exact KV path. Do not log tokens or secret_id values.";
                case VaultFailureCategory.NotFound:
                    return "Check whether the KV path exists and whether the application is using the expected mount point.";
                case VaultFailureCategory.MountPointOrPath:
                    return "Check VAULT_MOUNT_POINT, KV engine version, and whether the path should omit the mount point prefix.";
                case VaultFailureCategory.Authentication:
                    return "Check VAULT_AUTH_METHOD and whether the required token or AppRole values are present.";
                case VaultFailureCategory.Network:
                    return "Check VAULT_ADDR, TLS trust, proxy/firewall rules, and IIS application pool environment inheritance.";
                default:
                    return "Enable safe diagnostics and compare environment variables with App.config fallback values.";
            }
        }
    }
}
