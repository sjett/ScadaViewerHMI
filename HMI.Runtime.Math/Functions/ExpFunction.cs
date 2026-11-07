// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.MathExpressions.Functions;

using HMI.MathExpressions.Common;
using HMI.MathExpressions.Values;

public sealed class ExpFunction(Value arg) : UnaryFunction(arg)
{
    public override string Name => "exp";

    public override double Evaluate() => Math.Exp(Arg.Evaluate());

    public override FunctionBase CreateInstance(IReadOnlyList<Value> args)
    {
        if (args.Count != 1)
            throw new MathException("exp() requires 1 argument");

        return new ExpFunction(args[0]);
    }
}

