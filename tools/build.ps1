$scriptDir = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent
Set-Location $scriptDir
[Environment]::CurrentDirectory = $scriptDir

if((Test-Path "nuget.exe") -eq $false) {
  invoke-webrequest "https://nuget.org/nuget.exe" -outfile "nuget.exe"  
} 

if((Test-Path ./Fake) -eq $false) {
  & nuget install FAKE -ExcludeVersion -OutputDirectory .
}

if((Test-Path ./xunit.runner.console) -eq $false) {
  & nuget install xunit.runner.console -ExcludeVersion -OutputDirectory .
}

& ./Fake/tools/Fake.exe ./build.fsx
