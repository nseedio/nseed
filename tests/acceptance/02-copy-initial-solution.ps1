function CopyInitialSolution($testDirectoryName) {
    StepName "Copy Initial Solution"

    if(test-path $testDirectoryName)
    {
        Info "The directory '$testDirectoryName' already exists and will first be removed."

        RemoveDirectory $testDirectoryName
    }

    CreateDirectory $testDirectoryName

    CopyInitialSolutionCore $testDirectoryName
}

function RemoveDirectory($testDirectoryName) {
    Step "Removing directory '$testDirectoryName'."

    Remove-Item -Recurse -Force $testDirectoryName
    Write-Host

    ExitIfError
}

function CreateDirectory($testDirectoryName) {
    Step "Creating the test directory '$testDirectoryName'."

    New-Item -ItemType Directory -Force -Path $testDirectoryName
    Write-Host

    ExitIfError
}

function CopyInitialSolutionCore($testDirectoryName) {
    Step "Copying the initial solution to '$testDirectoryName'."

    Copy-Item templates//initial-solution//* $testDirectoryName -Force -Recurse
    Write-Host

    ExitIfError
}