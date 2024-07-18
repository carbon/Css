using System.Buffers;
using System.IO;

namespace Carbon.Css.Parser;

public sealed class CssTokenizer : IDisposable
{
    private readonly Stack<CssToken> _stack;

    private CssToken current;
    private bool isEnd;

    // Don't make these RO
    private LexicalModeContext _mode;
    private readonly SourceReader reader;

    // Exposed for testing
    public CssTokenizer(string text)
        : this(new SourceReader(new StringReader(text))) { }

    internal CssTokenizer(SourceReader reader, LexicalMode mode = LexicalMode.Selector)
    {
        this.reader = reader;

        this.reader.Advance(); // Start the reader

        _mode = new LexicalModeContext(mode);

        _stack = new Stack<CssToken>(3);
        current = default;
        isEnd = false;

        this.Next(); // Load the first token
    }

    public CssToken Current => current;

    public bool IsEnd => isEnd;

    public bool TryPeek(out CssToken token)
    {
        if (reader.IsEof)
        {
            token = default;

            return false;
        }

        token = ReadInternal();

        _stack.Push(token);

        return true;
    }

    // Returns the current token and advances to the next
    public CssToken Consume()
    {
        if (isEnd)
        {
            throw new EndOfStreamException("Already ready the last token");
        }

        var c = current;

        if (!reader.IsEof)
        {
            Next();
        }

        else isEnd = true;

        return c;
    }

    public CssToken Next()
    {
        if (_stack.Count > 0)
        {
            current = _stack.Pop();
        }
        else
        {
            current = ReadInternal();
        }

        return current;
    }

    private CssToken ReadInternal()
    {
        if (reader.IsEof)
        {
            throw new Exception($"Cannot read past EOF. Current: {current}.");
        }

        if (char.IsWhiteSpace(reader.Current) || reader.Current is '\uFEFF')
        {
            return ReadTrivia();
        }

        switch (reader.Current)
        {
            case '@':
                return new CssToken(CssTokenKind.AtSymbol, reader.ReadSymbol("@"), reader.Position);
            case '$':
                _mode.Enter(LexicalMode.Symbol);

                return new CssToken(CssTokenKind.Dollar, reader.ReadSymbol("$"), reader.Position);

            case '/':

                return reader.Peek() switch {
                    '/' or '*' => ReadComment(),
                    ' ' => new CssToken(CssTokenKind.Divide, reader.ReadSymbol("/"), reader.Position),
                    _ => new CssToken(CssTokenKind.String, reader.Read(), reader.Position)
                };


            case ':':
                char peek = reader.Peek();
                // Pseudo-elements
                if (peek is ':')
                {
                    return ReadPseudoElementName();
                }
                else if (peek is '-')
                {
                    return ReadValue();
                }
                else
                {
                    var colon = new CssToken(CssTokenKind.Colon, reader.ReadSymbol(":"), reader.Position);

                    return MaybeColonOrPseudoClass(colon);
                }

            case ',':
                return new CssToken(CssTokenKind.Comma, reader.ReadSymbol(","), reader.Position);
            case '~':
                return new CssToken(CssTokenKind.Tilde, reader.ReadSymbol("~"), reader.Position);
            case ';':
                LeaveValueMode();
                return new CssToken(CssTokenKind.Semicolon, reader.ReadSymbol(";"), reader.Position);
            case '{':
                _mode.Enter(LexicalMode.Block);

                return new CssToken(CssTokenKind.BlockStart, reader.ReadSymbol("{"), reader.Position);
            case '}':

                LeaveValueMode();

                if (_mode.Current is LexicalMode.InterpolatedString)
                {
                    _mode.Leave(LexicalMode.InterpolatedString);

                    return new CssToken(CssTokenKind.InterpolatedStringEnd, reader.ReadSymbol("}"), reader.Position);
                }
                else
                {
                    _mode.Leave(LexicalMode.Block, this.Current.Position);

                    return new CssToken(CssTokenKind.BlockEnd, reader.ReadSymbol("}"), reader.Position);
                }

            case '(': return new CssToken(CssTokenKind.LeftParenthesis, reader.ReadSymbol("("), reader.Position);
            case ')': return new CssToken(CssTokenKind.RightParenthesis, reader.ReadSymbol(")"), reader.Position);

            case '&':
                return reader.Peek() is '&'
                    ? new CssToken(CssTokenKind.And, reader.Read(2), reader.Position - 1)
                    : new CssToken(CssTokenKind.Ampersand, reader.ReadSymbol("&"), reader.Position);

            case '|' when (reader.Peek() is '|'): // ||
                return new CssToken(CssTokenKind.Or, reader.Read(2), reader.Position - 1);

            case '!': // !=
                if (reader.Peek() is '=') return new CssToken(CssTokenKind.NotEquals, reader.Read(2), reader.Position - 1);

                else break;
            case '=' when (reader.Peek() is '='): // ==
                return new CssToken(CssTokenKind.Equals, reader.Read(2), reader.Position - 1);

            case '>': // >=
                return reader.Peek() is '='
                    ? new CssToken(CssTokenKind.Gte, reader.Read(2), reader.Position - 1)
                    : new CssToken(CssTokenKind.Gt, reader.ReadSymbol(">"), reader.Position);

            case '<': // <=
                return reader.Peek() is '='
                    ? new CssToken(CssTokenKind.Lte, reader.Read(2), reader.Position - 1)
                    : new CssToken(CssTokenKind.Lt, reader.ReadSymbol("<"), reader.Position);

            case '#' when reader.Peek() is '{':
                _mode.Enter(LexicalMode.InterpolatedString);

                return new CssToken(CssTokenKind.InterpolatedStringStart, reader.Read(2), reader.Position - 1);

            case '+' when reader.Peek() is ' ':
                return new CssToken(CssTokenKind.Add, reader.ReadSymbol("+"), reader.Position);
            case '*':
                return new CssToken(CssTokenKind.Multiply, reader.ReadSymbol("*"), reader.Position);
            case '.' when char.IsAsciiDigit(reader.Peek()):
                return ReadNumber();

            case '-':
                if (char.IsAsciiDigit(reader.Peek()))
                    return ReadNumber();
                else if (reader.Peek() is ' ')
                    return new CssToken(CssTokenKind.Subtract, reader.ReadSymbol("-"), reader.Position);
                else
                    break;

            case '\'' or '"':
                return ReadQuotedString(reader.Current);
            case '0' or '1' or '2' or '3' or '4' or '5' or '6' or '7' or '8' or '9':
                return ReadNumber();

        }

        return _mode.Current switch {
            LexicalMode.Symbol => ReadSymbol(),
            LexicalMode.Value => ReadValue(),
            LexicalMode.Unit => ReadUnit(),
            _ => ReadName()
        };
    }

    private CssToken ReadUnit()
    {
        reader.Mark();

        while (char.IsAsciiLetter(reader.Current) || reader.Current is '%')
        {
            if (reader.IsEof) throw SyntaxException.UnexpectedEOF("Name");

            reader.Advance();
        }

        _mode.Leave(LexicalMode.Unit);

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

            reader.Advance();
        }

        _mode.Leave(LexicalMode.Symbol);

        return new CssToken(CssTokenKind.Name, reader.Unmark(), reader.MarkStart);
    }

    private CssToken ReadName()
    {
        reader.Mark();

        while (!char.IsWhiteSpace(reader.Current) && !(reader.Current is '{' or '}' or '(' or ')' or ';' or ':' or ',' or SourceReader.EofChar))
        {
            if (reader.Current is '#' && reader.Peek() is '{')
            {
                break;
            }
            else if (reader.Current is '[' && reader.Position > reader.MarkStart)
            {
                break;
            }

            reader.Advance();
        }

        return new CssToken(CssTokenKind.Name, reader.Unmark(), reader.MarkStart);
    }

    private CssToken ReadPseudoElementName()
    {
        reader.Mark();

        while (reader.Current is ':' || char.IsAsciiLetter(reader.Current))
        {
            reader.Advance();
        }

        return new CssToken(CssTokenKind.Name, reader.Unmark(), reader.MarkStart);
    }

    private void LeaveValueMode()
    {
        if (_mode.Current is LexicalMode.Value)
        {
            _mode.Leave(LexicalMode.Value, this.Current.Position);
        }
    }

    private CssToken ReadQuotedString(char quoteChar)
    {
        reader.Mark();

        reader.Advance(); // "

        while (reader.Current != quoteChar && !reader.IsEof)
        {
            reader.Advance();
        }

        reader.Advance(); // "

        return new CssToken(CssTokenKind.String, reader.Unmark(), reader.MarkStart);
    }

    private CssToken MaybeColonOrPseudoClass(CssToken colon)
    {
        if (char.IsWhiteSpace(reader.Current))
        {
            _mode.Enter(LexicalMode.Value);

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
            reader.Advance();
        }

        var text = reader.Unmark();

        if (reader.Current is '(' || PseudoClassNames.Contains(text) || PseudoElementNames.Contains(text))
        {
            return new CssToken(CssTokenKind.Name, ":" + text, reader.MarkStart);
        }

        else
        {
            // We'll return this on the next read...

            _stack.Push(new CssToken(CssTokenKind.String, text, reader.MarkStart));

            _mode.Enter(LexicalMode.Value);

            return colon;
        }
    }

    private static readonly SearchValues<char> BreakChars = SearchValues.Create(['{', '}', '(', ')', ';', ',', SourceReader.EofChar]);

    private CssToken ReadValue()
    {
        reader.Mark();

        while (!char.IsWhiteSpace(reader.Current) && !BreakChars.Contains(reader.Current))
        {
            reader.Advance();
        }

        var text = reader.Unmark();

        if (PseudoClassNames.Contains(text) || PseudoElementNames.Contains(text))
        {
            return new CssToken(CssTokenKind.Name, text, reader.MarkStart);
        }

        return new CssToken(CssTokenKind.String, text, reader.MarkStart);
    }

    private CssToken ReadNumber()
    {
        reader.Mark();

        // Read a leading '-'
        if (reader.Current is '-') reader.Advance();

        while ((char.IsAsciiDigit(reader.Current) || reader.Current is '.') && !reader.IsEof)
        {
            reader.Advance();
        }

        if (reader.Current is '%' || char.IsAsciiLetter(reader.Current))
        {
            _mode.Enter(LexicalMode.Unit);
        }

        return new CssToken(CssTokenKind.Number, reader.Unmark(), reader.MarkStart);
    }

    private CssToken ReadTrivia()
    {
        reader.Mark();

        // \uFEFF : zero with non-breaking space

        while ((char.IsWhiteSpace(reader.Current) || reader.Current is '\uFEFF') && !reader.IsEof)
        {
            reader.Advance();
        }

        return new CssToken(CssTokenKind.Whitespace, reader.Unmark(), reader.MarkStart);
    }

    private CssToken ReadComment()
    {
        /* */

        reader.Mark();

        reader.Advance();                  // read /

        if (reader.Current is '/')
        {
            return ReadLineComment();
        }

        reader.Advance();                  // read *

        while (!reader.IsEof)
        {
            if (reader.Current is '*' && reader.Peek() is '/')
            {
                break;
            }

            reader.Advance();
        }

        reader.Advance(); // read *
        reader.Advance(); // read /

        return new CssToken(CssTokenKind.Comment, reader.Unmark(), reader.MarkStart);
    }

    private CssToken ReadLineComment()
    {
        // line comment

        reader.Advance(); // read /

        bool isDirective = reader.Current is '=';

        while (!(reader.Current is '\n' or '\r'))
        {
            if (reader.IsEof) break;

            reader.Advance();
        }

        return new CssToken(isDirective ? CssTokenKind.Directive : CssTokenKind.Comment, reader.Unmark(), reader.MarkStart);
    }

    public void Dispose()
    {
        reader.Dispose();
    }
}

// https://www.w3.org/TR/css-syntax/#typedef-dimension-token