$MsBuild = ${env:ProgramFiles(x86)} + "\MSBuild\14.0\Bin\MSBuild.exe";   
# http://blog.davidebbo.com/2014/01/the-right-way-to-restore-nuget-packages.html
[string]$currentDir = Get-Location
$SlnFilePath = $currentDir + "\Well.All.sln";  
$Configuration = "Debug"
$nuget = $currentDir + "\..\bin\nuget\nuget.exe"

# Display Progress            
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






