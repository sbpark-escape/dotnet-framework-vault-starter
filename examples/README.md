# Examples

These examples show how the helper can be attached to common legacy .NET Framework application shapes. They are intentionally small and use dummy values only.

The examples compile without a Vault server. A real Vault lookup requires replacing dummy configuration with your own non-production Vault setup.

## Included Examples

- [ConsoleApp](ConsoleApp/README.md): command-line or scheduled job style startup.
- [LegacyWebFormsExample](LegacyWebFormsExample/README.md): WebForms or ASMX style usage with a small factory.
- [WindowsServiceExample](WindowsServiceExample/README.md): Windows Service style startup flow.

## Safety Rules

- Do not commit real tokens, role ids, secret ids, Vault addresses, or connection strings.
- Log only `SET`, `EMPTY`, `MISSING`, or `MASKED`.
- Treat diagnostics endpoints as protected operational tools.
