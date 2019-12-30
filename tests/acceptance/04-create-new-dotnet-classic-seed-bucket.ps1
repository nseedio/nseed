function CreateNewDotNetClassicSeedBucket($testDirectoryName) {	
	if ($IsWindows) {
		
		TestName "Create New .NET Classic Seed Bucket"

		CreateNewDotNetClassicSeedBucketCore $testDirectoryName

        ManuallyAddNSeedNuGetPackageToTheProject
	}
}

function CreateNewDotNetClassicSeedBucketCore($testDirectoryName) {		
	$action = { nseed new --solution $testDirectoryName//Solution.sln --framework netframework4.6.2 --name DotNetClassicSeeds }
		
	$message = "Open the Solution.sln and check the following:`n - The Program.cs and SampleSeed.cs look as expected.`nDoes the created project look like expected?"
		
	RunStep "Creating new .NET Classic Seed Bucket in '$testDirectoryName'." $action $message
}

function ManuallyAddNSeedNuGetPackageToTheProject() {		
    $action = { Info "Add the NSeed NuGet package to the 'DotNetClassicSeeds' project (e.g. in Visual Studio)." }

	$message = "Did you added NSeed NuGet package to the 'DotNetClassicSeeds' project?"
		
	RunStep "Manually adding NSeed NuGet package to the project 'DotNetClassicSeeds.csproj'" $action $message
}