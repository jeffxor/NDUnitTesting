param 
( 
	[ValidateNotNullOrEmpty()][string]$Activity = $(throw "Required parameter missing: Activity"),
	[string]$Version,
    [string]$Template,
    [string]$File,
	[string]$ManifestPath,
    [string]$Username,
    [string]$Password,
	[string]$NolioServer,
	[string]$Environment,
	[string]$Release,
	[string]$Application,
	[string]$BuildDropLocation,
	[ValidateSet("true","false")][string]$DoStepValidation = "true",
	[ValidateSet("Major","Minor")][string]$ReleaseType = "Minor",
	[Int]$Timeout = 120,
	[Int]$TimeoutInterval = 10,
	[Switch]$DebugMode
)


function RunTemplate() {

	$returnArray = @()

	$UsernamePasswordEncoded = ConvertToBase64($User + ":" + $Password)
	$cred = New-Object System.Net.NetworkCredential($Username, $Password)
	$url = 'http://' + $NolioServer + ':8080/datamanagement/a/api/run-template'
	$request = [Net.WebRequest]::Create($url)

	$request.ServicePoint.Expect100Continue = $false
	$request.PreAuthenticate = $true

	$request.Credentials = $cred
	$request.Headers.Add("AUTHORIZATION", "Basic " + $UsernamePasswordEncoded); # user:pass encoded in base 64
	$request.ContentType = "application/json"
	$request.Method = "POST"
	$fileName = [io.path]::GetFileNameWithoutExtension("$File")
	$fileExtension = [io.path]::GetExtension("$File")
	$fileExtension = $fileExtension -replace "\.", ""

	$propertiesData = (New-Object PSObject | 
		Add-Member -PassThru NoteProperty Release-Manifest-File-Name $fileName |
		Add-Member -PassThru NoteProperty Release-Manifest-File-Extension $fileExtension |
		Add-Member -PassThru NoteProperty Release-Manifest-File-Location $ManifestPath
	) | ConvertTo-JSON

	$data = (New-Object PSObject |
		Add-Member -PassThru NoteProperty environment $Environment |
		Add-Member -PassThru NoteProperty template $Template |
		Add-Member -PassThru NoteProperty release $Release |
		Add-Member -PassThru NoteProperty application $Application |
		Add-Member -PassThru NoteProperty version $Version |
		Add-Member -PassThru NoteProperty doStepsValidation $DoStepValidation |
		Add-Member -PassThru NoteProperty releaseType $ReleaseType |
		Add-Member -PassThru NoteProperty properties propertiesData
	) | ConvertTo-JSON

	$data = $data -replace "propertiesData", $propertiesData
	$data = $data -replace """{", "{"
	$data = $data -replace "}""", "}"
	
	$data
	$bytes = [System.Text.Encoding]::ASCII.GetBytes($data)

	$request.ContentLength = $bytes.Length

	$requestStream = [System.IO.Stream]$request.GetRequestStream()
	$requestStream.write($bytes, 0, $bytes.Length)

	$response = $request.GetResponse()

	[IO.Stream] $stream = $response.GetResponseStream()
	[IO.StreamReader] $reader = New-Object IO.StreamReader($stream)
	[string] $output = $reader.readToEnd()
	$stream.flush()
	$stream.close()

	$responseOutput = $output | ConvertFrom-Json
	
	if(!$responseOutput.result)
	{
		$return = 1
		$returnError = $responseOutput.description	
	}
	else
	{
		$return = 0
		$returnError = ""
	}
	
	$returnArray += $return
	$returnArray += $responseOutput
	$returnArray += $returnError	
	return $returnArray;
}



function CheckReleaseStatus() {

	$returnArray = @()
	$result = ""
	$returnError = ""
	$return = 1
	$UsernamePasswordEncoded = ConvertToBase64($User + ":" + $Password)
	$cred = New-Object System.Net.NetworkCredential($Username, $Password)
	$url = 'http://' + $NolioServer + ':8080/datamanagement/a/api/release-status'
	$timeout = $Timeout # Number of iteration
	$timeoutInterval = $TimeoutInterval # Sec
	$i=0;

	while ($i -le $timeout) {

		$request = [Net.WebRequest]::Create($url)
		$request.ServicePoint.Expect100Continue = $false
		$request.PreAuthenticate = $true
		$request.Credentials = $cred
		$request.Headers.Add("AUTHORIZATION", "Basic " + $UsernamePasswordEncoded); # user:pass encoded in base 64
		$request.ContentType = "application/json"
		$request.Method = "POST"

		$data = (New-Object PSObject |
		Add-Member -PassThru NoteProperty environment $Environment |
		Add-Member -PassThru NoteProperty release $Release |
		Add-Member -PassThru NoteProperty application $Application |
		Add-Member -PassThru NoteProperty version $Version
		) | ConvertTo-JSON


		$bytes = [System.Text.Encoding]::ASCII.GetBytes($data)
		$request.ContentLength = $bytes.Length
		$requestStream = [System.IO.Stream]$request.GetRequestStream()
		$requestStream.write($bytes, 0, $bytes.Length)

		$response = $request.GetResponse()
		$stream = $response.GetResponseStream()
		$reader = New-Object IO.StreamReader($stream)
		$result = $reader.readToEnd() | ConvertFrom-JSON

		$stream.flush()
		$stream.close()

		#Write-Host "status $result.status"

		if ($result.status -eq "Finished")
		{   		
			$return = 0
			$returnError = ""
			break
		}
		elseif ($result.status -eq "Failed")
		{
			$return = 1
			$returnError = "Release deployment failed."
			break
		}
		elseif ($result.status -eq "Canceled")
		{
			$return = 1
			$returnError = "Release deployment cancelled."
			break
		}
		else
		{
			$waittime = $i * $timeoutInterval
			
			if ($i -ge $timeout)
			{
				$return = 1
				$returnError = "Release deployment timed out."
				break
			}
			else
			{
				#Write-Host "Release deployment running for $waittime sec (max "($i * $timeoutInterval)"). Checking again in 10 seconds..."
				Start-Sleep -Second $timeoutInterval
				$i++
			}
		}

	}

	$returnArray += $return
	$returnArray += $result
	$returnArray += $returnError	
	return $returnArray;
}


function ConvertToBase64($string) {
   $bytes  = [System.Text.Encoding]::UTF8.GetBytes($string);
   $encoded = [System.Convert]::ToBase64String($bytes); 

   return $encoded;
}

function ConvertFromBase64($string) {
   $bytes  = [System.Convert]::FromBase64String($string);
   $decoded = [System.Text.Encoding]::UTF8.GetString($bytes); 

   return $decoded;
}

function UpdateManifestVersion($path, $file, $version) {

	$returnArray = @()

	
	$currentbamboobuild = ${version}.split("-")
	#$buildlocation = $currentbamboobuild[0]+"-"+$currentbamboobuild[1]+"/"+$currentbamboobuild[2]+"/build-"+$currentbamboobuild[3]

	$buildlocation = $buildvariables[0]+"-"+$buildvariables[1]+"/shared/build-"+$buildvariables[3]

	$nullOut = (Get-Content $path\$file) | Foreach-Object {$_ -replace "%version%", ${version}} | Foreach-object {$_ -replace "%location%", ${buildlocation}}  |  Set-Content $path\$version.xml


	$returnArray += 0
	$returnArray += "$path\$version.xml created"
	$returnArray += ''	
	return $returnArray;
}


function MakeFilesWriteable($path)
{

	$nullOut = Get-Childitem -Include *.* $path -Recurse | % { $_.IsReadOnly = $false }

}

function MakeFileWriteable($file)
{

	$nullOut = Set-ItemProperty $file -name IsReadOnly -value $false

}


function Zip-Folders ($DropSiteFolder){
	$returnArray = @()

try{
Set-alias sz "$env:ProgramFiles\7-Zip\7z.exe"
if (-not (test-path "$env:ProgramFiles\7-Zip\7z.exe" )) { #Check if 7zip exist in the machine
$returnArray += "$env:ProgramFiles\7-Zip\7z.exe needed"
}
Write-Host "Testing path $DropSiteFolder"
if (test-path $DropSiteFolder){#Check if the folder location/name is valid
new-psdrive -name drop -PSProvider FileSystem -root $DropSiteFolder #Create a new PSDrive for the drop site location
}
else {
$returnArray += "Can't create a PSDrive for the location $DropSiteFolder specified."
}
set-location drop: #Change current directory to the drop location
try{
$returnArray += "Compressing using 7zip"
 
#dir | Where-Object { $_.PSIsContainer } | ForEach-Object { C:\"Program Files"\7-zip\7z.exe a -mx9 "$_.zip" $_.FullName }
#see http://www.dotnetperls.com/7-zip-examples
#http://www.solo-technology.com/blog/2007/12/14/7-zip-compression-vs-speed/
#Create zip file for each of the folder
#http://superuser.com/questions/340046/create-an-archive-from-a-directory-without-the-directory-name-being-added-to-the
 
dir | Where-Object { $_.PSIsContainer } | ForEach-Object {sz a -mx9 -mmt="on" "$_.zip" ".\$_\*" }

}
catch{
$returnArray += "Error during the archive operation"
}
}
catch{
$returnArray +=  $error[0]
}

return $returnArray;
}

switch($Activity)
{
	
	"CheckReleaseStatus" { 
	
		$return = CheckReleaseStatus
	
	}
	
	
	"PrepareManifest" { 
	
		$return = UpdateManifestVersion $ManifestPath $File $Version
	
	}
	
	"RunTemplate" { 
	
		$return = RunTemplate
	
	}
	
	"Zip-Folders" {
	
		$return = Zip-Folders $BuildDropLocation
	}
	default {
		$returnArray = @()
		$returnArray += 1
		$returnArray += ""
		$return = $returnArray += "Invalid Activity parameter"	
	}

}

$return
