param(
    [string]$Version=(Read-Host "NSeed CLI Tool Version")
)

. (Join-Path $PSScriptRoot "common-functions.ps1")
. (Join-Path $PSScriptRoot "00A-check-minimum-required-powershell-version.ps1")
. (Join-Path $PSScriptRoot "00B-install-nseed-cli-tool.ps1")
. (Join-Path $PSScriptRoot "01-display-help.ps1")
. (Join-Path $PSScriptRoot "02-copy-initial-solution.ps1")
. (Join-Path $PSScriptRoot "03-create-new-dotnet-core-seed-bucket.ps1")
. (Join-Path $PSScriptRoot "04-create-new-dotnet-classic-seed-bucket.ps1")
. (Join-Path $PSScriptRoot "05-display-seed-bucket-information-for-dotnet-core-seed-bucket.ps1")
. (Join-Path $PSScriptRoot "06-display-seed-bucket-information-for-dotnet-classic-seed-bucket.ps1")

$error.clear()

Info "Running acceptance tests for the NSeed CLI Tool v$Version."

CheckMinimumRequiredPowerShellVersion "6.0.0.0"

InstallNSeedCliTool $Version

DisplayHelp

$testDirectoryName = "test-v$Version"

CopyInitialSolution $testDirectoryName

CreateNewDotNetCoreSeedBucket $testDirectoryName

CreateNewDotNetClassicSeedBucket $testDirectoryName

DisplaySeedBucketInformationForDotNetCoreSeedBucket $testDirectoryName

DisplaySeedBucketInformationForDotNetClassicSeedBucket $testDirectoryName