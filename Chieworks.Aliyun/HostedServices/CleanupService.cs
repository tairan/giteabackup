using Chieworks.Aliyun.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chieworks.Aliyun.HostedServices
{
    public class CleanupService : IHostedService
    {
        private readonly AliyunOssService ossService;
        private readonly ILogger logger;
        private Timer _timer;

        public CleanupService(AliyunOssService aliyunOssService, ILoggerFactory loggerFactory)
        {
            ossService = aliyunOssService;
            logger = loggerFactory.CreateLogger(Constants.LOGGER_NAME);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"service {nameof(CleanupService)} is started.");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromDays(1)); // TODO: move interval time to appsettings

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            logger.LogInformation($"cleanup job is started.");

            var collection = ossService.List()
                .Where(n => (DateTime.Now - n.LastModified).TotalDays > 7); // TODO: move expire date to appsettings.

            foreach (var item in collection)
            {
                ossService.Delete(item);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"cleanup job is stopped.");
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}
