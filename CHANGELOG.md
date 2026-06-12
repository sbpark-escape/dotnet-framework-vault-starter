# Changelog

All notable changes to this project will be documented in this file.

This project follows the spirit of [Keep a Changelog](https://keepachangelog.com/en/1.1.0/) with small, practical release notes.

## [Unreleased]

### Added

- Per-example README files for ConsoleApp, WebForms/ASMX style, and Windows Service style usage.
- Markdown link validation script for local checks and CI.
- Expanded troubleshooting notes for IIS environment variables, AppRole values, KV path confusion, TLS/certificate errors, and safe logging.
- Maintainer roadmap and release planning notes.
- More detailed contribution and issue template guidance.
- Post-release checklist, maintainer issue drafts, and release plan.

### Changed

- README roadmap now links to the dedicated roadmap document.
- Diagnostics masking language now consistently uses `MASKED` for non-empty secret values.

## [0.1.0] - 2026-06-12

### Added

- Initial .NET Framework 4.7.2 helper library.
- Token and AppRole authentication support through VaultSharp.
- Environment-variable-first configuration with App.config fallback.
- Safe diagnostics that report `SET`, `EMPTY`, or `MISSING` without printing secret values.
- Console, WebForms/ASMX-style, and Windows Service-style examples.
- Local tests for option loading, fallback behavior, and masking.
- IIS, AppRole, local token, troubleshooting, and migration docs.

### Notes

- This release is a practical starter/helper with examples and documentation for legacy environments.
- KV v2 is assumed.

## Possible [0.1.1] Candidates

These are not committed promises. They are small candidates for the next patch release:

- Polish IIS troubleshooting based on first feedback.
- Add classic WebForms or `packages.config` reference notes.
- Improve XML documentation comments for public APIs.
- Add a multi-key secret reading example.
- Review README and docs for release wording and dummy-only examples.
