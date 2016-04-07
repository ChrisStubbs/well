param($BuildRoot, $SpecRunLog)

Write-Host $BuildRoot
Write-Host $SpecRunLog

$Total = Get-Content $SpecRunLog | Where-Object { $_.StartsWith("Total:") }
$Succeeded = Get-Content $SpecRunLog | Where-Object { $_.StartsWith("Succeeded:") }
$Ignored = Get-Content $SpecRunLog | Where-Object { $_.StartsWith("Ignored:") }
$Pending = Get-Content $SpecRunLog | Where-Object { $_.StartsWith("Pending:") }
$Skipped = Get-Content $SpecRunLog | Where-Object { $_.StartsWith("Skipped:") }
$Failed = Get-Content $SpecRunLog | Where-Object { $_.StartsWith("Failed:") }

Write-Host "$Total"
Write-Host "$Succeeded"
Write-Host "$Ignored"
Write-Host "$Pending"
Write-Host "$Skipped"
Write-Host "$Failed"

If ($Failed.Contains("Failed: 0"))
{
	$Result = "SUCCESS"
}
Else
{
	$Result = "FAILURE"
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
		$XmlWriter.WriteRaw($Total)
		$xmlWriter.WriteEndElement()
		
		$XmlWriter.WriteStartElement('text')
		$XmlWriter.WriteAttributeString('action', 'append')
		$XmlWriter.WriteRaw($Succeeded)
		$xmlWriter.WriteEndElement()
		
		$XmlWriter.WriteStartElement('text')
		$XmlWriter.WriteAttributeString('action', 'append')
		$XmlWriter.WriteRaw($Ignored)
		$xmlWriter.WriteEndElement()
		
		$XmlWriter.WriteStartElement('text')
		$XmlWriter.WriteAttributeString('action', 'append')
		$XmlWriter.WriteRaw($Pending)
		$xmlWriter.WriteEndElement()	

		$XmlWriter.WriteStartElement('text')
		$XmlWriter.WriteAttributeString('action', 'append')
		$XmlWriter.WriteRaw($Skipped)
		$xmlWriter.WriteEndElement()		
		
		$XmlWriter.WriteStartElement('text')
		$XmlWriter.WriteAttributeString('action', 'append')
		$XmlWriter.WriteRaw($Failed)
		$xmlWriter.WriteEndElement()
	$xmlWriter.WriteEndElement()

# close the "build" node:
$xmlWriter.WriteEndElement()
 
# finalize the document:
$xmlWriter.WriteEndDocument()
$xmlWriter.Flush()
$xmlWriter.Close()

 