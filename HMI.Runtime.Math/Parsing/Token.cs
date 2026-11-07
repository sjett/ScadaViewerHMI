// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.MathExpressions.Parsing;

public enum TokenType
{
    Number,
    Identifier,
    Operator,
    LeftParen,
    RightParen,
    Comma,
    Bracket,
    End
}

public sealed record Token(TokenType Type, string Text)
{
    public override string ToString() => Text;
}
