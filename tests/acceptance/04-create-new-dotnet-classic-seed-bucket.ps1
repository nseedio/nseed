function CreateNewDotNetClassicSeedBucket($testDirectoryName) {
    TestName "Create New .NET Classic Seed Bucket"

    CreateNewDotNetClassicSeedBucketCore $testDirectoryName
}

function CreateNewDotNetClassicSeedBucketCore($testDirectoryName) {
    $action = { nseed new --solution $testDirectoryName//Solution.sln --framework netframework4.6.2 --name DotNetClassicSeeds }

    $message = "Open the Solution.sln and check the following:`n - The solution builds without errors.`n - The Program.cs and SampleSeed.cs look as expected.`nDoes the created project look like expected?"

    RunStep "Creating new .NET Classic Seed Bucket in '$testDirectoryName'." $action $message
}