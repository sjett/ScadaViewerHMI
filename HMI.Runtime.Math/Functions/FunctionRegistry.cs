// SPDX-License-Identifier: GPL-3.0-only
using HMI.MathExpressions.Compilation;

namespace HMI.MathExpressions.Functions;

public static class FunctionRegistry
{
    public static void RegisterBuiltInFunctions(ExpressionCompiler compiler)
    {
        compiler.AddFunction(new AbsFunction(null!));
        compiler.AddFunction(new AcosFunction(null!));
        compiler.AddFunction(new AsinFunction(null!));
        compiler.AddFunction(new AtanFunction(null!));
        compiler.AddFunction(new CeilingFunction(null!));
        compiler.AddFunction(new CosFunction(null!));
        compiler.AddFunction(new CoshFunction(null!));
        compiler.AddFunction(new ExpFunction(null!));
        compiler.AddFunction(new FloorFunction(null!));
        compiler.AddFunction(new LogFunction(null!));
        compiler.AddFunction(new Log10Function(null!));
        compiler.AddFunction(new MaxFunction(null!, null!));
        compiler.AddFunction(new MinFunction(null!, null!));
        compiler.AddFunction(new RoundFunction(null!));
        compiler.AddFunction(new SignFunction(null!));
        compiler.AddFunction(new SinFunction(null!));
        compiler.AddFunction(new SinhFunction(null!));
        compiler.AddFunction(new SqrtFunction(null!));
        compiler.AddFunction(new TanFunction(null!));
        compiler.AddFunction(new TanhFunction(null!));
        compiler.AddFunction(new IfFunction(null!, null!, null!));
    }
}
