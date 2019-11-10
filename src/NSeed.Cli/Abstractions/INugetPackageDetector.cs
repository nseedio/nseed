using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSeed.Cli.Abstractions
{
    internal interface INugetPackageDetector
    {
        IOperationResponse Detect(Project project, string nugetPackageName);
    }
}
