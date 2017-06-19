param($profileName, $testPath, $toolsPath, $projectName)

#.\SpecRun.ps1 "staging1" "C:\dev\well\src\4. Test\BDD\bin\Release" "C:\dev\well\tools\SpecRun.Runner.1.2.0"

$HtmlName = $profileName

cls
function ExecuteSpecRunTests
{
	$fileExe = $toolsPath + "\tools\SpecRun.exe"
	$result = & $fileExe buildserverrun "$testPath\$profileName.srprofile" /log:specrun.log
	$code = $LASTEXITCODE
	$exitC = 1
	if ($code -lt 440)
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