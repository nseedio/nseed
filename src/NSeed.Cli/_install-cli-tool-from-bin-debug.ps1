param(
    [string]$Version=(Read-Host "NSeed CLI Tool Version")
)

# ------- Common functions -------

# TODO: This code is copied (and slightly modified) from the acceptance code scripts. Think of some common PS1 scripts in the future.

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

function ExitIfError() {
    if ((!$?) -or ($LASTEXITCODE -ne 0) -or ($error.count -gt 0))
    {
        Error "An error occured while executing the comman.`nThe script execution will terminate.`nFix the error and run the script again."
        Exit
    }
}

function Run($description, $action) {
    Info $description

    Invoke-Command -ScriptBlock $action

    Write-Host

    ExitIfError
}

# ------- Main -------

function InstallNSeedCliToolFromBinDebug($version) {
    Info "Install NSeed CLI Tool From Bin/Debug"

    if (IsNSeedCliToolAlreadyInstalled)
    {
        Info "NSeed CLI Tool is already installed and will first be uninstalled."

        UninstallNSeedCliTool
    }

    InstallNSeedCliToolCore $version
}

function IsNSeedCliToolAlreadyInstalled() {
    $globalToolListContainsNSeedCli = dotnet tool list -g | Out-String -Stream | Select-String "NSeed.Cli"
    ![string]::IsNullOrEmpty($globalToolListContainsNSeedCli)
}

function UninstallNSeedCliTool() {
    Run "Uninstalling existing NSeed CLI Tool." { dotnet tool uninstall -g NSeed.Cli }
}

function InstallNSeedCliToolCore($version) {
    Run "Installing NSeed CLI Tool v$version." { dotnet tool install -g --version $version --add-source "../../output" NSeed.Cli }
}


InstallNSeedCliToolFromBinDebug $Version