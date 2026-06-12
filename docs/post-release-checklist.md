# Post-Release Checklist

Use this checklist after publishing source code or creating a GitHub release. It is ordered so a maintainer can move from repository settings to release hygiene, issue setup, maintenance evidence, and eventual OSS support application preparation.

## A. GitHub Repository Settings

- Confirm repository visibility is public.
- Set repository description:
  - `Practical starter/helper for using HashiCorp Vault from legacy .NET Framework/IIS applications.`
- Set topics:
  - `dotnet-framework`
  - `csharp`
  - `iis`
  - `hashicorp-vault`
  - `vaultsharp`
  - `secrets-management`
  - `legacy-dotnet`
- Confirm Issues are enabled.
- Leave Discussions disabled unless there is enough community activity to justify them.
- Confirm README renders correctly.
- Confirm CI badge if a badge is added later.
- Confirm the license is displayed by GitHub.
- Confirm the security policy is visible.
- Confirm default branch is `main`.
- Consider branch protection for `main` with CI required.

## B. Release Management

- Confirm whether a `v0.1.0` release exists.
- Confirm `CHANGELOG.md` matches the GitHub release notes.
- Confirm release notes avoid overclaiming.
- Describe the first release as an `Initial public starter release`.
- Do not publish a NuGet package until the API surface and installation story are reviewed.
- Keep `Unreleased` in `CHANGELOG.md` for work after the tag.

## C. Issues Management

- Create 5 to 8 `good first issue` items.
- Include at least one documentation issue.
- Include at least one example issue.
- Include at least one troubleshooting issue.
- Include at least one enhancement issue.
- Include acceptance criteria in every issue.
- Keep scope small enough for a real contributor to finish.
- Avoid issues that require real Vault credentials or internal infrastructure.

## D. Maintenance Evidence

- Make small README improvement commits.
- Make small docs improvement commits.
- Make example improvement commits.
- Make test improvement commits.
- Open issues and close a few with clear PRs.
- Create a `v0.1.1` patch release after meaningful docs or test cleanup.
- Create a `v0.2.0` milestone for larger example coverage.
- Keep release notes short and factual.

## E. External Sharing

- Share the project as a practical problem-solving starter, not as a star request.
- Possible places to share:
  - LinkedIn
  - OKKY
  - Velog
  - GitHub profile README
- Avoid overclaiming adoption or maturity.
- If stars are low, say so honestly or omit the metric.
- Never invent stars, downloads, contributors, or production users.

## F. Final July Support Check

- Confirm repository is public.
- Confirm README purpose is clear in the first screen.
- Confirm at least one release exists.
- Confirm issues and milestones exist.
- Confirm visible maintenance history exists.
- Confirm GitHub username and profile are public enough for review.
- Use only real numbers for stars, downloads, contributors, releases, and issues.
