function CopyInitialSolution($testDirectoryName) {
    TestName "Copy Initial Solution"

    if(test-path $testDirectoryName)
    {
        Info "The directory '$testDirectoryName' already exists and will first be removed."

        RemoveDirectory $testDirectoryName
    }

    CreateDirectory $testDirectoryName

    CopyInitialSolutionCore $testDirectoryName
}

function RemoveDirectory($testDirectoryName) {
    RunStep "Removing directory '$testDirectoryName'." { Remove-Item -Recurse -Force $testDirectoryName }
}

function CreateDirectory($testDirectoryName) {
    RunStep "Creating the test directory '$testDirectoryName'." { New-Item -ItemType Directory -Force -Path $testDirectoryName }
}

function CopyInitialSolutionCore($testDirectoryName) {
    RunStep "Copying the initial solution to '$testDirectoryName'." { Copy-Item templates//initial-solution//* $testDirectoryName -Force -Recurse }
}