function RunInfoForDotNetClassicSeedBucket($testDirectoryName) {

	if ($IsWindows) {

	TestName "Run Info for .NET Classic Seed Bucket"
	
	DotNetClassicSeedBucketInfo $testDirectoryName

	}
}

function DotNetClassicSeedBucketInfo($testDirectoryName) {
	
$action = { nseed info --project $testDirectoryName//DotNetClassicSeeds }
	
$message = "Does the info output look like expected?"
	
RunStep "Show info for .NET Classic Seed Bucket in '$testDirectoryName'." $action $message
}