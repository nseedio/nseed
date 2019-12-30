function  DisplaySeedBucketInformationForDotNetCoreSeedBucket($testDirectoryName) {
    TestName "Display Seed Bucket Information for .NET Core Seed Bucket"

    DisplaySeedBucketInformationForDotNetCoreSeedBucketCore $testDirectoryName
}

function DisplaySeedBucketInformationForDotNetCoreSeedBucketCore($testDirectoryName) {
    $action = { nseed info --project $testDirectoryName//DotNetCoreSeeds }

    $message = "Does the info output look like expected?"

    RunStep "Displaying seed bucket information for .NET Core seed bucket in '$testDirectoryName'." $action $message
}