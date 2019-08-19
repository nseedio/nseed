using NSeed.Cli.Subcommands.New.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSeed.Cli.Subcommands.New.Runner
{
    /// <summary>
    /// Model for running new subcommand.
    /// </summary>
    public class NewSubcommandRunnerArgs
    {
        /// <summary>
        /// Gets or sets new subcommand project Template.
        /// </summary>
        public Template Template { get; set; } = new Template();

        /// <summary>
        /// Gets or sets path to solution directory.
        /// </summary>
        public string SolutionDirectory { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets path to solution (.sln) file.
        /// </summary>
        public string Solution { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets project name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets framework name.
        /// </summary>
        public string Framework { get; set; } = string.Empty;
    }
}
