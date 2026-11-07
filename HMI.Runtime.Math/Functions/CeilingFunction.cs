// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.MathExpressions.Functions;

using HMI.MathExpressions.Common;
using HMI.MathExpressions.Values;

public sealed class CeilingFunction(Value arg) : UnaryFunction(arg)
{
    public override string Name => "ceiling";

    public override double Evaluate() => Math.Ceiling(Arg.Evaluate());

    public override FunctionBase CreateInstance(IReadOnlyList<Value> args)
    {
        if (args.Count != 1)
            throw new MathException("ceiling() requires 1 argument");

        return new CeilingFunction(args[0]);
    }
}

