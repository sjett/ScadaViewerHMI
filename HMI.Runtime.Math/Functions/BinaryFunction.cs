// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.MathExpressions.Functions;

using HMI.MathExpressions.Values;
/// <summary>
/// 二元函数基类
/// </summary>
/// <param name="arg"></param>
public abstract class BinaryFunction(Value left, Value right) : FunctionBase
{
    protected Value Left { get; } = left;
    protected Value Right { get; } = right;
}
