using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Xml.Linq;

namespace CalcuMate_1._0
{
    class WolframAlphaClient
    {
        private static readonly IConfiguration Configuration;
        private static readonly string AppId;
        private static readonly string BaseUrl = "http://api.wolframalpha.com/v2/query";
        private static readonly NlpProcessor NlpProcessor;

        static WolframAlphaClient()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();
            AppId = Configuration["WolframAlpha:ApiKey"] ?? throw new InvalidOperationException("A chave da API do Wolfram Alpha não está definida no arquivo de configuração.");
            NlpProcessor = new NlpProcessor();
        }

        public async Task<string> QueryWolframAlpha(string input)
        {
            string processedInput = NlpProcessor.ProcessInput(input);

            using (var client = new HttpClient())
            {
                string url = $"{BaseUrl}?input={Uri.EscapeDataString(processedInput)}&appid={AppId}&format=plaintext";
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseData = await response.Content.ReadAsStringAsync();

                XDocument xml = XDocument.Parse(responseData);
                string result = xml.Descendants("pod")
                                   .Where(pod => (string)pod.Attribute("title") == "Result")
                                   .Descendants("plaintext")
                                   .FirstOrDefault()?.Value;

                return result ?? "Nenhum resultado encontrado.";

            }
        }
    }
}
