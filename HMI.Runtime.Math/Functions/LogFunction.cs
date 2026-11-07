// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.MathExpressions.Functions;

using HMI.MathExpressions.Common;
using HMI.MathExpressions.Values;

public sealed class LogFunction(Value arg) : UnaryFunction(arg)
{
    public override string Name => "log";

    public override double Evaluate() => Math.Log(Arg.Evaluate());

    public override FunctionBase CreateInstance(IReadOnlyList<Value> args)
    {
        if (args.Count != 1)
            throw new MathException("log() requires 1 argument");

        return new LogFunction(args[0]);
    }
}

