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

        static WolframAlphaClient()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();
            AppId = Configuration["WolframAlpha:ApiKey"] ?? throw new InvalidOperationException("A chave da API do Wolfram Alpha não está definida no arquivo de configuração.");
        }

        public async Task<string> QueryWolframAlpha(string input)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{BaseUrl}?input={Uri.EscapeDataString(input)}&appid={AppId}&format=plaintext";
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseData = await response.Content.ReadAsStringAsync();

                    //Verifica se a resposta está vazia
                    if (string.IsNullOrWhiteSpace(responseData))
                        return "A resposta está vazia. Verifique sua conexão ou tente novamente.";

                    //Tratamento da resposta
                    try
                    {
                        var xml = XDocument.Parse(responseData);
                        var root = xml.Root;

                        // Verifica os atributos do <queryresult>
                        bool success = root?.Attribute("success")?.Value == "true";
                        bool error = root?.Attribute("error")?.Value == "true";

                        if (!success)
                            return "A consulta não foi compreendida. Tente reformular com termos matemáticos mais claros.";

                        if (error)
                            return "O Wolfram Alpha encontrou um erro interno ao processar sua consulta.";

                        // Tenta encontrar o pod "Result"
                        var resultPod = xml.Descendants("pod")
                                           .FirstOrDefault(p => p.Attribute("title")?.Value.ToLower() == "result");

                        if (resultPod == null)
                        {
                            // Procura outros pods úteis, como "Solution", "Decimal approximation", etc.
                            // trecho corrigido
                            var fallbackPod = xml.Descendants("pod")
                                .FirstOrDefault(p =>
                                {
                                    // pega o atributo e normaliza para minúsculas
                                    var title = p.Attribute("title")?.Value?.ToLower();
                                    // só testa Contains se title não for nulo
                                    return title != null
                                           && (title.Contains("solution") || title.Contains("approximation"));
                                });

                            var fallbackText = fallbackPod?.Descendants("plaintext").FirstOrDefault()?.Value;

                            if (!string.IsNullOrWhiteSpace(fallbackText))
                                return fallbackText;

                            return "Nenhum resultado direto foi retornado. Tente simplificar ou detalhar a consulta.";
                        }

                        string resultText = resultPod.Descendants("plaintext").FirstOrDefault()?.Value;

                        return string.IsNullOrWhiteSpace(resultText)
                            ? "Resultado não disponível no formato esperado."
                            : resultText;
                    }
                    catch (Exception ex)
                    {
                        return $"Erro ao analisar a resposta: {ex.Message}";
                    }


                }
            }
            catch (HttpRequestException ex)
            {
                return $"[network-error] Falha ao conectar à internet: {ex.Message}";
            }
            catch (TaskCanceledException)
            {
                return "[network-error] Tempo limite de conexão excedido. Verifique sua rede.";
            }
            catch (Exception ex)
            {
                return $"[network-error] Erro inesperado: {ex.Message}";
            }
        }
    }
}
