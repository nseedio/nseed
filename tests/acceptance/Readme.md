# How to run NSeed acceptance test's on Windows, Linux, and macOS
Are acceptance tests are based on interactivity between a tester and NSeed CLI global tool.
They are created using PowerShell scripts that are installing the NSeed global tool and calling different CLI commands, expecting from the user to confirm successful or less successful output.

To use test properly we first need to install PowerShell version >= 6.0
Since the PowerShell is now open-sourced we can use this link to download and install the proper version. [PowerShell Github](https://github.com/PowerShell/PowerShell#get-powershell)

### Windows
---
+ Install **PowerShell as .Net Core global tool**  [Link](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell-core-on-windows?view=powershell-6)
    + ``` dotnet tool install --global PowerShell ```
+ Run **pwsh command** in any of your cmd tools (PowerShell, Command Prompt, Windows Terminal...) 
+ Locate acceptance test folder in NSeed repository and run ```.\_run-acceptance-tests.ps1 ```

### Linux
---
+ Install powerShell on Linux  [Link](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell-core-on-linux) 
    + On the provided link you will find steps for installing PowerShell on Linux depending on Linux distributions and version. Example: [Ubuntu 18.4](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell-core-on-linux?view=powershell-6#ubuntu-1804) 
+ Run **pwsh command** in Linux terminal
+ Locate acceptance test folder in NSeed repository and run ```.\_run-acceptance-tests.ps1 ```

### Mac Os
---
+ Install **PowerShell as .Net Core global tool**  [Link](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell-core-on-macos?view=powershell-6)

  + ``` dotnet tool install --global PowerShell ```
+ Run **pwsh command** in masOS terminal
+ Locate acceptance test folder in NSeed repository and run ```.\_run-acceptance-tests.ps1 ```

### Windows <3 Linux
---
[Link](https://docs.microsoft.com/en-us/windows/wsl/install-win10) to install WSL (Windows Subsystem for Linux) on Windows10 