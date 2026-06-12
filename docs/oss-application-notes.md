# OSS Application Notes

This document is not an application draft. It is a factual note for maintainers who may later describe this repository in an OSS support application or project summary.

## Problem This Repo Addresses

Legacy .NET Framework applications often keep connection strings, API keys, or service credentials in `Web.config`, `App.config`, deployment transforms, scripts, or static helper classes. Moving those applications to a modern runtime may take time, but teams can still improve secret handling earlier by introducing Vault-backed lookup with cautious defaults and clear migration guidance.

## Target Users

- Maintainers of .NET Framework 4.7.2 or 4.8 applications.
- Teams operating IIS-hosted WebForms or ASMX applications.
- Teams operating Windows Services, scheduled jobs, or console utilities on Windows Server.
- Engineers migrating hardcoded secrets out of legacy code with minimal disruption.

## Why Legacy .NET Framework and IIS Matter

Many production systems still run on .NET Framework and IIS because they are business-critical, costly to rewrite, or tied to older deployment models. These systems may not be ready for .NET Core or ASP.NET Core, but they still need safer secret-handling patterns. Practical examples for IIS App Pool identity, Windows environment variables, and App.config fallback can reduce the gap between modern secret management guidance and legacy operational reality.

## Current Features

- `net472` helper library usable from .NET Framework 4.7.2 and 4.8 applications.
- VaultSharp-based Token and AppRole authentication setup.
- Environment variable priority with App.config/Web.config fallback.
- Safe diagnostics that report `SET`, `EMPTY`, `MISSING`, or `MASKED`.
- Console, WebForms/ASMX-style, and Windows Service-style examples.
- Vault-free tests for configuration loading and diagnostics behavior.
- IIS, AppRole, local token, troubleshooting, and migration documentation.

## Maintenance Plan

Through July, maintenance should focus on small, reviewable updates:

- improve docs based on likely first-user questions
- add focused tests for configuration behavior
- expand examples for common legacy hosting models
- keep templates and release notes tidy
- avoid broad security claims or large rewrites

See [roadmap.md](roadmap.md) for milestone details.

## Useful Codex Maintenance Tasks

Codex can help with routine OSS maintenance tasks such as:

- issue triage and summarization
- PR review for docs, examples, and tests
- docs improvement and consistency checks
- release note generation from merged changes
- test coverage improvement for configuration and diagnostics behavior
- example expansion for legacy application shapes

Codex should not be used as a substitute for security review, Vault policy design, or production deployment approval.

## Expressions to Avoid

Avoid phrases that overstate the project:

- `production-ready`
- `enterprise-grade`
- `perfect security`
- `complete Vault integration`
- `drop-in security solution`
- `secure by default for all environments`

Prefer realistic wording:

- `practical starter`
- `helper`
- `migration guide`
- `safe defaults`
- `examples for legacy environments`
- `documentation for IIS and Windows Server adoption`

## Current Gaps

- No real Vault integration tests in default CI.
- KV v2 is assumed; KV v1 is not implemented.
- No NuGet package is published yet.
- No secret caching or rotation helper is included.
- AppRole `secret_id` delivery remains an operational concern.
- TLS, proxy, and Windows Server certificate trust guidance can be expanded.
- Classic non-SDK WebForms and `packages.config` samples are not included yet.
