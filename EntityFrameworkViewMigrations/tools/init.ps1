param($installPath, $toolsPath, $package, $project)

if (Get-Module | ?{ $_.Name -eq 'EntityFrameworkViewMigrations' })
{
    Remove-Module EntityFrameworkViewMigrations
}

Import-Module (Join-Path $toolsPath EntityFrameworkViewMigrations.psd1)