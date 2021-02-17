using Chieworks.Aliyun.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chieworks.Aliyun.HostedServices
{
    public class BackupService : IHostedService
    {
        private readonly AliyunOssService ossService;
        private readonly GiteaService _giteaService;
        private Timer _timer;
        private readonly ILogger logger;

        public BackupService(AliyunOssService aliyunOssService, GiteaService giteaService, ILoggerFactory loggerFactory)
        {
            ossService = aliyunOssService;
            _giteaService = giteaService;
            logger = loggerFactory.CreateLogger(Constants.LOGGER_NAME);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"service {nameof(BackupService)} is started.");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromDays(1)); // TODO: move interval time to appsettings

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            logger.LogInformation($"backup job is started.");
            var filename = _giteaService.Backup();

            ossService.Upload(filename);

            File.Delete(filename);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"backup job is stopped.");
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}
