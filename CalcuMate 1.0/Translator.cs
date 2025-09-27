using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CalcuMate_1._0
{
    static class Translator
    {
        private static readonly HttpClient client = new HttpClient();
        public static async Task<string> TranslateToEnglish(string text, bool reverse)
        {
            try
            {
                string encodedText = Uri.EscapeDataString(text);
                string url = reverse
                    ? $"https://api.mymemory.translated.net/get?q={encodedText}&langpair=en|pt"
                    : $"https://api.mymemory.translated.net/get?q={encodedText}&langpair=pt|en";

                var response = await client.GetStringAsync(url);
                using JsonDocument doc = JsonDocument.Parse(response);
                string translatedText = doc.RootElement
                    .GetProperty("responseData")
                    .GetProperty("translatedText")
                    .GetString();

                return translatedText;
            }
            catch (HttpRequestException ex)
            {
                return $"[network-error] Falha ao conectar ao serviço de tradução: {ex.Message}";
            }
            catch (TaskCanceledException)
            {
                return "[network-error] Tempo limite de conexão excedido. Verifique sua rede.";
            }
        }


    }
}
