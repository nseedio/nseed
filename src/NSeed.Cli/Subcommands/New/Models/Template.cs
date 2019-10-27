using NSeed.Cli.Assets;
using System;
using System.IO;
using static NSeed.Cli.Assets.Resources;

namespace NSeed.Cli.Subcommands.New.Models
{
    internal class Template
    {
        internal static Template Empty { get; } = new Template();

        public Template(string templatesDirectoryPath, FrameworkType frameworkType)
        {
            DirectoryName = Path.Combine(templatesDirectoryPath, GetTemplateDirectory());
            Name = Guid.NewGuid().ToString();

            string GetTemplateDirectory()
            {
                return frameworkType switch
                {
                    FrameworkType.NETCoreApp => NSeedCoreTemplateDirectory,
                    FrameworkType.NETFramework => NSeedClassicTemplateDirectory,
                    FrameworkType.None => string.Empty,
                    _ => string.Empty,
                };
            }
        }

        private Template()
        {
            DirectoryName = string.Empty;
            Name = string.Empty;
        }

        public string DirectoryName { get; }

        public string Name { get; }

        public void ReplacePlaceholders()
        {
            var templateConfigFilePath = Path.Combine(DirectoryName, ".template.config", "template.json");
            string content = File.ReadAllText(templateConfigFilePath);
            content = content.Replace("{{TemplateName}}", Name);
            File.WriteAllText(templateConfigFilePath, content);
        }
    }
}
