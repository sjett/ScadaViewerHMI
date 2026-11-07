// SPDX-License-Identifier: GPL-3.0-only
using HMI.MathExpressions.Common;

namespace HMI.MathExpressions.Parsing;

public sealed class Tokenizer
{
    private readonly string _text;
    private int _pos;

    private static readonly HashSet<string> MultiCharOperators =
    [
        "&&", "||", "<=", ">=", "!=", "<>", "=="
    ];

    public Tokenizer(string text)
    {
        _text = text;
    }

    public IEnumerable<Token> Tokenize()
    {
        while (_pos < _text.Length)
        {
            char ch = _text[_pos];

            if (char.IsWhiteSpace(ch))
            {
                _pos++;
                continue;
            }

            if (char.IsDigit(ch) || ch == '.')
                yield return ReadNumber();
            else if (char.IsLetter(ch) || ch == '_')
                yield return ReadIdentifier();
            else if (IsOperatorStart(ch))
                yield return ReadOperator();
            else if (ch == '(')
            {
                _pos++;
                yield return new Token(TokenType.LeftParen, "(");
            }
            else if (ch == ')')
            {
                _pos++;
                yield return new Token(TokenType.RightParen, ")");
            }
            else if (ch == ',')
            {
                _pos++;
                yield return new Token(TokenType.Comma, ",");
            }
            else
            {
                throw new MathException($"Unexpected character '{ch}' at position {_pos}");
            }
        }

        yield return new Token(TokenType.End, "");
    }

    private Token ReadNumber()
    {
        int start = _pos;
        while (_pos < _text.Length && (char.IsDigit(_text[_pos]) || _text[_pos] == '.'))
            _pos++;

        return new Token(TokenType.Number, _text[start.._pos]);
    }

    private Token ReadIdentifier()
    {
        int start = _pos;
        while (_pos < _text.Length && (char.IsLetterOrDigit(_text[_pos]) || _text[_pos] == '_'))
            _pos++;

        return new Token(TokenType.Identifier, _text[start.._pos]);
    }

    private Token ReadOperator()
    {
        // Try multi-char operators first
        foreach (var op in MultiCharOperators)
        {
            if (_text.AsSpan(_pos).StartsWith(op))
            {
                _pos += op.Length;
                return new Token(TokenType.Operator, op);
            }
        }

        // Single char operator
        char ch = _text[_pos++];
        return new Token(TokenType.Operator, ch.ToString());
    }

    private static bool IsOperatorStart(char ch)
        => "+-*/^<>=&|!".Contains(ch);
}
