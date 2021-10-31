using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace T2DUploader
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            var rootCommand = new RootCommand
            {
                new Option<FileInfo>(
                    "--tinkoff-dump",
                    description: "Path to tinkoff dump"),
                new Option<FileInfo>(
                    "--drebedengi-dump",
                    "Path to drebedengi dump"),
                new Option<string>(
                    "-o",
                    "An option whose argument is parsed as a FileInfo")
            };

            rootCommand.Description = "An app to convert tinkoff dump to drebedengi format";
            
            rootCommand.Handler = CommandHandler.Create<FileInfo?, FileInfo?, string?>(async (tinkoffDump, drebedengiDump, o) =>
            {
                Console.WriteLine($"The value for --int-option is: {tinkoffDump?.FullName}");
                Console.WriteLine($"The value for --bool-option is: {drebedengiDump?.FullName}");
                Console.WriteLine($"The value for --file-option is: {o ?? "null"}");

                Debug.Assert(tinkoffDump != null, nameof(tinkoffDump) + " != null");
                
                
                // parse expenses
                IAsyncEnumerable<Expense> tinkoffExpenses = TinkoffExpenseParser.Parse(tinkoffDump);
                List<Expense> drebedengiExpenses = await DrebedengiExpenseParser.ParseList(tinkoffDump);
                
                // and find out what is already in drebedengi
                await foreach (Expense expense in tinkoffExpenses)
                {
                    // there is an expense like read from tinkoff
                    if (drebedengiExpenses.Any(e => e.Like(expense)))
                    {
                        //todo: ask user what to do (how?)
                        
                    }
                }
            });
            
            
            await Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<MainService>((serviceProvider) =>
                    {
                        var lifetime = serviceProvider.GetService<IHostApplicationLifetime>();

                        if (lifetime == null)
                        {
                            throw new NullReferenceException("IHostApplicationLifetime is required");
                        }
                        
                        return new MainService(async () => await rootCommand.InvokeAsync(args), lifetime);
                    });
                })
                .RunConsoleAsync();
        }
    }
}
