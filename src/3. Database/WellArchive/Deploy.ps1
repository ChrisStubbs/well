# Internal Variables
$databaseName = $OctopusParameters["DatabaseName"]
$contentPath  = (Join-Path $OctopusOriginalPackageDirectoryPath "content")
$deployPath  = (Join-Path $contentPath "deploy")
$dbFileName = (Get-ChildItem $deployPath\*.dacpac -Name | Select-Object -First 1)
$dbFilePath = (Join-Path $deployPath $dbFileName)
$dacpac = "Well-Archive.dacpac"
$dacpac = (Join-Path $deployPath $dacpac)
$deployScriptName = $databaseName + "_db-$(((get-date).ToUniversalTime()).ToString("yyyyMMddHHmmss")).sql"
$deployScriptName = (Join-Path $deployPath $deployScriptName)
$publishSettingsPath = (Join-Path $deployPath "Well-Archive")
$publishSettingsPath = $publishSettingsPath + ".xml"
$baseSqlPackagePath = "${Env:ProgramFiles(x86)}\Microsoft SQL Server\"
$sqlInstanceName = $OctopusParameters["SqlInstance"]

$roundhouseDir = (Join-Path $OctopusOriginalPackageDirectoryPath "content\ManualStep")
$roundhouseExe = (Join-Path $roundhouseDir "roundhouse.0.8.6\bin\rh.exe")

if (Test-Path  ($baseSqlPackagePath + "\120\DAC\bin\SqlPackage.exe"))
{
	$sqlPackageDir = (Join-Path $baseSqlPackagePath "\120\DAC\bin")
}
elseif (Test-Path  ($baseSqlPackagePath + "\110\DAC\bin\SqlPackage.exe"))
{
	$sqlPackageDir = (Join-Path $baseSqlPackagePath "\110\DAC\bin")
}

$sqlPackageExe = (Join-Path $sqlPackageDir "SqlPackage.exe")

$machineName = $OctopusParameters["Octopus.Machine.Name"]
#$machineName = "."

Write-Host "databaseName     : " $databaseName
Write-Host "contentPath      : " $contentPath
Write-Host "dbFileName       : " $dbFileName
Write-Host "dbFilePath       : " $dbFilePath
Write-Host "Dacpac           : " $dacpac
Write-Host "Output script    : " $deployScriptName
Write-Host "Publish file     : " $publishSettingsPath
Write-Host "Package dir      : " $sqlPackageDir
Write-Host "Package exe      : " $sqlPackageExe
Write-Host "Machine name     : " $machineName
Write-Host "SQL Instance     : " $sqlInstanceName

#Run the roundhouse scripts
Set-Location $roundhouseDir
& $roundhouseExe /d=$databaseName /f=scripts /s=$machineName\$sqlInstanceName --dnc=true --silent=true

# Run the deployment tool
Set-Location $sqlPackageDir
& $sqlPackageExe /Profile:$publishSettingsPath /TargetDatabaseName:$databaseName /TargetServername:$machineName\$sqlInstanceName /Action:Script /SourceFile:"$dacpac" /outputpath:$deployScriptName | Write-Host

# Attach the generated SQL script as an artifact
New-OctopusArtifact $deployScriptName

Set-Location $sqlPackageDir
& $sqlPackageExe /Profile:$publishSettingsPath /TargetDatabaseName:$databaseName /TargetServername:$machineName\$sqlInstanceName /Action:Publish /SourceFile:"$dacpac" | Write-Host