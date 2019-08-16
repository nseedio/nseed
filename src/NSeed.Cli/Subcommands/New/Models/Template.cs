using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSeed.Cli.Subcommands.New.Models
{
    /// <summary>
    /// Project template.
    /// </summary>
    public class Template
    {
        /// <summary>
        /// Gets or sets project template path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets Project name.
        /// </summary>
        public string Name { get; set; }
    }
}
