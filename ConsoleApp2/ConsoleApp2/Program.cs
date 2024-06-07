using System.Text.RegularExpressions;

public interface IOperation
{
    double Apply(double left, double right);
    int Precedence { get; }
    char OperatorSymbol { get; }
}
public interface IExpressionParser
{
    List<string> Parse(string expression);
}
public interface IExpressionEvaluator
{
    double Evaluate(List<string> tokens);
}
public class Addition : IOperation
{
    public double Apply(double left, double right) => left + right;
    public int Precedence => 1;
    public char OperatorSymbol => '+';
}
public class Subtraction : IOperation
{
    public double Apply(double left, double right) => left - right;
    public int Precedence => 1;
    public char OperatorSymbol => '-';
}
public class Multiplication : IOperation
{
    public double Apply(double left, double right) => left * right;
    public int Precedence => 2;
    public char OperatorSymbol => '*';
}
public class Division : IOperation
{
    public double Apply(double left, double right) => left / right;
    public int Precedence => 2;
    public char OperatorSymbol => '/';
}

public class ExpressionParser : IExpressionParser
{
    public List<string> Parse(string expression)
    {
        var tokens = new List<string>();
        var matches = Regex.Matches(expression, @"(\d+\.?\d*|\+|\-|\*|\/|\(|\))");

        foreach (Match match in matches)
        {
            tokens.Add(match.Value);
        }

        return tokens;
    }
}
public class ExpressionEvaluator : IExpressionEvaluator
{
    private readonly Dictionary<char, IOperation> _operations;

    public ExpressionEvaluator(IEnumerable<IOperation> operations)
    {
        _operations = operations.ToDictionary(op => op.OperatorSymbol);
    }

    public double Evaluate(List<string> tokens)
    {
        var values = new Stack<double>();
        var operators = new Stack<char>();

        for (int i = 0; i < tokens.Count; i++)
        {
            string token = tokens[i];

            if (double.TryParse(token, out double value))
            {
                values.Push(value);
            }
            else if (token == "(")
            {
                operators.Push('(');
            }
            else if (token == ")")
            {
                while (operators.Peek() != '(')
                {
                    ProcessOperator(values, operators);
                }
                operators.Pop();
            }
            else if (_operations.ContainsKey(token[0]))
            {
                while (operators.Count > 0 && _operations.ContainsKey(operators.Peek()) &&
                       _operations[operators.Peek()].Precedence >= _operations[token[0]].Precedence)
                {
                    ProcessOperator(values, operators);
                }
                operators.Push(token[0]);
            }
        }

        while (operators.Count > 0)
        {
            ProcessOperator(values, operators);
        }

        return values.Pop();
    }

    private void ProcessOperator(Stack<double> values, Stack<char> operators)
    {
        var right = values.Pop();
        var left = values.Pop();
        var op = operators.Pop();

        values.Push(_operations[op].Apply(left, right));
    }
}
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Введите выражение:");
        string input = Console.ReadLine();

        IExpressionParser parser = new ExpressionParser();
        IExpressionEvaluator evaluator = new ExpressionEvaluator(new List<IOperation>
        {
            new Addition(),
            new Subtraction(),
            new Multiplication(),
            new Division()
        });

        List<string> tokens = parser.Parse(input);
        double result = evaluator.Evaluate(tokens);

        Console.WriteLine($"Результат: {result}");
    }
}