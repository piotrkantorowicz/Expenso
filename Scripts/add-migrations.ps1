<#
.SYNOPSIS
A script to run migrations for a .NET project.

.DESCRIPTION
This script runs a migration for a .NET project using the Entity Framework Core CLI. 
It takes in parameters for the migration name, DbContext, project directory, output directory, and optionally the startup project directory.

.PARAMETER migrationName
The name of the migration to be added.

.PARAMETER dbContext
The name of the DbContext to be used.

.PARAMETER projectDir
The directory of the project where the migration will be added.

.PARAMETER outputDir
The directory where the migration files will be outputted.

.PARAMETER startUpProjectDir
The directory of the startup project.

.PARAMETER Help
Displays the help documentation for the script

.EXAMPLE
.\run-migrations.ps1 -migrationName "InitialMigration" -dbContext "MyDbContext" -projectDir "MyProject" -outputDir "Migrations"
#>

param (
    [string]$migrationName,
    [string]$dbContext,  
    [string]$projectDir,    
    [string]$outputDir,
    [string]$startUpProjectDir,
    [switch]$Help
)

if ($Help) {
    Get-Help $MyInvocation.MyCommand.Definition -Full
    exit
}

if (!$migrationName -or !$dbContext -or !$projectDir -or !$outputDir -or !$startUpProjectDir) {
    throw "The parameters -migrationName, -dbContext, -projectDir, -outputDir and -startUpProjectDir are required."
}

$initialLocation = Get-Location

Set-Location $projectDir

dotnet ef migrations add $migrationName --output-dir $outputDir --context $dbContext -- $startUpProjectDir

Set-Location $initialLocation