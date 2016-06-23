param($siteUrl)

#$siteUrl = "http://ho-ms-qa1/well/dashboard/"

cls
function RunProtractorTests
{
	$fileExe = "..\src\4. Test\Well.Test.UI\node_modules\.bin\protractor"
	$result = & $fileExe "..\src\4. Test\Well.Test.UI\conf.js" --params.baseUrl=$siteUrl
	$code = $LASTEXITCODE
	$exitC = 1
	if ($code -eq 0 -or $code -eq 110 -or $code -eq 120 -or $code -eq 210 -or $code -eq 430)
	{
		$exitC = 0
	}
    $result > ..\ProtractorOutput.txt
	Write-Host $result
	return $exitC
}
$exitCode = RunProtractorTests

Write-Host "Exiting with code $exitCode"

exit $exitCode