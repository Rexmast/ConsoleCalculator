using System;
using CalculatorLibrary;

namespace CalculatorSolution
{
    class Program
    {
        static void Main(string[] args)
        {
            var calculator = new Calculator();
            Console.WriteLine("Введите выражение:");
            var input = Console.ReadLine();

            try
            {
                var result = calculator.Evaluate(input);
                Console.WriteLine($"Результат: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}
