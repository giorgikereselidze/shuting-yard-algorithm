using System.Data;
using System.Text.RegularExpressions;

namespace shuting_yard_algorithm
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, give me your expression and I'll calculate it!");


            while (true)
            {
                try
                {
                    Console.Write("Type here: ");

                    string? expression = Console.ReadLine();

                    if (string.IsNullOrEmpty(expression))
                        throw new ArgumentNullException("Error. Argument is null! Try again.");

                    List<string> tokens = TokenizeExpression(expression);

                    Dictionary<string, int> precedence = new Dictionary<string, int>
                    {
                        {"+",1 },
                        {"-",1 },
                        {"*",2 },
                        {"/",2 }
                    };

                    double result = EvaluateException(tokens, precedence);
                    Console.WriteLine(result);

                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

        }


        static List<string> TokenizeExpression(string expression)
        {
            string pattern = @"\d+\.?\d*|[+\-*/()]|\s+";

            MatchCollection matches = Regex.Matches(expression, pattern);


            List<string> tokens = new List<string>();
            foreach (Match item in matches)
            {
                tokens.Add(item.Value);
            }

            return tokens;
        }

        static double EvaluateException(List<string> tokens, Dictionary<string, int> precedence)
        {
            // stack for operators
            Stack<string> operators = new Stack<string>();

            // queue for operands(numbers)
            Stack<double> operands = new Stack<double>();


            foreach (var token in tokens)
            {
                if (double.TryParse(token, out double number))
                {
                    operands.Push(number);
                }
                else if (token == "(")
                {
                    operators.Push(token);
                }
                else if (token == ")")
                {
                    while (operators.Peek() != "(")
                    {
                        MakeCalculation(operands, operators.Pop());
                    }

                    operators.Pop();
                }
                else
                {
                    while (operators.Count > 0 && precedence.ContainsKey(operators.Peek()) && precedence[operators.Peek()] >= precedence[token])
                    {
                        MakeCalculation(operands, operators.Pop());
                    }

                    operators.Push(token);
                }
            }

            while (operators.Count > 0)
            {
                MakeCalculation(operands, operators.Pop());
            }

            return operands.Pop();
        }



        static void MakeCalculation(Stack<double> operands, string o)
        {
            double b = operands.Pop();
            double a = operands.Pop();


            switch (o)
            {
                case "+": operands.Push(a + b); break;
                case "-": operands.Push(a - b); break;
                case "*": operands.Push(a * b); break;
                case "/":
                    if (b == 0)
                        throw new DivideByZeroException("Division by zero is not allowed!");
                    operands.Push(a / b);
                    break;
                default:
                    throw new ArgumentException($"Unknown operator: {o}");
            }
        }
    }
}
