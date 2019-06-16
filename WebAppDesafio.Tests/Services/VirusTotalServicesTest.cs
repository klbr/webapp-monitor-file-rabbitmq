using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VirusTotalServices;
using VirusTotalServices.Infra;
using Xunit;

namespace Desafio.Tests.Services
{
    public class VirusTotalServicesTest
    {
        private readonly VirusTotalConfiguration configuration;
        private readonly VirusTotalService service;

        public VirusTotalServicesTest()
        {
            configuration = new VirusTotalConfiguration
            {
                ApiKey = "7d12106f6dc6e029ad3a6ced7d91008b639bd223f88e8bcef7bb2ebbe870d1f5",
                ApiKeyName = "apiKey",
                ApiUrl = "https://www.virustotal.com/vtapi/v2/",
                Methods = new Methods
                {
                    Report = "file/report",
                    Scan = "file/scan"
                }
            };

            service = new VirusTotalService(configuration);
        }

        [Fact]
        public void CheckExeFileShouldBeSucess()
        {
            //arrange
            var filename = "notepad.exe";
            var data = File.ReadAllBytes("Resources\\notepad.exe");

            //act
            var scanOutput = service.ScanFile(filename, data);

            //assert
            Assert.NotNull(scanOutput);
        }
    }
}
