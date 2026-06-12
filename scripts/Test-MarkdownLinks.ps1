param(
    [string]$Root = "."
)

$ErrorActionPreference = "Stop"

$rootPath = (Resolve-Path -LiteralPath $Root).Path
$markdownFiles = Get-ChildItem -LiteralPath $rootPath -Recurse -File -Filter "*.md" |
    Where-Object { $_.FullName -notmatch "\\(bin|obj)\\" }

$linkPattern = '\[[^\]]+\]\(([^)]+)\)'
$failures = New-Object System.Collections.Generic.List[string]

foreach ($file in $markdownFiles) {
    $content = Get-Content -LiteralPath $file.FullName -Raw
    $matches = [regex]::Matches($content, $linkPattern)

    foreach ($match in $matches) {
        $target = $match.Groups[1].Value.Trim()

        if ($target.StartsWith("#") -or
            $target -match "^[a-zA-Z][a-zA-Z0-9+.-]*:" -or
            $target.StartsWith("mailto:")) {
            continue
        }

        $targetWithoutAnchor = ($target -split "#", 2)[0]
        if ([string]::IsNullOrWhiteSpace($targetWithoutAnchor)) {
            continue
        }

        $targetWithoutAnchor = [Uri]::UnescapeDataString($targetWithoutAnchor)
        $candidate = Join-Path -Path $file.DirectoryName -ChildPath $targetWithoutAnchor

        if (-not (Test-Path -LiteralPath $candidate)) {
            $relativeFile = Resolve-Path -LiteralPath $file.FullName -Relative
            $failures.Add("$relativeFile links to missing target '$target'")
        }
    }
}

if ($failures.Count -gt 0) {
    $failures | ForEach-Object { Write-Error $_ }
    exit 1
}

Write-Output "Markdown link validation passed for $($markdownFiles.Count) file(s)."
