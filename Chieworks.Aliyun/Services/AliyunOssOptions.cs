using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chieworks.Aliyun.Services
{
    public class AliyunOssOptions
    {
        public string Endpoint { get; set; }

        public string AccessKeyId { get; set; }

        public string AccessKeySecret { get; set; }

        public string BucketName { get; set; }
    }
}
