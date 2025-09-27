using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CalcuMate_1._0
{
    static class PlaceholderTranslator
    {
        public static async Task<string> TranslateWithPlaceholdersAsync(string input)
        {
            var expressionMap = new Dictionary<string, string>();
            int exprCount = 1;

            // Regex para detectar expressões matemáticas
            var exprPattern = @"([\d\w\s\+\-\*/\^=().→]+)";
            var matches = Regex.Matches(input, exprPattern);

            string maskedInput = input;

            foreach (Match match in matches)
            {
                string expr = match.Value.Trim();

                // Heurística: só substitui se tiver operadores ou variáveis
                if (Regex.IsMatch(expr, @"[\+\-\*/\^=()]") || Regex.IsMatch(expr, @"\b[a-zA-Z]\b"))
                {
                    string placeholder = $"[EXPR{exprCount}]";
                    if (!expressionMap.ContainsKey(placeholder))
                    {
                        expressionMap[placeholder] = expr;
                        maskedInput = maskedInput.Replace(expr, placeholder);
                        exprCount++;
                    }
                }
            }

            // Traduz apenas o texto com placeholders
            string translated = await Translator.TranslateToEnglish(maskedInput, false);

            if (translated.StartsWith("[network-error]"))
                return translated;

            // Reinsere expressões
            foreach (var kvp in expressionMap)
            {
                translated = translated.Replace(kvp.Key, kvp.Value);
            }

            return translated;
        }

    }
}
