// SPDX-License-Identifier: GPL-3.0-only
using HMI.MathExpressions.Common;
using HMI.MathExpressions.Functions;
using HMI.MathExpressions.Operators;
using HMI.MathExpressions.Values;

namespace HMI.MathExpressions.Parsing;

public sealed class ExpressionParser
{
    private readonly List<Token> _tokens;
    private int _pos;

    private Token Current => _pos < _tokens.Count ? _tokens[_pos] : new Token(TokenType.End, "");

    public ExpressionParser(string expression)
    {
        _tokens = new Tokenizer(expression).Tokenize().ToList();
    }

    public Value ParseExpression(
        Dictionary<string, FunctionBase> functions,
        Dictionary<string, VariableValue> variables)
    {
        return ParseLogical(functions, variables);
    }

    // -----------------------------
    // 1. Logical operators: &&, ||
    // -----------------------------
    private Value ParseLogical(
        Dictionary<string, FunctionBase> functions,
        Dictionary<string, VariableValue> variables)
    {
        var left = ParseComparison(functions, variables);

        while (Match("&&") || Match("||"))
        {
            string op = Previous().Text;
            var right = ParseComparison(functions, variables);

            left = op switch
            {
                "&&" => new LogicalAndOperator(left, right),
                "||" => new LogicalOrOperator(left, right),
                _ => throw new MathException($"Unknown logical operator {op}")
            };
        }

        return left;
    }

    // -----------------------------
    // 2. Comparison: <, >, <=, >=, ==, !=
    // -----------------------------
    private Value ParseComparison(
        Dictionary<string, FunctionBase> functions,
        Dictionary<string, VariableValue> variables)
    {
        var left = ParseAdditive(functions, variables);

        while (Match("<") || Match(">") || Match("<=") || Match(">=") || Match("==") || Match("!=") || Match("<>"))
        {
            string op = Previous().Text;
            var right = ParseAdditive(functions, variables);

            left = op switch
            {
                "<" => new LessThanOperator(left, right),
                ">" => new GreaterThanOperator(left, right),
                "<=" => new LessOrEqualOperator(left, right),
                ">=" => new GreaterOrEqualOperator(left, right),
                "==" => new EqualOperator(left, right),
                "!=" => new NotEqualOperator(left, right),
                "<>" => new NotEqualOperator(left, right),
                _ => throw new MathException($"Unknown comparison operator {op}")
            };
        }

        return left;
    }

    // -----------------------------
    // 3. Additive: +, -
    // -----------------------------
    private Value ParseAdditive(
        Dictionary<string, FunctionBase> functions,
        Dictionary<string, VariableValue> variables)
    {
        var left = ParseMultiplicative(functions, variables);

        while (Match("+") || Match("-"))
        {
            string op = Previous().Text;
            var right = ParseMultiplicative(functions, variables);

            left = op switch
            {
                "+" => new AddOperator(left, right),
                "-" => new SubtractOperator(left, right),
                _ => throw new MathException($"Unknown additive operator {op}")
            };
        }

        return left;
    }

    // -----------------------------
    // 4. Multiplicative: *, /
    // -----------------------------
    private Value ParseMultiplicative(
        Dictionary<string, FunctionBase> functions,
        Dictionary<string, VariableValue> variables)
    {
        var left = ParsePower(functions, variables);

        while (Match("*") || Match("/"))
        {
            string op = Previous().Text;
            var right = ParsePower(functions, variables);

            left = op switch
            {
                "*" => new MultiplyOperator(left, right),
                "/" => new DivideOperator(left, right),
                _ => throw new MathException($"Unknown multiplicative operator {op}")
            };
        }

        return left;
    }

    // -----------------------------
    // 5. Power: ^
    // -----------------------------
    private Value ParsePower(
        Dictionary<string, FunctionBase> functions,
        Dictionary<string, VariableValue> variables)
    {
        var left = ParseUnary(functions, variables);

        while (Match("^"))
        {
            var right = ParseUnary(functions, variables);
            left = new PowerOperator(left, right);
        }

        return left;
    }

    // -----------------------------
    // 6. Unary: +x, -x
    // -----------------------------
    private Value ParseUnary(
        Dictionary<string, FunctionBase> functions,
        Dictionary<string, VariableValue> variables)
    {
        if (Match("+"))
            return ParseUnary(functions, variables);

        if (Match("-"))
            return new NegateOperator(ParseUnary(functions, variables));

        return ParsePrimary(functions, variables);
    }

    // -----------------------------
    // 7. Primary: number, variable, function, (expr)
    // -----------------------------
    private Value ParsePrimary(
        Dictionary<string, FunctionBase> functions,
        Dictionary<string, VariableValue> variables)
    {
        if (Match(TokenType.Number))
            return new NumberValue(double.Parse(Previous().Text));

        if (Match(TokenType.Identifier))
        {
            string name = Previous().Text;

            // Function call
            if (Match(TokenType.LeftParen))
            {
                var args = new List<Value>();

                if (!Match(TokenType.RightParen))
                {
                    do
                    {
                        args.Add(ParseExpression(functions, variables));
                    }
                    while (Match(TokenType.Comma));

                    Consume(TokenType.RightParen, "Expected ')' after function arguments");
                }

                if (!functions.TryGetValue(name, out var func))
                    throw new MathException($"Function '{name}' not found");

                return func.CreateInstance(args);
            }

            // Variable
            if (!variables.TryGetValue(name, out var variable))
            {
                variable = new VariableValue(name);
                variables[name] = variable;
            }

            return variable;
        }

        if (Match(TokenType.LeftParen))
        {
            var expr = ParseExpression(functions, variables);
            Consume(TokenType.RightParen, "Expected ')'");
            return expr;
        }

        throw new MathException($"Unexpected token: {Current.Text}");
    }

    // -----------------------------
    // Token helpers
    // -----------------------------
    private bool Match(string text)
    {
        if (Current.Type == TokenType.Operator && Current.Text == text)
        {
            _pos++;
            return true;
        }
        return false;
    }

    private bool Match(TokenType type)
    {
        if (Current.Type == type)
        {
            _pos++;
            return true;
        }
        return false;
    }

    private Token Previous() => _tokens[_pos - 1];

    private Token Consume(TokenType type, string message)
    {
        if (Current.Type == type)
            return _tokens[_pos++];

        throw new MathException(message);
    }
}
