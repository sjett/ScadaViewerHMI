// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.MathExpressions.Functions;

using HMI.MathExpressions.Common;
using HMI.MathExpressions.Values;

public sealed class CosFunction(Value arg) : UnaryFunction(arg)
{
    public override string Name => "cos";

    public override double Evaluate() => Math.Cos(Arg.Evaluate());

    public override FunctionBase CreateInstance(IReadOnlyList<Value> args)
    {
        if (args.Count != 1)
            throw new MathException("cos() requires 1 argument");

        return new CosFunction(args[0]);
    }
}

