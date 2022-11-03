param(
	$LibraryFolder,
	$OutputFolder,
    $NunitConsoleFolder,
    $DebugFileFolder,
    $DebugFunction
)
BEGIN{
    #Prepare log file to write the result
	$debugFilePath = $DebugFileFolder + "powershell-export_2.txt"
	if(!(Test-Path $debugFilePath))
    {
		$debugFile = New-Item -Path $debugFilePath -ItemType File
	}
	else
	{ 
		$debugFile = Get-Item -Path $debugFilePath
	}
	"Starting Export : " + (Get-Date) | Out-File $debugFile -Append
    "Debug folder : " + $debugFilePath | Out-File $debugFile -Append
}
PROCESS{
    $nunitConsoleDir = $NunitConsoleFolder + "nunit3-console.exe"

    $dllDir= $LibraryFolder + "DynoMapper_Selenium.dll"

    Start-Process -FilePath $nunitConsoleDir -ArgumentList "--test=$DebugFunction $dllDir" -WindowStyle Hidden
    $result = @{}
    $result.Add("Status","0")
    $result
}
END{
	"End Export : " + (Get-Date) | Out-File $debugFile -Append
}