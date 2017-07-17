# Copyright (c) Designeo s.r.o.  All rights reserved.

$InitialDatabase = '0'

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

    Write-Host "Add EF view migration"
}

Export-ModuleMember @('Add-ViewMigration') -Variable InitialDatabase
