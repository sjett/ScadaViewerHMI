// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.MathExpressions.Functions;

using HMI.MathExpressions.Common;
using HMI.MathExpressions.Values;

public sealed class FloorFunction(Value arg) : UnaryFunction(arg)
{
    public override string Name => "floor";

    public override double Evaluate() => Math.Floor(Arg.Evaluate());

    public override FunctionBase CreateInstance(IReadOnlyList<Value> args)
    {
        if (args.Count != 1)
            throw new MathException("floor() requires 1 argument");

        return new FloorFunction(args[0]);
    }
}

