using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: CLSCompliant(false)]

[assembly: AssemblyCompany("NSeed")]
[assembly: AssemblyProduct("NSeed")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCopyright("Copyright © 2019 NSeed")]

[assembly: AssemblyVersion("0.1.0")]
// AssemblyFileVersion is not set explicitly. This automatically makes it same as the AssemblyVersion.
[assembly: AssemblyInformationalVersion("0.1.0-prealpha")]

[assembly: AssemblyCulture("")]
[assembly: NeutralResourcesLanguage("en-US")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif