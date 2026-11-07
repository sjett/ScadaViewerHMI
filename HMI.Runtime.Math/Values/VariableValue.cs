// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.MathExpressions.Values;

public sealed class VariableValue(string name, double initialValue = double.NaN) : Value
{
    public string Name { get; } = name;
    public double Value { get; private set; } = initialValue;

    public void Set(double newValue) => Value = newValue;

    public override double Evaluate() => Value;
}
