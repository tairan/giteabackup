using Aliyun.OSS;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Chieworks.Aliyun.Services
{
    public class AliyunOssService
    {
        private readonly ILogger logger;
        private readonly string _bucketName;

        public OssClient Client { get; }

        public AliyunOssService(IOptionsMonitor<AliyunOssOptions> options, ILoggerFactory loggerFactory)
        {
            Client = new OssClient(options.CurrentValue.Endpoint, options.CurrentValue.AccessKeyId, options.CurrentValue.AccessKeySecret);
            _bucketName = options.CurrentValue.BucketName;
            logger = loggerFactory.CreateLogger(Constants.LOGGER_NAME);
        }

        public IEnumerable<OssObjectSummary> List()
        {
            logger.LogInformation($"list all objects in bucket {_bucketName}");

            var result = Client.ListObjects(_bucketName);

            return result.ObjectSummaries;
        }

        public void Delete(OssObjectSummary ossObject)
        {
            logger.LogInformation($"delete the object {ossObject.Key} from bucket {_bucketName}");
            Client.DeleteObject(_bucketName, ossObject.Key);
        }

        public void Upload(string filename)
        {
            logger.LogInformation($"upload file {filename} to bucket {_bucketName}");
            Client.PutObject(_bucketName, Path.GetFileName(filename), File.OpenRead(filename));
        }
    }
}
