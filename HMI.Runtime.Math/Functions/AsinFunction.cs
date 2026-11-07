// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.MathExpressions.Functions;

using HMI.MathExpressions.Common;
using HMI.MathExpressions.Values;

public sealed class AsinFunction(Value arg) : UnaryFunction(arg)
{
    public override string Name => "asin";

    public override double Evaluate() => Math.Asin(Arg.Evaluate());

    public override FunctionBase CreateInstance(IReadOnlyList<Value> args)
    {
        if (args.Count != 1)
            throw new MathException("asin() requires 1 argument");

        return new AsinFunction(args[0]);
    }
}
