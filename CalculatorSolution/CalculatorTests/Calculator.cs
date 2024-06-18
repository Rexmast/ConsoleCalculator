using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CalculatorLibrary
{
    public class Calculator
    {
        private readonly Dictionary<string, Func<double, double, double>> operations;

        public Calculator()
        {
            operations = new Dictionary<string, Func<double, double, double>>
            {
                { "+", (a, b) => a + b },
                { "-", (a, b) => a - b },
                { "*", (a, b) => a * b },
                { "/", (a, b) => a / b }
            };
        }

        public double Evaluate(string expression)
        {
            var tokens = Tokenize(expression);
            var result = Parse(tokens);
            return result;
        }

        private List<string> Tokenize(string expression)
        {
            var pattern = @"(\d+\.?\d*|\+|\-|\*|\/|\(|\))";
            var regex = new Regex(pattern);
            var matches = regex.Matches(expression);

            var tokens = new List<string>();
            foreach (Match match in matches)
            {
                tokens.Add(match.Value);
            }

            return tokens;
        }

        private double Parse(List<string> tokens)
        {
            // Примитивный парсер для демонстрации 
            Stack<double> values = new Stack<double>();
            Stack<string> ops = new Stack<string>();

            foreach (var token in tokens)
            {
                if (double.TryParse(token, out double number))
                {
                    values.Push(number);
                }
                else if (operations.ContainsKey(token))
                {
                    ops.Push(token);
                }
                else
                {
                    throw new ArgumentException($"Unexpected token: {token}");
                }
            }

            while (ops.Count > 0)
            {
                var op = ops.Pop();
                var b = values.Pop();
                var a = values.Pop();
                values.Push(operations[op](a, b));
            }

            return values.Pop();
        }
    }
}
