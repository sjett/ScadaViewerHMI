// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.MathExpressions.Functions;

using HMI.MathExpressions.Common;
using HMI.MathExpressions.Values;

public sealed class Log10Function(Value arg) : UnaryFunction(arg)
{
    public override string Name => "log10";

    public override double Evaluate() => Math.Log10(Arg.Evaluate());

    public override FunctionBase CreateInstance(IReadOnlyList<Value> args)
    {
        if (args.Count != 1)
            throw new MathException("log10() requires 1 argument");

        return new Log10Function(args[0]);
    }
}

