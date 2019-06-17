using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Desafio.ConsoleUpload
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!args.Any())
            {
                Console.WriteLine("Path argument is missing");
                Console.ReadLine();
                return;
            }

            foreach (var path in args)
            {
                if (!File.Exists(path))
                {
                    Console.WriteLine($"File '{path}' not exists. Skipping...");
                    continue;
                }

                var data = File.ReadAllBytes(path);
                var filename = path.Split('\\').LastOrDefault();

                if (data.Length == 0 || string.IsNullOrEmpty(filename))
                {
                    Console.WriteLine($"File '{path}' is invalid. Skipping...");
                    continue;
                }

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:5001");

                    var bytes = new ByteArrayContent(data);
                    bytes.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "\"file\"",
                        FileName = $"\"{filename}\"",
                        Size = data.LongLength,
                    };
                    bytes.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    var multiContent = new MultipartFormDataContent
                    {
                        { bytes }
                    };
                    using (var result = client.PostAsync("api/process", multiContent).Result)
                    {
                        if (result.StatusCode != HttpStatusCode.OK)
                        {
                            throw new Exception($"Erro when use Api. StatusCode: {result.StatusCode}");
                        }
                    }
                }
                Console.WriteLine($"File '{filename}' was uploaded successfully");
            }
        }
    }
}
