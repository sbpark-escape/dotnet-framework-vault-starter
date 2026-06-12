# Migration From Hardcoded Secrets

Use this document when you are replacing hardcoded connection strings, API keys, or config-file secrets in an existing .NET Framework application. The safest migration is small and reversible.

## 1. Inventory Secrets

Look for:

- connection strings
- API keys
- SMTP passwords
- service account passwords
- transform files with environment-specific values
- deployment scripts that echo secrets

## 2. Add Vault Settings First

Deploy only configuration and diagnostics first:

- `VAULT_ADDR`
- `VAULT_MOUNT_POINT`
- `VAULT_AUTH_METHOD`
- `VAULT_ROLE_ID` and `VAULT_SECRET_ID` for servers

Confirm diagnostics show `SET` for required values.

## 3. Move One Low-Risk Secret

Before:

```csharp
var apiKey = ConfigurationManager.AppSettings["LegacyApiKey"];
```

After:

```csharp
var provider = VaultSecretProviderFactory.Create();
var apiKey = provider.GetSecret("legacy-app/external-api", "apiKey");
```

Log only status:

```csharp
Trace.TraceInformation("LegacyApiKey: " + VaultDiagnostics.Status(apiKey));
```

## 4. Migrate Connection Strings Carefully

Before:

```csharp
var connectionString =
    ConfigurationManager.ConnectionStrings["LegacyDb"].ConnectionString;
```

After:

```csharp
var provider = VaultSecretProviderFactory.Create();
var connectionString = provider.GetSecret("legacy-app/database", "connectionString");
```

Recommended rollout:

- deploy code with a feature flag or fallback path
- test on one non-critical environment
- monitor connection pool and startup behavior
- remove fallback only after operational confidence is high

## 5. Remove Hardcoded Values

After the Vault path is stable:

- delete real values from config files
- rotate the moved secret
- remove old deployment variables
- check logs and build artifacts
- update runbooks

## 6. Keep Rollback Practical

For old applications, avoid large refactors during the first secret migration. Keep the change near the previous configuration access point.
