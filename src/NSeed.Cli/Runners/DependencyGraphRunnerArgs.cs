using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSeed.Cli.Runners
{
    /// <summary>
    /// Model for generating dependency graph.
    /// </summary>
    public class DependencyGraphRunnerArgs
    {
        /// <summary>
        /// Gets or sets path to solution directory.
        /// </summary>
        public string SolutionDirectory { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets path to solution (.sln) file.
        /// </summary>
        public string Solution { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets dependency graph runner output path.
        /// </summary>
        public string OutputPath { get; set; } = string.Empty;
    }
}
