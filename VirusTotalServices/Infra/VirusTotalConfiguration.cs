using System;
using System.Collections.Generic;
using System.Text;

namespace VirusTotalServices.Infra
{
    public class VirusTotalConfiguration
    {
        public string ApiKeyName { get; set; }
        public string ApiKey { get; set; }
        public string ApiUrl { get; set; }
        public Methods Methods { get; set; }
    }

    public class Methods
    {
        public string Scan { get; set; }
        public string Report { get; set; }
    }
}
