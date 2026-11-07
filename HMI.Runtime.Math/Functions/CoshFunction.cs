// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.MathExpressions.Functions;

using HMI.MathExpressions.Common;
using HMI.MathExpressions.Values;

public sealed class CoshFunction(Value arg) : UnaryFunction(arg)
{
    public override string Name => "cosh";

    public override double Evaluate() => Math.Cosh(Arg.Evaluate());

    public override FunctionBase CreateInstance(IReadOnlyList<Value> args)
    {
        if (args.Count != 1)
            throw new MathException("cosh() requires 1 argument");

        return new CoshFunction(args[0]);
    }
}

