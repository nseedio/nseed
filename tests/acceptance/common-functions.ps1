function Info($message) {
    Write-Host $message
    Write-Host
}

function Warning($message) {
    Write-Host $message -ForegroundColor Yellow
    Write-Host
}

function Error($message) {
    Write-Host $message -ForegroundColor Red
    Write-Host
}

function Confirmation($message) {
    Write-Host $message -ForegroundColor DarkGreen
    Write-Host
}

function Step($message) {
    Write-Host (" --> " + $message) -ForegroundColor Cyan
    Write-Host
}

function ExitIfError() {
    if ((!$?) -or ($LASTEXITCODE -ne 0) -or ($error.count -gt 0))
    {
        Error "An error occured while executing the previous step.`nThe script execution will terminate.`nFix the error and run the acceptance tests again."
        #TODO Write to error history file.
        Exit
    }
}

function TestName($testName) {
    $horizontalLine = new-object System.String('-', $testName.Length)
    Write-Host
    Write-Host $horizontalLine -ForegroundColor Cyan
    Write-Host $testName -ForegroundColor Cyan
    Write-Host $horizontalLine -ForegroundColor Cyan
    Write-Host
}

function AssertStepIsSuccessful($message) {
    if ($Host.UI.PromptForChoice("", $message, @('&Yes'; '&No'), 1) -ne 0)
    {
        Write-Host
        Error "The test was not successful.`nThe script execution will terminate.`nFix the error and run the acceptance tests again."
        #TODO Write to error history file.
        Exit                
    }

    Write-Host
}

function RunStep($stepDescription, $action, $assertMessage) {
    Step $stepDescription

    Invoke-Command -ScriptBlock $action

    Write-Host

    ExitIfError

    if (![string]::IsNullOrWhitespace($assertMessage)) {
        AssertStepIsSuccessful $assertMessage
    }
}