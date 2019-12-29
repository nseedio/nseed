function CheckMinimumRequiredPowerShellVersion($minimumRequiredVersion) {
    TestName "Check Minimum Required PowerShell Version"

    $installedVersion = GetPowerShellVersion

    Info "Minimum required version: $minimumRequiredVersion"
    Info "Installed version:        $installedVersion"

    if ($installedVersion -lt $minimumRequiredVersion) {
        Error "The installed version is smaller then the minimum required version.`nThe acceptance tests will not be executed.`nFor detailed instructions on how to install the required PowerShell version see the README.md."
        Exit
    }
}

function GetPowerShellVersion {
    if (test-path Variable:PSVersionTable) {$PSVersionTable.PSVersion.ToString()} else {"1.0.0.0"}
}