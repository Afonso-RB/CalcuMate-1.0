using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CalcuMate_1._0
{
    static class WolframQueryFormatter
    {
        private static readonly Dictionary<string, string> Terms = new()
        {
            { "raiz quadrada", "square root" },
            { "raíz quadrada", "square root" },
            { "raiz de", "square root of" },
            { "limite", "limit" },
            { "derivada", "derivative" },
            { "integração", "integration" },
            { "integral de", "integral of" },
            { "soma", "sum" },
            { "multiplicação", "multiplication" },
            { "multiplique", "multiply" },
            { "divida", "divide" },
            { "divisão", "division" },
            { "subtraia", "subtract" },
            { "subtração", "subtraction" },
            { "por", "by" },
            { "de", "of" },
            { "tende a", "approaches" },
            { "quando", "when" },
            { "x tende a", "x→" },
            { "quanto é", "what is" },
            { "qual é", "what is" },
            { "calcule", "calculate" },
            { "resolva", "solve" },
            { "encontre", "find" },
            { "para", "for" },
            { "logaritmo", "logarithm" },
            { "logaritmo de", "log of" },
            { "exponencial de", "exponential of" },
            { "cos", "cosine" },
            { "sen", "sine" },
            { "tangente", "tangent" },
            { "faça", "" },
            { "quero", "" },
            { "me diga", "" },
            { "por favor", "" },
            { "o valor de", "" },
            { "quanto dá", "" },
            { "qual o valor de", "" },
            { "qual o resultado de", "" },
            { "qual é o resultado de", "" },
            { "qual é o valor de", "" },
            { "calcula", "" },
            { "calcular", "" },
            { "resultado de", "" },
            { "valor de", "" },
            { "resultado", "" }


    };

        public static string ConvertToWolframFriendlyQuery(string input) 
        {
            string processed = input.ToLower().Trim();
            foreach (var term in Terms)
            {
                processed = Regex.Replace(processed, term.Key, term.Value, RegexOptions.IgnoreCase);
            }
            // Remove palavras de preenchimento
            processed = Regex.Replace(processed, @"(quero|calcular|calcula|por favor|faça|me diga|qual é|o valor de|quanto dá)", "", RegexOptions.IgnoreCase);

            // Remove múltiplos espaços
            processed = Regex.Replace(processed, @"\s+", " ").Trim();

            // Correções específicas
            processed = processed.Replace("limit", "limit of"); // Ajuste para garantir legibilidade

            return processed;
        }
    }
}
