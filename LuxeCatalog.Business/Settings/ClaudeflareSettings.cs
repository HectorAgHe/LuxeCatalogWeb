using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.Settings
{
    public class CloudflareSettings
    {
        public string AccountId { get; set; } = string.Empty;
        public string AccessKeyId { get; set; } = string.Empty;
        public string SecretAccessKey { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
        public string BucketName { get; set; } = string.Empty;
        public string PublicUrl { get; set; } = string.Empty;
    }
}   
