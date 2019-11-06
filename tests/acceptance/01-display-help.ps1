function DisplayHelp() {
    StepName "Display Help"

    CallOnlyNSeed

    CallNSeedHelp

    CallNSeedInfoHelp
}

function CallOnlyNSeed() {
    Step "Calling: nseed."

    nseed

    Write-Host

    ExitIfError

    AssertTestIsSuccessful "Does the help output look like expected?"
}

function CallNSeedHelp() {
    Step "Calling: nseed --help."

    nseed --help

    Write-Host

    ExitIfError

    AssertTestIsSuccessful "Does the help output look like expected?"
}

function CallNSeedInfoHelp() {
    Step "Calling: nseed info --help."

    nseed info --help

    Write-Host

    ExitIfError

    AssertTestIsSuccessful "Does the help output look like expected?"
}