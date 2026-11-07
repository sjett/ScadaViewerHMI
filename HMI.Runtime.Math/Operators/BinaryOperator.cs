// SPDX-License-Identifier: GPL-3.0-only

using HMI.MathExpressions.Values;

namespace HMI.MathExpressions.Operators;
/// <summary>
/// 二元运算符基类
/// </summary>
public abstract class BinaryOperator(Value left, Value right) : OperatorBase
{
    protected Value Left { get; } = left;
    protected Value Right { get; } = right;
}
