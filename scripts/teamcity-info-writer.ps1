param($BuildRoot, $SpecRunLog, $ProtractorLogPath)

#$BuildRoot = "C:\dev\well"
#$SpecRunLog = "..\specrun.log"
#$ProtractorLogPath = "..\ProtractorOutput.txt"

Write-Host $BuildRoot
Write-Host $SpecRunLog

$UITotals = Get-Content $ProtractorLogPath | Where { $_ -match "\b(?<total>[0-9]+) spec, (?<failed>[0-9]+) failures\b" } | 
foreach { new-object PSObject –prop @{ Total=$matches['total']; Failed=$matches['failed'] }   }

Write-Host "UI Total: " $UITotals[0].Total
Write-Host "UI Failed: " $UITotals[0].Failed

$BDDTotal = Get-Content $SpecRunLog | Where-Object { $_.StartsWith("Total:") }
$BDDSucceeded = Get-Content $SpecRunLog | Where-Object { $_.StartsWith("Succeeded:") }
$BDDIgnored = Get-Content $SpecRunLog | Where-Object { $_.StartsWith("Ignored:") }
$BDDPending = Get-Content $SpecRunLog | Where-Object { $_.StartsWith("Pending:") }
$BDDSkipped = Get-Content $SpecRunLog | Where-Object { $_.StartsWith("Skipped:") }
$BDDFailed = Get-Content $SpecRunLog | Where-Object { $_.StartsWith("Failed:") }

Write-Host "BDD $BDDTotal"
Write-Host "BDD $BDDSucceeded"
Write-Host "BDD $BDDIgnored"
Write-Host "BDD $BDDPending"
Write-Host "BDD $BDDSkipped"
Write-Host "BDD $BDDFailed"

$Result = "FAILURE"
If ($BDDFailed.Contains("Failed: 0") -and $UITotals[0].Failed -eq 0)
{
	$Result = "SUCCESS"
}

# this is where the document will be saved:
$Path = "$BuildRoot\teamcity-info.xml"
 
# get an XMLTextWriter to create the XML
$XmlWriter = New-Object System.XMl.XmlTextWriter($Path,$Null)
 
# choose a pretty formatting:
$xmlWriter.Formatting = 'Indented'
$xmlWriter.Indentation = 1
$XmlWriter.IndentChar = "`t"
 
# write the header
$xmlWriter.WriteStartDocument()
 
# create root element "build" and add some attributes to it
$xmlWriter.WriteStartElement('build')
$XmlWriter.WriteAttributeString('number', '1.0.{build.number}')

	$xmlWriter.WriteStartElement('statusInfo')
    $XmlWriter.WriteAttributeString('status', $Result)
    	$XmlWriter.WriteStartElement('text')
		$XmlWriter.WriteAttributeString('action', 'append')
		$XmlWriter.WriteRaw("UI Total:" + $UITotals[0].Total + " Failed:" + $UITotals[0].Failed)
		$xmlWriter.WriteEndElement()

		$XmlWriter.WriteStartElement('text')
		$XmlWriter.WriteAttributeString('action', 'append')
		$XmlWriter.WriteRaw("BDD: " + $BDDTotal + " " + $BDDFailed)
		$xmlWriter.WriteEndElement()
	$xmlWriter.WriteEndElement()

# close the "build" node:
$xmlWriter.WriteEndElement()
 
# finalize the document:
$xmlWriter.WriteEndDocument()
$xmlWriter.Flush()
$xmlWriter.Close()

 