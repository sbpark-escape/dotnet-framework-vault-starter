# AGENTS.md

This file guides coding agents working in this repository.

## Repository Purpose

`dotnet-framework-vault-starter` is a practical starter/helper for using HashiCorp Vault from legacy .NET Framework applications running on IIS or Windows Server.

## Target Environment

- .NET Framework 4.7.2 and 4.8
- ASP.NET WebForms
- ASMX services
- Windows Services
- IIS-hosted applications
- Windows Server scheduled jobs or console utilities

## Wording Rules

- Do not describe this project as production-ready, enterprise-grade, or a complete security solution.
- Prefer realistic wording: starter, helper, migration guide, safe defaults, examples for legacy environments.
- Be clear that Vault policy design, TLS setup, AppRole bootstrap, and production review remain user responsibilities.

## Secret Handling Rules

- Do not include real secrets.
- Do not include Vault tokens, RoleId, SecretId, internal URLs, connection strings, or any real secrets in issues, pull requests, logs, screenshots, or examples.
- Use dummy values only.
- Prefer `https://vault.example.invalid` for fake Vault addresses.
- Prefer `__DUMMY_...__` placeholders for tokens, RoleId values, SecretId values, and connection strings.
- Never log returned secret values. Use `SET`, `EMPTY`, `MISSING`, or `MASKED`.

## Compatibility Rules

- Keep the main library compatible with .NET Framework 4.7.2.
- Do not add dependencies unless they materially help legacy adoption.
- VaultSharp compatibility should be documented when package behavior changes.
- Default tests must not require a real Vault server.

## Documentation Priority

- README and docs quality matter as much as code.
- Link new docs from README or a relevant docs index.
- Keep examples copyable but clearly dummy-only.
- Expand troubleshooting before adding broad abstractions.

## Testing

Run these checks when relevant:

```powershell
dotnet build .\dotnet-framework-vault-starter.sln --configuration Release
dotnet run --configuration Release --project .\tests\DotNetFrameworkVaultStarter.Tests\DotNetFrameworkVaultStarter.Tests.csproj
.\scripts\Test-MarkdownLinks.ps1
```

## Public OSS Maintenance

- Prefer small, reviewable pull requests.
- Add or update issue templates and docs when changing contributor flow.
- Keep `CHANGELOG.md` factual.
- Do not invent stars, downloads, contributors, users, or adoption claims.

## Change Summary Format

When reporting changes, include:

- files changed
- docs added or updated
- tests or checks run
- security or secret-handling considerations
- remaining risks or next steps
