# Release Plan

This release plan keeps the repository moving in small, believable steps through July. It is intentionally modest and can be adjusted based on real feedback.

## v0.1.0: Initial Public Starter Release

### Goal

Publish the first public version of the starter with a clear README, examples, tests, and OSS maintenance files.

### Included Changes

- .NET Framework 4.7.2 helper library.
- VaultSharp-based Token and AppRole setup.
- Environment variable priority with App.config fallback.
- Safe diagnostics and masking helpers.
- Console, WebForms/ASMX-style, and Windows Service-style examples.
- Basic Vault-free tests.
- Initial docs, issue templates, PR template, CI, and changelog.

### Excluded Changes

- NuGet package publishing.
- Real Vault integration tests.
- KV v1 support.
- Secret caching or rotation helpers.

### Release Checklist

- Build succeeds.
- Tests pass.
- Markdown links pass.
- README renders correctly on GitHub.
- No real secrets or internal URLs appear in docs, examples, tests, or templates.
- GitHub release notes match `CHANGELOG.md`.

### Maintenance Evidence

- Shows initial source publication.
- Shows CI and test setup.
- Shows baseline docs and issue templates.

## v0.1.1: Documentation and Troubleshooting Cleanup

### Goal

Improve first-user experience with clearer docs and practical troubleshooting.

### Included Changes

- More IIS App Pool environment variable notes.
- More AppRole missing-value diagnostics guidance.
- Improved README wording based on first review.
- Better links between README and docs.
- Good first issues opened and triaged.

### Excluded Changes

- API redesign.
- NuGet publishing.
- New runtime dependencies.

### Release Checklist

- Docs link check passes.
- Build and tests still pass.
- Release notes describe only the documentation and troubleshooting changes.

### Maintenance Evidence

- Shows active docs maintenance after initial release.
- Shows issue triage and response to likely user problems.

## v0.1.2: Examples and Test Improvements

### Goal

Add focused examples and tests that make the starter easier to evaluate without a real Vault server.

### Included Changes

- Expanded WebForms or ASMX example.
- Improved Windows Service notes.
- Additional tests for diagnostics and missing settings.
- Possibly a multi-key secret reading example.

### Excluded Changes

- Full local Vault integration workflow unless it is clearly separated from default CI.
- NuGet package publishing.
- Production deployment automation.

### Release Checklist

- Example projects compile.
- Default tests remain Vault-free.
- Docs state that examples use dummy values only.

### Maintenance Evidence

- Shows examples are being improved after release.
- Shows test coverage moving in small increments.

## v0.2.0: Expanded Legacy App Scenarios and Local Vault Dev Guide

### Goal

Add broader legacy scenario coverage once the initial starter is stable.

### Included Changes

- Classic WebForms or `packages.config` guidance.
- Windows Task Scheduler notes.
- Local Vault dev server or docker-compose guide using dummy data.
- Clearer KV v2 path helper guidance.

### Excluded Changes

- Claiming general production readiness.
- Replacing Vault policy design or deployment review.
- Mandatory local Vault dependency in default tests.

### Release Checklist

- New examples compile or are clearly documentation-only.
- Local Vault guide is optional and uses dummy data.
- CI remains understandable and maintainable.

### Maintenance Evidence

- Shows planned expansion based on realistic legacy use cases.
- Shows a path from starter documentation to hands-on local experimentation.
