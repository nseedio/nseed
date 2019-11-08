function CreateNewDotNetCoreSeedBucket($testDirectoryName) {
    TestName "Create New .NET Core Seed Bucket"

    CreateNewDotNetCoreSeedBucketCore $testDirectoryName
}

function CreateNewDotNetCoreSeedBucketCore($testDirectoryName) {
    $action = { nseed new --solution $testDirectoryName//Solution.sln --framework netcoreapp2.0 }

    $message = "Open the Solution.sln and check the following:`n - The solution builds without errors.`n - The Program.cs and SampleSeed.cs look as expected.`nDoes the created project look like expected?"

    RunStep "Creating new .NET Core Seed Bucket in '$testDirectoryName'." $action $message
}