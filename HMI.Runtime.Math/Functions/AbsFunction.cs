// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.MathExpressions.Functions;

using HMI.MathExpressions.Common;
using HMI.MathExpressions.Values;

public sealed class AbsFunction(Value arg) : UnaryFunction(arg)
{
    public override string Name => "abs";

    public override double Evaluate() => Math.Abs(Arg.Evaluate());

    public override FunctionBase CreateInstance(IReadOnlyList<Value> args)
    {
        if (args.Count != 1)
            throw new MathException("abs() requires 1 argument");

        return new AbsFunction(args[0]);
    }
}
