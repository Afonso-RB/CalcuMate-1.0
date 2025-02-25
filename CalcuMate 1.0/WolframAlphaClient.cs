using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcuMate_1._0
{
    class WolframAlphaClient
    {
        public static readonly string AppId = Environment.GetEnvironmentVariable("WOLFRAM_ALPHA_API_KEY");
        public static readonly string BaseUrl = "http://api.wolframalpha.com/v2/query";


    }
}
