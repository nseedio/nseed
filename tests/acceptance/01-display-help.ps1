function DisplayHelp() {
    TestName "Display Help"

    $assertTestMessage = "Does the help output look like expected?"

    CallOnlyNSeed $assertTestMessage

    CallNSeedHelp $assertTestMessage

    CallNSeedNewHelp $assertTestMessage

    CallNSeedInfoHelp $assertTestMessage
}

function CallOnlyNSeed($message) {
    RunStep "Calling: nseed." { nseed } $message
}

function CallNSeedHelp($message) {
    RunStep "Calling: nseed --help." { nseed --help } $message
}

function CallNSeedNewHelp($message) {
    RunStep "Calling: nseed new --help." { nseed new --help } $message
}

function CallNSeedInfoHelp($message) {
    RunStep "Calling: nseed info --help." { nseed info --help } $message
}