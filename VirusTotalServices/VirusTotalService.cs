using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using VirusTotalServices.Infra;
using VirusTotalServices.Interface;
using VirusTotalServices.Models;

namespace VirusTotalServices
{
    public class VirusTotalService : IVirusTotalService
    {
        private readonly VirusTotalConfiguration configuration;

        public VirusTotalService(IOptions<VirusTotalConfiguration> configuration)
        {
            this.configuration = configuration.Value;
        }

        public VirusTotalService(VirusTotalConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public ScanOutput ScanFile(string fileName, byte[] data)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(configuration.ApiUrl);

                var bytes = new ByteArrayContent(data);
                bytes.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "\"file\"",
                    FileName = $"\"{fileName}\"",
                    Size = data.LongLength,
                };
                bytes.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                var apiKey = new StringContent(configuration.ApiKey);
                apiKey.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "\"apikey\""
                };

                var multiContent = new MultipartFormDataContent
                {
                    { bytes },
                    { apiKey }
                };

                return SendPost(httpClient, multiContent);
            }
        }

        private ScanOutput SendPost(HttpClient httpClient, MultipartFormDataContent multiContent)
        {
            //var path = QueryHelpers.AddQueryString(, query);
            using (var result = httpClient.PostAsync(configuration.Methods.Scan, multiContent).Result)
            {
                if (result.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception($"Erro when use VirusTotal Api. StatusCode: {result.StatusCode}");
                }
                var json = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<ScanOutput>(json);
            }
        }
    }
}
