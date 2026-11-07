// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.MathExpressions.Functions;

using HMI.MathExpressions.Common;
using HMI.MathExpressions.Values;

public sealed class SignFunction(Value arg) : UnaryFunction(arg)
{
    public override string Name => "sign";

    public override double Evaluate() => Math.Sign(Arg.Evaluate());

    public override FunctionBase CreateInstance(IReadOnlyList<Value> args)
    {
        if (args.Count != 1)
            throw new MathException("sign() requires 1 argument");

        return new SignFunction(args[0]);
    }
}

