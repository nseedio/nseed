param(
    [string]$Version=(Read-Host "NSeed CLI Tool Version")
)

. (Join-Path $PSScriptRoot "common-functions.ps1")
. (Join-Path $PSScriptRoot "00-install-nseed-cli-tool.ps1")
. (Join-Path $PSScriptRoot "01-copy-initial-solution.ps1")
. (Join-Path $PSScriptRoot "02-create-new-dotnet-core-seed-bucket.ps1")

$error.clear()

Info "Running acceptance tests for the NSeed CLI Tool v$Version."

InstallNSeedCliTool $Version

$testDirectoryName = "test-v$Version"

CopyInitialSolution $testDirectoryName

CreateNewDotNetCoreSeedBucket $testDirectoryName