function  DisplaySeedBucketInformationForDotNetClassicSeedBucket($testDirectoryName) {
    if ($IsWindows) {
        TestName "Display Seed Bucket Information for .NET Classic Seed Bucket"

        DisplaySeedBucketInformationForDotNetClassicSeedBucketCore $testDirectoryName
    }
}

function DisplaySeedBucketInformationForDotNetClassicSeedBucketCore($testDirectoryName) {
    $action = { nseed info --project $testDirectoryName//DotNetClassicSeeds }

    $message = "Does the info output look like expected?"

    RunStep "Displaying seed bucket information for .NET Classic seed bucket in '$testDirectoryName'." $action $message
}