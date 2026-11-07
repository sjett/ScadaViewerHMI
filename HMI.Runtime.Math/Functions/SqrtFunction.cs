// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.MathExpressions.Functions;

using HMI.MathExpressions.Common;
using HMI.MathExpressions.Values;

public sealed class SqrtFunction(Value arg) : UnaryFunction(arg)
{
    public override string Name => "sqrt";

    public override double Evaluate() => Math.Sqrt(Arg.Evaluate());

    public override FunctionBase CreateInstance(IReadOnlyList<Value> args)
    {
        if (args.Count != 1)
            throw new MathException("sqrt() requires 1 argument");

        return new SqrtFunction(args[0]);
    }
}

