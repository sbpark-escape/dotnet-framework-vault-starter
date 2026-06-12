# IIS Environment Variables

Use this document when a Vault-backed application works from a console or developer shell but fails under IIS, or when `VAULT_*` values appear to be missing after deployment. IIS environment variables can be confusing because the process that serves the application is not the same process where an administrator usually tests commands.

## Supported Variables

- `VAULT_ADDR`
- `VAULT_MOUNT_POINT`
- `VAULT_AUTH_METHOD`
- `VAULT_TOKEN`
- `VAULT_ROLE_ID`
- `VAULT_SECRET_ID`

## Recommended Server Pattern

Use machine-level variables or a deployment mechanism that injects variables into the application pool process. Prefer AppRole on servers.

```powershell
[Environment]::SetEnvironmentVariable("VAULT_ADDR", "https://vault.example.invalid", "Machine")
[Environment]::SetEnvironmentVariable("VAULT_MOUNT_POINT", "secret", "Machine")
[Environment]::SetEnvironmentVariable("VAULT_AUTH_METHOD", "AppRole", "Machine")
[Environment]::SetEnvironmentVariable("VAULT_ROLE_ID", "__ROLE_ID_FROM_SECURE_DEPLOYMENT__", "Machine")
[Environment]::SetEnvironmentVariable("VAULT_SECRET_ID", "__SECRET_ID_FROM_SECURE_DEPLOYMENT__", "Machine")
```

Recycle the application pool after changes:

```powershell
Import-Module WebAdministration
Restart-WebAppPool -Name "YourAppPoolName"
```

If the worker process still does not see the variables, restart IIS during a maintenance window:

```powershell
iisreset
```

## Common Pitfalls

- Variables set with `$env:NAME = "value"` affect only the current PowerShell process.
- Variables set under one Windows user are not automatically visible to the application pool identity.
- Machine-level changes are not always visible to already-running worker processes.
- Web gardens or multiple servers need consistent configuration on every worker.
- Some deployment systems overwrite or clear environment blocks.

## Safe Diagnostics

Use:

```csharp
System.Diagnostics.Trace.TraceInformation(VaultDiagnostics.CreateReport());
```

The report prints `SET`, `EMPTY`, or `MISSING`. Masking helpers print `MASKED` for non-empty secret values. Diagnostics do not print token, role_id, secret_id, or secret values.

Protect any endpoint or page that exposes diagnostics. It still reveals process identity and configuration shape.
