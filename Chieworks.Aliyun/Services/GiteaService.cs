using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Chieworks.Aliyun.Services
{
    public class GiteaService
    {
        private readonly string gitea = "/usr/local/bin/gitea";
        private readonly ILogger logger;

        public GiteaService(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger(Constants.LOGGER_NAME);
        }

        public string Backup()
        {
            var filename = Path.Combine(Path.GetTempPath(), $"gitea-v{DateTime.Now:yyyy-MM-dd}.bak");

            var startInfo = new ProcessStartInfo
            {
                FileName = gitea,
                Arguments = $"dump --file {filename} --work-path /home/gitea --custom-path /home/gitea --config /etc/gitea/app.ini",
                CreateNoWindow = true,
                UseShellExecute = true,
            };

            logger.LogInformation($"invoke gitea process to backup.");

            Process.Start(startInfo)?.WaitForExit();

            return filename;
        }
    }
}
