// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.MathExpressions.Functions;

using HMI.MathExpressions.Common;
using HMI.MathExpressions.Values;

public sealed class TanhFunction(Value arg) : UnaryFunction(arg)
{
    public override string Name => "tanh";

    public override double Evaluate() => Math.Tanh(Arg.Evaluate());

    public override FunctionBase CreateInstance(IReadOnlyList<Value> args)
    {
        if (args.Count != 1)
            throw new MathException("tanh() requires 1 argument");

        return new TanhFunction(args[0]);
    }
}

