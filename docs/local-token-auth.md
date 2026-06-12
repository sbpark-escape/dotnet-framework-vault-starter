# Local Token Auth

Use this document when a developer needs to run a legacy .NET Framework app locally against non-production Vault data. Token auth is intended here for local development and small experiments; IIS and Windows Server deployments should normally use AppRole.

## Local Environment Setup

PowerShell with dummy values:

```powershell
$env:VAULT_ADDR = "https://vault.example.invalid"
$env:VAULT_MOUNT_POINT = "secret"
$env:VAULT_AUTH_METHOD = "Token"
$env:VAULT_TOKEN = "__LOCAL_DEV_TOKEN__"
```

This affects only the current shell and child processes.

## App.config Fallback

```xml
<appSettings>
  <add key="Vault:Address" value="https://vault.example.invalid" />
  <add key="Vault:MountPoint" value="secret" />
  <add key="Vault:AuthMethod" value="Token" />
  <add key="Vault:Token" value="__LOCAL_DEV_TOKEN__" />
</appSettings>
```

Do not commit a real token. Prefer environment variables or a local user secret workflow outside this repository.

## Local Vault Dev Server

If you use Vault dev mode, remember that it is not production-like. It can help test API flow, but it should not be used as evidence that server auth, TLS, or policies are correct.

Example only:

```bash
vault server -dev
```

Then write dummy data:

```bash
vault kv put secret/legacy-app/database connectionString="__DUMMY_CONNECTION_STRING__"
```

Never use production secrets in local examples.
