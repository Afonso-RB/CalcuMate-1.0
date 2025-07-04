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
        public static string Refine(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            string query = input.Trim().ToLower();

            // 1. Remover frases que não afetam o resultado
            string[] fillerPhrases = new[]
            {
            "please", "i want", "i would like to", "can you", "show me", "tell me", "what is", "calculate",
            "give me", "could you", "find", "answer to", "let me know", "i need", "i am looking for", "help me with", "explain", "show me how to"
        };
            foreach (var phrase in fillerPhrases)
            {
                query = Regex.Replace(query, $@"\b{Regex.Escape(phrase)}\b", "", RegexOptions.IgnoreCase);
            }

            // 2. Simplificar expressões matemáticas conhecidas
            query = Regex.Replace(query, @"square root of (\d+)", "sqrt($1)", RegexOptions.IgnoreCase);
            query = Regex.Replace(query, @"square of (\d+)", "$1^2", RegexOptions.IgnoreCase);
            query = Regex.Replace(query, @"logarithm of (\d+)", "log($1)", RegexOptions.IgnoreCase);
            query = Regex.Replace(query, @"natural logarithm of (\d+)", "ln($1)", RegexOptions.IgnoreCase);
            query = Regex.Replace(query, @"cube root of (\d+)", "cbrt($1)", RegexOptions.IgnoreCase);
            query = Regex.Replace(query, @"(\d+) raised to the power of (\d+)", "$1^$2", RegexOptions.IgnoreCase);
            query = Regex.Replace(query, @"(\d+) to the power of (\d+)", "$1^$2", RegexOptions.IgnoreCase);
            query = Regex.Replace(query, @"exponential of (\d+)", "exp($1)", RegexOptions.IgnoreCase);
            query = Regex.Replace(query, @"limit of ([\w\d\s/()+*-^]+?) as x (?:tends to|approaches) infinity", "limit $1 as x→∞", RegexOptions.IgnoreCase);
            query = Regex.Replace(query, @"(?<!\w)sine of ([\w\d]+)", "sin($1)", RegexOptions.IgnoreCase);
            query = Regex.Replace(query, @"(?<!\w)cosine of ([\w\d]+)", "cos($1)", RegexOptions.IgnoreCase);
            query = Regex.Replace(query, @"(?<!\w)tangent of ([\w\d]+)", "tan($1)", RegexOptions.IgnoreCase);
            query = Regex.Replace(query, @"(?<!\w)cotangent of ([\w\d]+)", "cot($1)", RegexOptions.IgnoreCase);
            query = Regex.Replace(query, @"(?<!\w)secant of ([\w\d]+)", "sec($1)", RegexOptions.IgnoreCase);
            query = Regex.Replace(query, @"(?<!\w)cosecant of ([\w\d]+)", "csc($1)", RegexOptions.IgnoreCase);
            query = Regex.Replace(query, @"(?<!\w)factorial of (\d+)", "$1!", RegexOptions.IgnoreCase);
            query = Regex.Replace(query, @"(?<!\w)pi", "π", RegexOptions.IgnoreCase);

            // 3. Padronizar setas e espaços
            query = query.Replace("->", "→");
            query = Regex.Replace(query, @"\s+", " ").Trim();

            return query;
        }

    }
}
