using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace T2DUploader
{
    public class MainService: IHostedService
    {
        private readonly Func<Task> _toRun;
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public MainService(Func<Task> toRun, IHostApplicationLifetime applicationLifetime)
        {
            _toRun = toRun;
            _applicationLifetime = applicationLifetime;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            cancellationToken.Register(() => _cancellationTokenSource.Cancel());
            Task whenRun = Task.Run(_toRun, _cancellationTokenSource.Token);
            whenRun.ContinueWith((task) => _applicationLifetime.StopApplication(),
                _cancellationTokenSource.Token);
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            cancellationToken.Register(() => _cancellationTokenSource.Cancel());
            return Task.CompletedTask;
        }
    }
}