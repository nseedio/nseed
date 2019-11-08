function InstallNSeedCliTool($version) {
    TestName "Install NSeed CLI Tool"

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
    RunStep "Uninstalling existing NSeed CLI Tool." { dotnet tool uninstall -g NSeed.Cli }
}

function InstallNSeedCliToolCore($version) {
    RunStep "Installing NSeed CLI Tool v$version." { dotnet tool install -g --version $version --add-source "../../output" NSeed.Cli }
}