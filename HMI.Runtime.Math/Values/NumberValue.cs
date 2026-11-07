// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.MathExpressions.Values;

public sealed class NumberValue(double value) : Value
{
    public override double Evaluate() => value;
}
