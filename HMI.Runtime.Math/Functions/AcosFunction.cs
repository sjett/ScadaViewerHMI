// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.MathExpressions.Functions;

using HMI.MathExpressions.Common;
using HMI.MathExpressions.Values;

public sealed class AcosFunction(Value arg) : UnaryFunction(arg)
{
    public override string Name => "acos";

    public override double Evaluate() => Math.Acos(Arg.Evaluate());

    public override FunctionBase CreateInstance(IReadOnlyList<Value> args)
    {
        if (args.Count != 1)
            throw new MathException("acos() requires 1 argument");

        return new AcosFunction(args[0]);
    }
}
