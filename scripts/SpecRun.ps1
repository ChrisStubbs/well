param($profileName, $testPath, $toolsPath, $projectName)

#.\SpecRun.ps1 "QA1" "C:\dev\od\CurrentSprint-Upgrade\src\Tests\BDD\bin\Release" "C:\dev\od\CurrentSprint-Upgrade\tools\SpecRun.Runner.1.2.0"

#$HtmlName = $profileName.Substring(0,$profileName.Length-4)

$HtmlName = $profileName

cls
function ExecuteSpecRunTests
{
	$fileExe = $toolsPath + "\tools\SpecRun.exe"
	$result = & $fileExe buildserverrun "$testPath\$profileName.srprofile" /log:specrun.log
	$code = $LASTEXITCODE
	$exitC = 1
	if ($code -eq 0 -or $code -eq 110 -or $code -eq 120 -or $code -eq 210 -or $code -eq 430)
	{
		$exitC = 0
	}
	Write-Host $result
	return $exitC
}
$seleniumExitCode = ExecuteSpecRunTests

Write-Host "Exiting selenium with code $seleniumExitCode"


$msis = [IO.Directory]::GetFiles($testPath, "*$HtmlName*.html")

foreach($msi in $msis) {
	
	rename-item $msi -NewName "index.html"
}

exit $seleniumExitCode