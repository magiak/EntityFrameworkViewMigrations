# Copyright (c) Designeo s.r.o.  All rights reserved.

<#
.SYNOPSIS
    Scaffolds a migration script for any pending model changes.

.DESCRIPTION
    Scaffolds a new migration script and adds it to the project.

.PARAMETER Name
    Specifies the name of the custom script.

.PARAMETER Force
    Specifies that the migration user code be overwritten when re-scaffolding an
    existing migration.

.PARAMETER ProjectName
    Specifies the project that contains the migration configuration type to be
    used. If omitted, the default project selected in package manager console
    is used.

.PARAMETER StartUpProjectName
    Specifies the configuration file to use for named connection strings. If
    omitted, the specified project's configuration file is used.

.PARAMETER ConfigurationTypeName
    Specifies the migrations configuration to use. If omitted, migrations will
    attempt to locate a single migrations configuration type in the target
    project.

.PARAMETER ConnectionStringName
    Specifies the name of a connection string to use from the application's
    configuration file.

.PARAMETER ConnectionString
    Specifies the the connection string to use. If omitted, the context's
    default connection will be used.

.PARAMETER ConnectionProviderName
    Specifies the provider invariant name of the connection string.

.PARAMETER IgnoreChanges
    Scaffolds an empty migration ignoring any pending changes detected in the current model.
    This can be used to create an initial, empty migration to enable Migrations for an existing
    database. N.B. Doing this assumes that the target database schema is compatible with the
    current model.

.PARAMETER AppDomainBaseDirectory
    Specifies the directory to use for the app-domain that is used for running Migrations
    code such that the app-domain is able to find all required assemblies. This is an
    advanced option that should only be needed if the solution contains	several projects 
    such that the assemblies needed for the context and configuration are not all
    referenced from either the project containing the context or the project containing
    the migrations.
	
.EXAMPLE
	Add-Migration First
	# Scaffold a new migration named "First"
	
.EXAMPLE
	Add-Migration First -IgnoreChanges
	# Scaffold an empty migration ignoring any pending changes detected in the current model.
	# This can be used to create an initial, empty migration to enable Migrations for an existing
	# database. N.B. Doing this assumes that the target database schema is compatible with the
	# current model.

#>
function Add-ViewMigration
{
    [CmdletBinding(DefaultParameterSetName = 'ConnectionStringName')]
        param (
            [parameter(Position = 0,
                Mandatory = $true)]
            [string] $Name,
            [parameter(Mandatory = $true)]
            [string] $ViewName,
            [switch] $Force,
            [string] $ProjectName,
            [string] $StartUpProjectName,
            [string] $ConfigurationTypeName,
            [parameter(ParameterSetName = 'ConnectionStringName')]
            [string] $ConnectionStringName,
            [parameter(ParameterSetName = 'ConnectionStringAndProviderName',
                Mandatory = $true)]
            [string] $ConnectionString,
            [parameter(ParameterSetName = 'ConnectionStringAndProviderName',
                Mandatory = $true)]
            [string] $ConnectionProviderName,
            [switch] $IgnoreChanges,
		    [string] $AppDomainBaseDirectory)

    Add-Migration $Name
    Add-CommandTypes

    $project = Get-Project
    $command = New-Object EntityFrameworkViewMigrations.PowerShellCommands.Commands.AddViewDbMigration($dte, $project)
    $command.SqlViewName = $ViewName
    $command.Execute()
}

function Add-ModelChangeOnlyMigration
{
    [CmdletBinding(DefaultParameterSetName = 'ConnectionStringName')]
        param (
            [parameter(Position = 0,
                Mandatory = $true)]
            [string] $Name,
            [switch] $Force,
            [string] $ProjectName,
            [string] $StartUpProjectName,
            [string] $ConfigurationTypeName,
            [parameter(ParameterSetName = 'ConnectionStringName')]
            [string] $ConnectionStringName,
            [parameter(ParameterSetName = 'ConnectionStringAndProviderName',
                Mandatory = $true)]
            [string] $ConnectionString,
            [parameter(ParameterSetName = 'ConnectionStringAndProviderName',
                Mandatory = $true)]
            [string] $ConnectionProviderName,
            [switch] $IgnoreChanges,
		    [string] $AppDomainBaseDirectory)

    Add-Migration $Name
    Add-CommandTypes

    $project = Get-Project
    $command = New-Object EntityFrameworkViewMigrations.PowerShellCommands.Commands.AddModelChangeOnlyDbMigration($dte, $project)
    $command.Execute()
}

function Add-SeedMigration
{
    [CmdletBinding(DefaultParameterSetName = 'ConnectionStringName')]
        param (
            [parameter(Position = 0,
                Mandatory = $true)]
            [string] $Name,
            [switch] $Force,
            [string] $ProjectName,
            [string] $StartUpProjectName,
            [string] $ConfigurationTypeName,
            [parameter(ParameterSetName = 'ConnectionStringName')]
            [string] $ConnectionStringName,
            [parameter(ParameterSetName = 'ConnectionStringAndProviderName',
                Mandatory = $true)]
            [string] $ConnectionString,
            [parameter(ParameterSetName = 'ConnectionStringAndProviderName',
                Mandatory = $true)]
            [string] $ConnectionProviderName,
            [switch] $IgnoreChanges,
		    [string] $AppDomainBaseDirectory)

    Add-Migration $Name
    Add-CommandTypes

    $project = Get-Project
    $command = New-Object EntityFrameworkViewMigrations.PowerShellCommands.Commands.AddSeedDbMigration($dte, $project)
    $command.Execute()
}

function Get-PackageInstallPath($package)
{
    $componentModel = Get-VsComponentModel
    $packageInstallerServices = $componentModel.GetService([NuGet.VisualStudio.IVsPackageInstallerServices])

    $vsPackage = $packageInstallerServices.GetInstalledPackages() | ?{ $_.Id -eq $package.Id -and $_.Version -eq $package.Version }
    
    return $vsPackage.InstallPath
}

function Copy-DLL
{
	[CmdletBinding(DefaultParameterSetName = 'ConnectionStringName')]
        param (
            [parameter(Position = 0,
                Mandatory = $true)]
            [string] $Version)

    Write-Host 'Please build EntityFrameworkViewMigrations.PowerShellCommands project'
    Read-Host -Prompt 'Hit enter if everything was done'
    $dllPath = 'C:\Users\lukas\Source\Repos\EntityFrameworkViewMigrations\EntityFrameworkViewMigrations.PowerShellCommands\bin\Debug\EntityFrameworkViewMigrations.PowerShellCommands.dll'
    $targetPath = "C:\Users\lukas\Source\Repos\EntityFrameworkViewMigration.Sample\packages\EntityFrameworkViewMigrations.$version\lib\net452"
    Copy-Item $dllPath $targetPath -force
}

function Write-Debug($text)
{
    #Write-Host "[DEBUG:] $text" -foreground Yellow
}

function Add-CommandTypes
{
    $projectObj = Get-Project
    Write-Debug "Project path: $projectObj.FullName"

    $package = Get-Package -ProjectName $projectObj.FullName | ?{ $_.Id -eq 'EntityFrameworkViewMigrations' }
    $packagePath = Get-PackageInstallPath $package
    Write-Debug "Package path: $packagePath"

    #C:\Users\lukas\Source\Repos\EntityFrameworkViewMigration.Sample\packages\EntityFrameworkViewMigrations.{version}\lib\net452'
    $efvDllPath = [io.path]::combine($packagePath, 'lib', 'net452', 'EntityFrameworkViewMigrations.dll')
    $pscDllPath = [io.path]::combine($packagePath, 'lib', 'net452', 'EntityFrameworkViewMigrations.PowerShellCommands.dll')
    
    Write-Debug "EntityFrameworkViewMigrations.dll path: $efvDllPath" -foreground Yellow
    Write-Debug "EntityFrameworkViewMigrations.PowerShellCommands.dll path: $pscDllPath" -foreground Yellow

    Add-Type -Path $efvDllPath
    Add-Type -Path $pscDllPath
}

Export-ModuleMember @('Add-ViewMigration', 'Add-ModelChangeOnlyMigration', 'Add-SeedMigration', 'Copy-DLL')