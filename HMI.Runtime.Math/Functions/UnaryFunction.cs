// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.MathExpressions.Functions;

using HMI.MathExpressions.Values;
/// <summary>
/// 一元函数基类
/// </summary>
/// <param name="arg"></param>
public abstract class UnaryFunction(Value arg) : FunctionBase
{
    protected Value Arg { get; } = arg;
}
