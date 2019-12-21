function RunInfoForDotNetCoreSeedBucket($testDirectoryName) {

		TestName "Run Info for .NET Core Seed Bucket"
		
		DotNetCoreSeedBucketInfo $testDirectoryName
}

function DotNetCoreSeedBucketInfo($testDirectoryName) {
		
	$action = { nseed info --project $testDirectoryName//DotNetCoreSeeds }
		
	$message = "Does the info output look like expected?"
		
	RunStep "Show info for .NET Core Seed Bucket in '$testDirectoryName'." $action $message
}