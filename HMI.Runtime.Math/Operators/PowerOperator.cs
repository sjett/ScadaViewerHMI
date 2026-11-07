// SPDX-License-Identifier: GPL-3.0-only
using HMI.MathExpressions.Values;

namespace HMI.MathExpressions.Operators;
/// <summary>
/// 幂运算
/// </summary>
/// <param name="left"></param>
/// <param name="right"></param>
public sealed class PowerOperator(Value left, Value right)
    : BinaryOperator(left, right)
{
    public override double Evaluate() => System.Math.Pow(Left.Evaluate(), Right.Evaluate());
}
