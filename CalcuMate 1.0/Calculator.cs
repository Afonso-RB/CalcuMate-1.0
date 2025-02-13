using System;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Statistics;
using NCalc;

namespace CalcuMate_1._0
{
    class Calculator
    {
        //Avaliar Expressão com NCalc
        public double EvalueteExpression(string expression)
        {
            var e = new Expression(expression);
            return Convert.ToDouble(e.Evaluate());
        }

        //Funções Trigonométricas com Math.Net
        public double CalculateSin(double angle)
        {
            return Trig.Sin(angle);
        }

        /* Operações matriciais com Math.NET
        {
        ...
        }*/

        /* Estatísticas descritivas com Math.NET
        {
        ...
        }
        */

        /* Produto escalar de vetores com Math.NET
        {
        ...
        }
        */

        // Conversões de unidades com Math.NET
        public double DegreesToRadians(double degrees)
        {
            return Trig.DegreeToRadian(degrees);
        }

        //Precessar entrada de usuário e decidir qual método chamar

    }
}
