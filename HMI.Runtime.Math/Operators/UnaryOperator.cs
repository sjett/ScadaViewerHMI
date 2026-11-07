// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.MathExpressions.Operators;

using HMI.MathExpressions.Values;

/// <summary>
/// 一元运算符基类
/// </summary>
/// <param name="operand"></param>
public abstract class UnaryOperator(Value operand) : OperatorBase
{
    protected Value Operand { get; } = operand;
}
