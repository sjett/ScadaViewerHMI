// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.MathExpressions.Functions;

using HMI.MathExpressions.Common;
using HMI.MathExpressions.Values;

public sealed class MaxFunction(Value left, Value right) : BinaryFunction(left, right)
{
    public override string Name => "max";

    public override double Evaluate()
        => Math.Max(Left.Evaluate(), Right.Evaluate());

    public override FunctionBase CreateInstance(IReadOnlyList<Value> args)
    {
        if (args.Count != 2)
            throw new MathException("max() requires 2 arguments");

        return new MaxFunction(args[0], args[1]);
    }
}


