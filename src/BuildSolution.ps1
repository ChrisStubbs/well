$MsBuild = ${env:ProgramFiles(x86)} + "\MSBuild\14.0\Bin\MSBuild.exe";   
[string]$currentDir = Get-Location
$SlnFilePath = $currentDir + "\Well.All.sln";  
$Configuration = "Debug"
$nuget = $currentDir + "\..\bin\nuget\nuget.exe"
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
			
# Display Progress            
Write-Host "Well Clean"   

# Display Progress            
Write-Host "Restore nuget packages"
     
$BuildArgs = @{
	FilePath = $nuget            
	ArgumentList = "restore", $SlnFilePath                    
	Wait = $true    
	#WindowStyle = "Hidden"       
}                        
Start-Process @BuildArgs     

# Display Progress            
Write-Host "Packages restored"  

# Display Progress            
Write-Host "Build Solution"  

# Prepare the Args for the actual build            
$BuildArgs = @{            
	FilePath = $MsBuild            
	ArgumentList = $SlnFilePath, "/t:rebuild", ("/p:Configuration=" + $Configuration), "/v:minimal" 
	Wait = $true            
	#WindowStyle = "Hidden"            
}            

# Start the build            
Start-Process @BuildArgs 
   
Write-Host "Well built"

#########################

# Display Progress            
Write-Host "Run webpack"
     
$BuildArgs = @{
	FilePath = "npm"            
	ArgumentList = "run", "buildProduction"                    
	Wait = $true    
	#WindowStyle = "Hidden"  
	WorkingDirectory = $currentDir + "\1. Layers\1.1 Presentation\Well.Dashboard"
}                        
Start-Process @BuildArgs     

# Display Progress            
Write-Host "Webpack ran"  

#########################

# Display Progress            
Write-Host "Publish Database"  
Write-Host "Run the roundhouse scripts" 
Set-Location $roundhouseDir
& $roundhouseExe /d="Well" /f=scripts /s=".\sqlserver2014" --dnc=true --silent=true

Write-Host "Run the deployment tool"
Set-Location $sqlPackageDir
& $sqlPackageExe /Profile:$publishSettingsPath /TargetDatabaseName:$databaseName /TargetServername:$machineName\$sqlInstanceName /Action:Publish /SourceFile:"$dacpac" | Write-Host

