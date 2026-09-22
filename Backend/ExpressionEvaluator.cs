using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;

namespace Backend;

public static class ExpressionEvaluator
{
    public static double Evalute(string infix) => EvalutePostfix(ToPostfix(infix));

    private static List<string> ToPostfix(string infix)
    {
        var posfix = new List<string>();
        var stack = new Stack<char>();

        for (int i = 0; i < infix.Length; i++)
        {
            var item = infix[i];

            if (IsOperator(item))
            {
                if (item == ')')
                {
                    var ope = stack.Pop();

                    while (ope != '(')
                    {
                        posfix.Add(ope.ToString());
                        ope = stack.Pop();
                    }
                }
                else
                {
                    if (stack.Count == 0)
                    {
                        stack.Push(item);
                    }
                    else
                    {
                        if (PriorityInfix(item) > PriorityStack(stack.Peek()))
                        {
                            stack.Push(item);
                        }
                        else
                        {
                            posfix.Add(stack.Pop().ToString());
                            stack.Push(item);
                        }
                    }
                }
            }
            else
            {
                var digits = string.Empty;

                while (i < infix.Length && !IsOperator(infix[i]))
                {
                    digits += infix[i];
                    i++;
                }

                i--;

                var number = Group(digits);

                posfix.Add(number.ToString());
            }
        }
        while (stack.Count != 0)
        {
            posfix.Add(stack.Pop().ToString());
        }

        return posfix;
    }
    

    private static int PriorityStack(char op) => op switch
    {
        '^' => 3,
        '*' => 2,
        '/' => 2,
        '+' => 1,
        '-' => 1,
        '(' => 0,
        _ => throw new Exception("Invalid expression."),
    };

    private static int PriorityInfix(char op) => op switch
    {
        '^' => 4,
        '*' => 2,
        '/' => 2,
        '+' => 1,
        '-' => 1,
        '(' => 5,
        _ => throw new Exception("Invalid expression."),
    };

    private static bool IsOperator(char item) => item == '^' || item == '*' || item == '/' || item == '+' || item == '-' || item == '(' || item == ')';

    private static double EvalutePostfix(List<string> postfix)
    {
        var stack = new Stack<double>();

        foreach (var item in postfix)
        {
            if (IsOperator(item[0]))
            {
                var ope2 = stack.Pop();
                var ope1 = stack.Pop();

                stack.Push(Calculate(ope1, ope2, item[0]));
            }
            else
            {
                stack.Push(Group(item));
            }
        }

        return stack.Pop();
    }

    private static double Calculate(double ope1, double ope2, char item) => item switch
    {
        '*' => ope1 * ope2,
        '/' => ope1 / ope2,
        '+' => ope1 + ope2,
        '-' => ope1 - ope2,
        '^' => Math.Pow(ope1, ope2),
        _ => throw new Exception("Invalid expression."),
    };

    private static int IsNumber(char oper) => oper switch
    {
        '1' => 1,
        '2' => 2,
        '3' => 3,
        '4' => 4,
        '5' => 5,
        '6' => 6,
        '7' => 7,
        '8' => 8,
        '9' => 9,
        '0' => 0,
        _ => throw new Exception("Invalid expression.")
    };

    private static int Group(string digits)
    {
        int number = 0;

        foreach (var ch in digits)
        {
            number = number * 10 + IsNumber(ch);
        }

        return number;
    }
}


