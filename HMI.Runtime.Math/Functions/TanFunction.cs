// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.MathExpressions.Functions;

using HMI.MathExpressions.Common;
using HMI.MathExpressions.Values;

public sealed class TanFunction(Value arg) : UnaryFunction(arg)
{
    public override string Name => "tan";

    public override double Evaluate() => Math.Tan(Arg.Evaluate());

    public override FunctionBase CreateInstance(IReadOnlyList<Value> args)
    {
        if (args.Count != 1)
            throw new MathException("tan() requires 1 argument");

        return new TanFunction(args[0]);
    }
}

