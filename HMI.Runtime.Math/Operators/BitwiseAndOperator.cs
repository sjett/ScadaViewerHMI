// SPDX-License-Identifier: GPL-3.0-only
using HMI.MathExpressions.Values;

namespace HMI.MathExpressions.Operators;

//位运算符
public sealed class BitwiseAndOperator(Value left, Value right)
    : BinaryOperator(left, right)
{
    public override double Evaluate()
        => Convert.ToInt64(Left.Evaluate()) & Convert.ToInt64(Right.Evaluate());
}

