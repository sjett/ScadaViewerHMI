// SPDX-License-Identifier: GPL-3.0-only
using HMI.MathExpressions.Values;

namespace HMI.MathExpressions.Operators;

public sealed class EqualOperator(Value left, Value right)
    : BinaryOperator(left, right)
{
    public override double Evaluate() => Left.Evaluate() == Right.Evaluate() ? 1 : 0;
}
