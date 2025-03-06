using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace CalcuMate_1._0
{
    class WolframAlphaClient
    {
        public static readonly string AppId = Environment.GetEnvironmentVariable("WOLFRAM_ALPHA_API_KEY") ?? throw new InvalidOperationException("A chave da API do Wolfram Alpha não está definida no ambiente.");
        public static readonly string BaseUrl = "http://api.wolframalpha.com/v2/query";
        
        public async Task<string> QueryWolframAlpha(string input)
        {
            using (var client = new HttpClient())
            {
                string url = $"{BaseUrl}?input={Uri.EscapeDataString(input)}&appid={AppId}&format=plaintext";
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseData = await response.Content.ReadAsStringAsync();
                return responseData;
            }
        }
    }
}
