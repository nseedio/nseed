function CreateNewDotNetCoreSeedBucket($testDirectoryName) {
    StepName "Create New .NET Core Seed Bucket"

    CreateNewDotNetCoreSeedBucketCore $testDirectoryName
}

function CreateNewDotNetCoreSeedBucketCore($testDirectoryName) {
    Step "Creating new .NET Core Seed Bucket in '$testDirectoryName'."

    nseed new --solution $testDirectoryName//Solution.sln --framework netcoreapp2.0
    Write-Host

    ExitIfError

    AssertTestIsSuccessful "Open the Solution.sln and check the following:`n - The solution builds without errors.`n - The Program.cs and SampleSeed.cs look as expected.`nDoes the created project look like expected?"
}