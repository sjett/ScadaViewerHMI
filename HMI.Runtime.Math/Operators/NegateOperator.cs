// SPDX-License-Identifier: GPL-3.0-only
using HMI.MathExpressions.Values;

namespace HMI.MathExpressions.Operators;

/// <summary>
/// 一元负号
/// </summary>
/// <param name="operand"></param>
public sealed class NegateOperator(Value operand)
    : UnaryOperator(operand)
{
    public override double Evaluate() => -Operand.Evaluate();
}
