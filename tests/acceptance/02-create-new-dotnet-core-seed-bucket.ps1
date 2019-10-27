function CreateNewDotNetCoreSeedBucket($testDirectoryName) {
    StepName "Create New .NET Core Seed Bucket"

    CreateNewDotNetCoreSeedBucketCore $testDirectoryName
}

function CreateNewDotNetCoreSeedBucketCore($testDirectoryName) {
    Info "Creating new .NET Core Seed Bucket in '$testDirectoryName'."

    nseed new --solution $testDirectoryName//Solution.sln --framework netcoreapp2.0
    Write-Host

    ExitIfError
}