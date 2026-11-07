// SPDX-License-Identifier: GPL-3.0-only
using HMI.MathExpressions.Common;
using HMI.MathExpressions.Functions;
using HMI.MathExpressions.Parsing;
using HMI.MathExpressions.Values;

namespace HMI.MathExpressions.Compilation;

public sealed class ExpressionCompiler
{
    private readonly Dictionary<string, VariableValue> _variables =
        new(StringComparer.OrdinalIgnoreCase);

    private readonly Dictionary<string, FunctionBase> _functions =
        new(StringComparer.OrdinalIgnoreCase);

    private Value? _root;

    public string Expression { get; private set; }

    public ExpressionCompiler(string expression, bool includeBuiltInFunctions = true)
    {
        Expression = expression;

        if (includeBuiltInFunctions)
            FunctionRegistry.RegisterBuiltInFunctions(this);
    }

    // -----------------------------
    // Variable management
    // -----------------------------
    public void SetVariable(string name, double value)
    {
        if (!_variables.TryGetValue(name, out var variable))
            _variables[name] = variable = new VariableValue(name);

        variable.Set(value);
    }

    public IReadOnlyDictionary<string, VariableValue> Variables => _variables;

    // -----------------------------
    // Function management
    // -----------------------------
    public void AddFunction(FunctionBase function)
    {
        _functions[function.Name] = function;
    }

    public IReadOnlyDictionary<string, FunctionBase> Functions => _functions;

    // -----------------------------
    // Compilation
    // -----------------------------
    public void Compile()
    {
        string normalized = ExpressionNumberFormatConverter.ConvertNumbersToDecimal(Expression);

        var parser = new ExpressionParser(normalized);
        _root = parser.ParseExpression(_functions, _variables);
    }

    // -----------------------------
    // Evaluation
    // -----------------------------
    public double Evaluate()
    {
        if (_root == null)
            Compile();

        return _root!.Evaluate();
    }

    // -----------------------------
    // Reset / Change expression
    // -----------------------------
    public void SetExpression(string expression)
    {
        Expression = expression;
        _root = null;
    }
}
