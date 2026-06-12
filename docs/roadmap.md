# Roadmap

Use this document to understand the small maintenance steps planned through July. The goal is to keep the repo useful and reviewable, not to promise a broad security platform.

## Maintenance Principles

- Keep changes small enough for legacy .NET Framework maintainers to review.
- Prefer practical docs, examples, and tests over large abstractions.
- Avoid real secrets, internal domains, or production-only assumptions in examples.
- Keep the default test suite independent from a real Vault server.

## Milestones

### v0.1.0: Initial Starter Release

Target: initial public release.

Scope:

- .NET Framework 4.7.2 helper library.
- Token and AppRole option loading.
- Environment-variable-first configuration with App.config fallback.
- Safe diagnostics that report status only.
- Console, WebForms/ASMX-style, and Windows Service-style examples.
- Basic tests for configuration and masking.
- Initial OSS files, issue templates, and CI.

### v0.1.1: Documentation and Troubleshooting Improvements

Target: small documentation release before wider sharing.

Candidate scope:

- Expand IIS App Pool environment variable troubleshooting.
- Improve example READMEs with execution flow and fallback config.
- Add markdown link validation to CI.
- Add clearer contribution guidance for documentation and example changes.
- Review all docs for dummy values and safe logging language.

### v0.2.0: More Legacy Examples and Test Coverage

Target: stronger starter for common legacy app shapes.

Candidate scope:

- Add a classic `packages.config` WebForms reference guide or sample.
- Add Windows Task Scheduler or console job notes.
- Add tests for missing required settings and unsupported auth methods.
- Add multi-key secret reading example.
- Improve troubleshooting around TLS and proxy behavior on Windows Server.

### Future

These are useful, but should wait until the starter has enough feedback:

- Evaluate NuGet packaging.
- Improve KV v2 helper ergonomics.
- Document a KV v1 strategy or adapter boundary.
- Add optional local Vault docker-compose example for integration testing.
- Consider an opt-in in-memory cache wrapper with explicit TTL.

## Release Plan Through July

The planned rhythm is small maintenance releases rather than large feature drops:

- Prepare `v0.1.0` once README, examples, tests, templates, and CI are internally consistent.
- Use `v0.1.1` for docs-only or mostly-docs improvements discovered during first public review.
- Use `v0.2.0` only if examples or tests expand enough to be useful as a minor release.
- Keep unreleased work in `CHANGELOG.md` until a tag is ready.
- Prefer one clear release note per user-visible change.

Before each release:

- run `dotnet build .\dotnet-framework-vault-starter.sln --configuration Release`
- run `dotnet run --configuration Release --project .\tests\DotNetFrameworkVaultStarter.Tests\DotNetFrameworkVaultStarter.Tests.csproj`
- run `.\scripts\Test-MarkdownLinks.ps1`
- search for real secrets, internal domains, real Vault addresses, and real connection strings
- confirm examples still use dummy values only

## Not Planned Right Now

- Broad Vault policy management.
- Secret rotation automation.
- A full deployment bootstrap system for AppRole `secret_id`.
- Replacing application-specific security review or infrastructure review.
