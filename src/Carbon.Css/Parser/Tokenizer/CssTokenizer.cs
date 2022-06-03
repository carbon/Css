using System;
using System.Collections.Generic;
using System.IO;

namespace Carbon.Css.Parser;

public sealed class CssTokenizer : IDisposable
{
    private readonly Stack<CssToken> stack;

    private CssToken current;
    private bool isEnd;

    // Don't make these RO
    private LexicalModeContext mode;
    private readonly SourceReader reader;

    // Exposed for testing
    public CssTokenizer(string text)
        : this(new SourceReader(new StringReader(text))) { }

    internal CssTokenizer(SourceReader reader, LexicalMode mode = LexicalMode.Selector)
    {
        this.reader = reader;

        this.reader.Next(); // Start the reader

        this.mode = new LexicalModeContext(mode);

        this.stack = new Stack<CssToken>(3);
        current = default;
        isEnd = false;

        this.Next(); // Load the first token
    }

    public CssToken Current => current;

    public bool IsEnd => isEnd;

    // Returns the current token and advances to the next
    public CssToken Consume()
    {
        if (isEnd)
        {
            throw new EndOfStreamException("Already ready the last token");
        }

        var c = current;

        if (!reader.IsEof) Next();
        else isEnd = true;

        return c;
    }

    public CssToken Next()
    {
        current = ReadNext();

        return current;
    }

    private CssToken ReadNext()
    {
        if (reader.IsEof)
        {
            throw new Exception($"Cannot read past EOF. Current: {current}.");
        }

        if (stack.Count > 0)
        {
            return stack.Pop();
        }

        if (char.IsWhiteSpace(reader.Current) || reader.Current is '\uFEFF')
        {
            return ReadTrivia();
        }

        switch (reader.Current)
        {
            case '@': return new CssToken(CssTokenKind.AtSymbol, reader.Read(), reader.Position);

            case '$':
                mode.Enter(LexicalMode.Symbol);

                return new CssToken(CssTokenKind.Dollar, reader.Read(), reader.Position);

            case '/':

                return reader.Peek() switch
                {
                    '/' or '*' => ReadComment(),
                    ' ' => new CssToken(CssTokenKind.Divide, reader.Read(), reader.Position),
                    _ => new CssToken(CssTokenKind.String, reader.Read(), reader.Position)
                };


            case ':':
                // Pseudo-elements
                if (reader.Peek() is ':' or '-')
                {
                    return ReadValue();
                }
                else
                {
                    var colon = new CssToken(CssTokenKind.Colon, reader.Read(), reader.Position);

                    return MaybeColonOrPseudoClass(colon);
                }

            case ',': return new CssToken(CssTokenKind.Comma, reader.Read(), reader.Position);
            case ';':
                LeaveValueMode();
                return new CssToken(CssTokenKind.Semicolon, reader.Read(), reader.Position);
            case '{':
                mode.Enter(LexicalMode.Block);

                return new CssToken(CssTokenKind.BlockStart, reader.Read(), reader.Position);
            case '}':

                LeaveValueMode();

                if (mode.Current == LexicalMode.InterpolatedString)
                {
                    mode.Leave(LexicalMode.InterpolatedString);

                    return new CssToken(CssTokenKind.InterpolatedStringEnd, reader.Read(), reader.Position);
                }
                else
                {
                    mode.Leave(LexicalMode.Block, this.Current.Position);

                    return new CssToken(CssTokenKind.BlockEnd, reader.Read(), reader.Position);
                }

            case '(': return new CssToken(CssTokenKind.LeftParenthesis, reader.Read(), reader.Position);
            case ')': return new CssToken(CssTokenKind.RightParenthesis, reader.Read(), reader.Position);

            case '&':
                if (reader.Peek() is '&')
                    return new CssToken(CssTokenKind.And, reader.Read(2), reader.Position - 1);
                else
                    return new CssToken(CssTokenKind.Ampersand, reader.Read(), reader.Position);

            case '|' when (reader.Peek() is '|'): // ||
                return new CssToken(CssTokenKind.Or, reader.Read(2), reader.Position - 1);

            case '!': // !=
                if (reader.Peek() is '=') return new CssToken(CssTokenKind.NotEquals, reader.Read(2), reader.Position - 1);

                else break;
            case '=' when (reader.Peek() is '='): // ==
                return new CssToken(CssTokenKind.Equals, reader.Read(2), reader.Position - 1);

            case '>': // >=
                if (reader.Peek() == '=') return new CssToken(CssTokenKind.Gte, reader.Read(2), reader.Position - 1);
                else return new CssToken(CssTokenKind.Gt, reader.Read(), reader.Position);

            case '<': // <=
                if (reader.Peek() == '=') return new CssToken(CssTokenKind.Lte, reader.Read(2), reader.Position - 1);
                else return new CssToken(CssTokenKind.Lt, reader.Read(), reader.Position);

            case '#' when reader.Peek() == '{':
                mode.Enter(LexicalMode.InterpolatedString);

                return new CssToken(CssTokenKind.InterpolatedStringStart, reader.Read(2), reader.Position - 1);

            case '+' when reader.Peek() == ' ':
                return new CssToken(CssTokenKind.Add, reader.Read(), reader.Position);
            case '*': return new CssToken(CssTokenKind.Multiply, reader.Read(), reader.Position);

            case '.' when char.IsDigit(reader.Peek()):
                return ReadNumber();

            case '-':
                if (char.IsDigit(reader.Peek()))
                    return ReadNumber();
                else if (reader.Peek() == ' ')
                    return new CssToken(CssTokenKind.Subtract, reader.Read(), reader.Position);
                else
                    break;

            case '\'':
            case '"':
                return ReadQuotedString(reader.Current);


            case '0':
            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case '6':
            case '7':
            case '8':
            case '9':
                return ReadNumber();

        }

        return mode.Current switch
        {
            LexicalMode.Symbol => ReadSymbol(),
            LexicalMode.Value => ReadValue(),
            LexicalMode.Unit => ReadUnit(),
            _ => ReadName()
        };
    }

    private CssToken ReadUnit()
    {
        reader.Mark();

        while (char.IsLetter(reader.Current) || reader.Current == '%')
        {
            if (reader.IsEof) throw SyntaxException.UnexpectedEOF("Name");

            reader.Next();
        }

        mode.Leave(LexicalMode.Unit);

        return new CssToken(CssTokenKind.Unit, reader.Unmark(), reader.MarkStart);
    }

    private CssToken ReadSymbol()
    {
        reader.Mark();

        while (!char.IsWhiteSpace(reader.Current) && !(
            reader.Current is '{'
                           or '}'
                           or '('
                           or ')'
                           or ';'
                           or ':'
                           or ','))
        {
            if (reader.IsEof) throw SyntaxException.UnexpectedEOF("Symbol");

            reader.Next();
        }

        mode.Leave(LexicalMode.Symbol);

        return new CssToken(CssTokenKind.Name, reader.Unmark(), reader.MarkStart);
    }

    private CssToken ReadName()
    {
        reader.Mark();

        while (!char.IsWhiteSpace(reader.Current) &&
            reader.Current != '{' &&
            reader.Current != '}' &&
            reader.Current != '(' &&
            reader.Current != ')' &&
            reader.Current != ';' &&
            reader.Current != ':' &&
            reader.Current != ',')
        {
            if (reader.IsEof) break;

            if (reader.Current == '#' && reader.Peek() == '{')
            {
                break;
            }

            reader.Next();
        }

        return new CssToken(CssTokenKind.Name, reader.Unmark(), reader.MarkStart);
    }

    private void LeaveValueMode()
    {
        if (mode.Current == LexicalMode.Value)
        {
            mode.Leave(LexicalMode.Value, this.Current.Position);
        }
    }

    private CssToken ReadQuotedString(char quoteChar)
    {
        reader.Mark();

        reader.Next(); // "

        while (reader.Current != quoteChar && !reader.IsEof)
        {
            reader.Next();
        }

        reader.Next(); // "

        return new CssToken(CssTokenKind.String, reader.Unmark(), reader.MarkStart);
    }

    private CssToken MaybeColonOrPseudoClass(CssToken colon)
    {
        if (char.IsWhiteSpace(reader.Current))
        {
            mode.Enter(LexicalMode.Value);

            return colon;
        }

        reader.Mark();

        while (!char.IsWhiteSpace(reader.Current) &&
            reader.Current != '{' &&
            reader.Current != '}' &&
            reader.Current != ')' &&
            reader.Current != '(' &&
            reader.Current != ';' &&
            reader.Current != ',' &&
            reader.Current != ':' &&
            !reader.IsEof)
        {
            reader.Next();
        }

        var text = reader.Unmark();

        if (reader.Current == '(' || PseudoClassNames.Contains(text))
        {
            return new CssToken(CssTokenKind.Name, ":" + text, reader.MarkStart);
        }

        else
        {
            // We'll return this on the next read...

            stack.Push(new CssToken(CssTokenKind.String, text, reader.MarkStart));

            mode.Enter(LexicalMode.Value);

            return colon;
        }
    }

    private CssToken ReadValue()
    {
        reader.Mark();

        while (!char.IsWhiteSpace(reader.Current) &&
            reader.Current != '{' &&
            reader.Current != '}' &&
            reader.Current != ')' &&
            reader.Current != '(' &&
            reader.Current != ';' &&
            reader.Current != ',' &&
            !reader.IsEof)
        {
            reader.Next();
        }

        var text = reader.Unmark();

        if (PseudoClassNames.Contains(text))
        {
            // LeaveValueMode();

            return new CssToken(CssTokenKind.Name, text, reader.MarkStart);
        }

        return new CssToken(CssTokenKind.String, text, reader.MarkStart);
    }

    private CssToken ReadNumber()
    {
        reader.Mark();

        // Read a leading '-'
        if (reader.Current == '-') reader.Next();

        while ((char.IsDigit(reader.Current) || reader.Current == '.') && !reader.IsEof)
        {
            reader.Next();
        }

        if (reader.Current == '%' || char.IsLetter(reader.Current))
        {
            mode.Enter(LexicalMode.Unit);
        }

        return new CssToken(CssTokenKind.Number, reader.Unmark(), reader.MarkStart);
    }

    private CssToken ReadTrivia()
    {
        reader.Mark();

        // \uFEFF : zero with non-breaking space

        while ((char.IsWhiteSpace(reader.Current) || reader.Current == '\uFEFF') && !reader.IsEof)
        {
            reader.Next();
        }

        return new CssToken(CssTokenKind.Whitespace, reader.Unmark(), reader.MarkStart);
    }

    private CssToken ReadComment()
    {
        /* */

        reader.Mark();

        reader.Read();                  // read /

        if (reader.Current == '/')
        {
            return ReadLineComment();
        }

        reader.Read();                  // read *

        while (!reader.IsEof)
        {
            if (reader.Current == '*' && reader.Peek() == '/')
            {
                break;
            }

            reader.Next();
        }

        reader.Read(); // read *
        reader.Read(); // read /

        return new CssToken(CssTokenKind.Comment, reader.Unmark(), reader.MarkStart);
    }

    private CssToken ReadLineComment()
    {
        // line comment

        reader.Next(); // read /

        bool isDirective = reader.Current == '=';

        while (reader.Current != '\n' && reader.Current != '\r')
        {
            if (reader.IsEof) break;

            reader.Next();
        }

        return new CssToken(isDirective ? CssTokenKind.Directive : CssTokenKind.Comment, reader.Unmark(), reader.MarkStart);
    }

    public void Dispose()
    {
        reader.Dispose();
    }
}

// https://www.w3.org/TR/css-syntax/#typedef-dimension-token