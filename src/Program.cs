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
using Microsoft.Extensions.Logging;

namespace T2DUploader
{
    class Program
    {
        private static async Task Main(string[] args)
        {           
            await Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {

                    services.AddSingleton<UploaderOptions, UploaderOptions>((sp) => 
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
                        System.CommandLine.Parsing.ParseResult r = rootCommand.Parse(args);
                        return new UploaderOptions() 
                        {
                            tinkoffDump = (FileInfo?)r.ValueForOption("--tinkoff-dump"),
                            drebedengiDump = (FileInfo?)r.ValueForOption("--drebedengi-dump"),
                            o = (string?)r.ValueForOption("-o")
                        };
                    });

                    services.AddSingleton<IUserInterface, ConsoleInterface>();
                    services.AddSingleton<Uploader, Uploader>();
                    services.AddHostedService<MainService>((serviceProvider) =>
                    {
                        var lifetime = serviceProvider.GetService<IHostApplicationLifetime>();

                        if (lifetime == null)
                        {
                            throw new NullReferenceException("IHostApplicationLifetime is required");
                        }

                        
                        var uploader = serviceProvider.GetService<Uploader>();

                        if (uploader == null)
                        {
                            throw new NullReferenceException("Uploader is required");
                        }


                        return new MainService(async () => {
                            await uploader.Upload();
                        }, lifetime);
                    });
                })
                .RunConsoleAsync();
        }
    }
}
