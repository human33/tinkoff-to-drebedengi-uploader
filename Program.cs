using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace T2DUploader
{
    class Program
    {
        private static async Task Main(string[] args)
        {            
            await Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    // services.AddHostedService<ConsoleHostedService>();
                })
                .RunConsoleAsync();
        }
    }
}
