$MsBuild = ${env:ProgramFiles(x86)} + "\MSBuild\14.0\Bin\MSBuild.exe";   

#get the script path
$currentDir = [string]$PSCommandPath
$currentDir = $currentDir -replace "\\BuildSolution.ps1", ""

$SlnFilePath = $currentDir + "\Well.All.sln";  
$Configuration = "Debug"
$nuget = $currentDir + "\..\bin\nuget\nuget.exe"

Write-Host "Pull lastes sources"
$BuildArgs = @{
	FilePath = "git"            
	ArgumentList = "pull"                      
	Wait = $true    
	#WindowStyle = "Hidden"       
}                        
Start-Process @BuildArgs    
Write-Host "Code up to date" 

Write-Host "Cleaning"     

$BuildArgs = @{
	FilePath = $MsBuild            
	ArgumentList = $SlnFilePath, "/t:clean", ("/p:Configuration=" + $Configuration), "/v:minimal"                      
	Wait = $true    
	#WindowStyle = "Hidden"       
}                        
Start-Process @BuildArgs      
			     
Write-Host "Well Clean"   

Write-Host "Restore nuget packages"
     
$BuildArgs = @{
	FilePath = $nuget            
	ArgumentList = "restore", $SlnFilePath                    
	Wait = $true    
	#WindowStyle = "Hidden"       
}                        
Start-Process @BuildArgs     
      
Write-Host "Packages restored"  

Write-Host "Build Solution"  

# Prepare the Args for the actual build            
$BuildArgs = @{            
	FilePath = $MsBuild            
	ArgumentList = $SlnFilePath, "/t:rebuild", ("/p:Configuration=" + $Configuration), "/v:minimal" 
	#Wait = $true            
	#WindowStyle = "Hidden"            
}            

# Start the build            
Start-Process @BuildArgs 
   
Write-Host "Well built"

Write-Host "Run webpack"
     
$BuildArgs = @{
	FilePath = "npm"            
	ArgumentList = "run", "buildProduction"                    
	#Wait = $true    
	#WindowStyle = "Hidden"  
	WorkingDirectory = $currentDir + "\1. Layers\1.1 Presentation\Well.Dashboard"
}                        
Start-Process @BuildArgs     

Write-Host "Webpack ran"  

$databaseName = "Well"
$machineName = "."
$sqlInstanceName = "SQLSERVER2014"
$roundhouseDir = (Join-Path $currentDir "3. Database\Well\ManualStep\")
$roundhouseExe = (Join-Path $roundhouseDir "roundhouse.0.8.6\bin\rh.exe")
$baseSqlPackagePath = "${Env:ProgramFiles(x86)}\Microsoft SQL Server\"

if (Test-Path  ($baseSqlPackagePath + "\120\DAC\bin\SqlPackage.exe"))
{
	$sqlPackageDir = (Join-Path $baseSqlPackagePath "\120\DAC\bin")
}
elseif (Test-Path  ($baseSqlPackagePath + "\110\DAC\bin\SqlPackage.exe"))
{
	$sqlPackageDir = (Join-Path $baseSqlPackagePath "\110\DAC\bin")
}

$sqlPackageExe = (Join-Path $sqlPackageDir "SqlPackage.exe")
$publishSettingsPath = (Join-Path $currentDir "3. Database\Well\Well.xml")
$dacpac = (Join-Path $currentDir "3. Database\Well\bin\debug\Well.dacpac")

Write-Host $databaseName
Write-Host $machineName
Write-Host $sqlInstanceName
Write-Host $roundhouseDir
Write-Host $roundhouseExe
Write-Host $baseSqlPackagePath
Write-Host $sqlPackageDir
Write-Host $sqlPackageExe
Write-Host $publishSettingsPath
Write-Host $dacpac

Write-Host "Run the roundhouse scripts - Ignore erros if you don't hsave a Well DataBase" 
Set-Location $roundhouseDir
& $roundhouseExe /d=$databaseName /f=scripts /s=$machineName\$sqlInstanceName --dnc=true --silent=true
Write-Host "Roundhouse scripts ran"

Write-Host "Publish Database"  
Set-Location $sqlPackageDir
& $sqlPackageExe /Profile:$publishSettingsPath /TargetDatabaseName:$databaseName /TargetServername:$machineName\$sqlInstanceName /Action:Publish /SourceFile:"$dacpac" | Write-Host

Write-Host "Database Published" 

Write-Host "Done. Press any key to finish"  
Read-Host