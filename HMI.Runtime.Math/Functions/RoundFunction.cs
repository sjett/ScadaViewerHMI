// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.MathExpressions.Functions;

using HMI.MathExpressions.Common;
using HMI.MathExpressions.Values;

public sealed class RoundFunction(Value arg) : UnaryFunction(arg)
{
    public override string Name => "round";

    public override double Evaluate() => Math.Round(Arg.Evaluate());

    public override FunctionBase CreateInstance(IReadOnlyList<Value> args)
    {
        if (args.Count != 1)
            throw new MathException("round() requires 1 argument");

        return new RoundFunction(args[0]);
    }
}

