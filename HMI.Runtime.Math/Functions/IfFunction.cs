// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.MathExpressions.Functions;

using HMI.MathExpressions.Common;
using HMI.MathExpressions.Values;

public sealed class IfFunction(Value cond, Value a, Value b) : FunctionBase
{
    private Value Cond { get; } = cond;
    private Value A { get; } = a;
    private Value B { get; } = b;

    public override string Name => "if";

    public override double Evaluate()
        => Cond.Evaluate() != 0 ? A.Evaluate() : B.Evaluate();

    public override FunctionBase CreateInstance(IReadOnlyList<Value> args)
    {
        if (args.Count != 3)
            throw new MathException("if() requires 3 arguments");

        return new IfFunction(args[0], args[1], args[2]);
    }
}



