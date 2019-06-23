using CommandLine;
using NSeed.Cli;
using System;
using System.Threading.Tasks;

namespace NSeed.Seeding
{
    public abstract class SeedBucket
    {
        protected static async Task Seed<TSeedBucket>(string[] args) where TSeedBucket : SeedBucket
        {
            if (args.Length > 0)
            {
                ICommandExecutor commandExecutor = null;

                Parser.Default.ParseArguments<ListVerbCommandOptions, SeedVerbCommandOptions>(args)
                    .WithParsed<ListVerbCommandOptions>(options => commandExecutor = new ListCommandExecutor(options))
                    .WithParsed<SeedVerbCommandOptions>(options => commandExecutor = new SeedCommandExecutor(options));

                try
                {
                    if (commandExecutor != null) await commandExecutor.Execute<TSeedBucket>();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                return;
            }
        }
    }
}