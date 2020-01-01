function Error($message) {
    Write-Host $message -ForegroundColor Red
    Write-Host
}

function AssertStepIsSuccessful($message) {
    if ($Host.UI.PromptForChoice("", $message, @('&Yes'; '&No'), 1) -ne 0) {
        Write-Host
        Error "The test was not successful.`nThe script execution will terminate.`nFix the error and run the tests again."
        #TODO Write to error history file.
        Exit                
    }

    Write-Host
}


dotnet test NSeed.Cli.Tests.Integration -c Release -v n
AssertStepIsSuccessful "Is test run successful?"

dotnet test NSeed.Cli.Tests.Unit -c Release -v n
AssertStepIsSuccessful "Is test run successful?"

dotnet test NSeed.Tests.Unit -c Release -v n
AssertStepIsSuccessful "Is test run successful?"
