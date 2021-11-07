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
using System.CommandLine.Builder;
using System.Text;
using Microsoft.Extensions.Logging;
using T2DUploader.Model;
using T2DUploader.Services;
using T2DUploader.Services.ExpenseMapper;

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
                new Option<FileInfo>(
                    "--desc2account",
                    "Path to description to account mapping"),
                new Option<string>(
                    "-o",
                    "An option whose argument is parsed as a FileInfo")
            };

            rootCommand.Description = "An app to convert tinkoff dump to drebedengi format";
            System.CommandLine.Parsing.ParseResult r = rootCommand.Parse(args);
            
            await Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<UploaderOptions, UploaderOptions>((sp) =>
                    {
                        var db = (FileInfo?)r.ValueForOption("--drebedengi-dump");
                        
                        if (db is not { Exists: true })
                        {
                            throw new Exception(
                                "--drebedengi-dump option is required to point to a existing file");
                        }

                        return new UploaderOptions()
                        {
                            DrebedengiDump = new T2DUploader.Utility.FileInfo(db),
                            OutputFilePath = (string?)r.ValueForOption("-o"),
                        };
                    });
                    services.AddSingleton<IUploader, Uploader>();
                    
                    services.AddSingleton<ExpenseMapperOptions, ExpenseMapperOptions>((sp) => 
                    {
                        var td = (FileInfo?)r.ValueForOption("--tinkoff-dump");

                        if (td is not { Exists: true })
                        {
                            throw new Exception(
                                "--tinkoff-dump option is required to point to a existing file");
                        }
                        
                        var desc2Account = (FileInfo?)r.ValueForOption("--desc2account");
                        
                        if (desc2Account is not { Exists: true })
                        {
                            throw new Exception(
                                "--desc2account option is required to point to a existing file");
                        }

                        Dictionary<string, string> desc2AccountMapping;
                        using (var stream = desc2Account.OpenRead())
                        {
                            desc2AccountMapping = System.Text.Json.JsonSerializer.DeserializeAsync
                                    <Dictionary<string, string>>(stream)
                                .GetAwaiter().GetResult() ?? throw new ArgumentNullException("desc2account");
                        }
                        
                        return new ExpenseMapperOptions() 
                        {
                            TinkoffDump = new T2DUploader.Utility.FileInfo(td),
                            DescriptionToAccount = desc2AccountMapping
                        };
                    });

                    services.AddSingleton<IUserInterface, ConsoleInterface>();
                    services.AddSingleton<IExpenseMapper, ExpenseMapper>();
                    services.AddHostedService<MainService>((serviceProvider) =>
                    {
                        var lifetime = serviceProvider.GetService<IHostApplicationLifetime>();

                        if (lifetime == null)
                        {
                            throw new NullReferenceException("IHostApplicationLifetime is required");
                        }

                        
                        var mapper = serviceProvider.GetService<IExpenseMapper>();

                        if (mapper == null)
                        {
                            throw new NullReferenceException("Mapper is required");
                        }
                        
                        var uploader = serviceProvider.GetService<IUploader>();

                        if (uploader == null)
                        {
                            throw new NullReferenceException("Uploader is required");
                        }

                        // var logger = serviceProvider.GetService<ILogger>();

                        return new MainService(async () => {
                            Console.Out.WriteLine("Load expenses");
                            await foreach (Expense expense in mapper.Map())
                            {
                                Console.Out.WriteLine("Handle expense");
                                await uploader.Upload(expense);
                            }

                        }, lifetime);
                    });
                })
                .RunConsoleAsync();
        }
    }
}
