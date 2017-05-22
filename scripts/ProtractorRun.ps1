#Note, the working directory for this script must be set to the UI test project folder
param($siteUrl)
#$siteUrl = "http://ho-ms-qa1/well/dashboard/"

cls
function RunProtractorTests
{
	$result = & node_modules\.bin\protractor conf.js --params.baseUrl=$siteUrl
	$code = $LASTEXITCODE
	$exitC = 1
	if ($code -eq 0 -or $code -eq 110 -or $code -eq 120 -or $code -eq 210 -or $code -eq 430)
	{
		$exitC = 0
	}
    Write-Host $result
    $result > ProtractorOutput.txt	
	return $exitC
}
$exitCode = RunProtractorTests

Write-Host "Exiting with code $exitCode"

exit $exitCode