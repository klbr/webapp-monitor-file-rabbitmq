using Desafio.Domain;
using Desafio.Domain.DTO;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using VirusTotalServices.Infra;
using VirusTotalServices.Interface;

namespace VirusTotalServices
{
    public class VirusTotalService : IVirusTotalService
    {
        private readonly VirusTotalConfiguration configuration;
        private JsonSerializerSettings settings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() } };

        public VirusTotalService(IOptions<VirusTotalConfiguration> configuration)
        {
            this.configuration = configuration.Value;
        }

        public VirusTotalService(VirusTotalConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public ReportedFileJson Report(string resource)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(configuration.ApiUrl);
                var path = $"{configuration.Methods.Report}?apikey={configuration.ApiKey}&resource={resource}";
                using (var result = httpClient.GetAsync(path).Result)
                {
                    if (result.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception($"Erro when use VirusTotal Api. StatusCode: {result.StatusCode}");
                    }
                    var json = result.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<ReportedFileJson>(json, settings);
                }
            }
        }

        public ScannedFileJson ScanFile(string fileName, byte[] data)
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

        private ScannedFileJson SendPost(HttpClient httpClient, MultipartFormDataContent multiContent)
        {
            //var path = QueryHelpers.AddQueryString(, query);
            using (var result = httpClient.PostAsync(configuration.Methods.Scan, multiContent).Result)
            {
                if (result.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception($"Erro when use VirusTotal Api. StatusCode: {result.StatusCode}");
                }
                var json = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<ScannedFileJson>(json, settings);
            }
        }
    }
}
