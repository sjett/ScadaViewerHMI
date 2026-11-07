// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.MathExpressions.Functions;

using HMI.MathExpressions.Values;

public abstract class FunctionBase : Value
{
    public abstract string Name { get; }

    public abstract FunctionBase CreateInstance(IReadOnlyList<Value> args);
}

