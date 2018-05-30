using System;
using System.Collections.Generic;
using System.IO;

namespace Carbon.Css.Parser
{
    public class CssTokenizer : IDisposable
    {
        private readonly SourceReader reader;
        private readonly LexicalModeContext mode;
        private readonly Stack<CssToken> stack = new Stack<CssToken>();

        private CssToken current;
        private bool isEnd = false;

        public CssTokenizer(SourceReader reader, LexicalMode mode = LexicalMode.Selector)
        {
            this.reader = reader ?? throw new ArgumentNullException(nameof(reader));

            this.reader.Next(); // Start the reader

            this.mode = new LexicalModeContext(mode);

            this.Next(); // Load the first token
        }

        public CssToken Current => current;

        public bool IsEnd => isEnd;

        // Returns the current token and advances to the next
        public CssToken Read()
        {
            if (isEnd) throw new EndOfStreamException("Already ready the last token");

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
            if (reader.IsEof) throw new Exception("Cannot read past EOF. Current: " + current.ToString() + ".");

            if (stack.Count > 0)
            {
                return stack.Pop();
            }

            if (char.IsWhiteSpace(reader.Current) || reader.Current == '\uFEFF')
            {
                return ReadTrivia();
            }

            switch (reader.Current)
            {
                case '@': return new CssToken(TokenKind.AtSymbol, reader.Read(), reader.Position);

                case '$':
                    mode.Enter(LexicalMode.Symbol);

                    return new CssToken(TokenKind.Dollar, reader.Read(), reader.Position);

                case '/':
                    var peek = reader.Peek();

                    if (peek == '/' || peek == '*')
                        return ReadComment();
                    else
                        return new CssToken(TokenKind.Divide, reader.Read(), reader.Position);

                case ':':
                    // Pseudo-elements
                    if (reader.Peek() == ':' || reader.Peek() == '-')
                    {
                        return ReadValue();
                    }
                    else
                    {
                        var colon = new CssToken(TokenKind.Colon, reader.Read(), reader.Position);

                        return MaybeColonOrPseudoClass(colon);
                    }

                case ',': return new CssToken(TokenKind.Comma, reader.Read(), reader.Position);
                case ';':
                    LeaveValueMode();
                    return new CssToken(TokenKind.Semicolon, reader.Read(), reader.Position);
                case '{':
                    mode.Enter(LexicalMode.Block);

                    return new CssToken(TokenKind.BlockStart, reader.Read(), reader.Position);
                case '}':

                    LeaveValueMode();

                    if (mode.Current == LexicalMode.InterpolatedString)
                    {
                        mode.Leave(LexicalMode.InterpolatedString);

                        return new CssToken(TokenKind.InterpolatedStringEnd, reader.Read(), reader.Position);
                    }
                    else
                    {
                        mode.Leave(LexicalMode.Block, this);

                        return new CssToken(TokenKind.BlockEnd, reader.Read(), reader.Position);
                    }

                case '(': return new CssToken(TokenKind.LeftParenthesis, reader.Read(), reader.Position);
                case ')': return new CssToken(TokenKind.RightParenthesis, reader.Read(), reader.Position);

                case '&':
                    if (reader.Peek() == '&')
                        return new CssToken(TokenKind.And, reader.Read(2), reader.Position - 1);
                    else
                        return new CssToken(TokenKind.Ampersand, reader.Read(), reader.Position);

                case '|':
                    if (reader.Peek() == '|') return new CssToken(TokenKind.Or, reader.Read(2), reader.Position - 1);

                    else break;

                case '!': // !=
                    if (reader.Peek() == '=') return new CssToken(TokenKind.NotEquals, reader.Read(2), reader.Position - 1);

                    else break;
                case '=': // ==
                    if (reader.Peek() == '=') return new CssToken(TokenKind.Equals, reader.Read(2), reader.Position - 1);

                    else break;

                case '>': // >=
                    if (reader.Peek() == '=') return new CssToken(TokenKind.Gte, reader.Read(2), reader.Position - 1);
                    else return new CssToken(TokenKind.Gt, reader.Read(), reader.Position);

                case '<': // <=
                    if (reader.Peek() == '=') return new CssToken(TokenKind.Lte, reader.Read(2), reader.Position - 1);
                    else return new CssToken(TokenKind.Lt, reader.Read(), reader.Position);

                case '#' when reader.Peek() == '{':
                    mode.Enter(LexicalMode.InterpolatedString);

                    return new CssToken(TokenKind.InterpolatedStringStart, reader.Read(2), reader.Position - 1);

                case '+': return new CssToken(TokenKind.Add, reader.Read(), reader.Position);
                case '*': return new CssToken(TokenKind.Multiply, reader.Read(), reader.Position);

                case '.' when char.IsDigit(reader.Peek()):
                    return ReadNumber();

                case '-':
                    if (char.IsDigit(reader.Peek()))
                        return ReadNumber();
                    else if (reader.Peek() == ' ')
                        return new CssToken(TokenKind.Subtract, reader.Read(), reader.Position);
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

            switch (mode.Current)
            {
                case LexicalMode.Symbol : return ReadSymbol();
                case LexicalMode.Value  : return ReadValue();
                case LexicalMode.Unit   : return ReadUnit();
                default                 : return ReadName();
            }
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

            return new CssToken(TokenKind.Unit, reader.Unmark(), reader.MarkStart);
        }

        private CssToken ReadSymbol()
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
                if (reader.IsEof) throw SyntaxException.UnexpectedEOF("Symbol");

                reader.Next();
            }

            mode.Leave(LexicalMode.Symbol);

            return new CssToken(TokenKind.Name, reader.Unmark(), reader.MarkStart);
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

            return new CssToken(TokenKind.Name, reader.Unmark(), reader.MarkStart);
        }

        private void LeaveValueMode()
        {
            if (mode.Current == LexicalMode.Value)
            {
                mode.Leave(LexicalMode.Value, this);
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

            return new CssToken(TokenKind.String, reader.Unmark(), reader.MarkStart);
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
                return new CssToken(TokenKind.Name, ":" + text, reader.MarkStart);
            }

            else
            {
                // We'll return this on the next read...

                stack.Push(new CssToken(TokenKind.String, text, reader.MarkStart));
                
                mode.Enter(LexicalMode.Value);

                return colon;
            }
        }

        private CssToken ReadValue()
        {
            reader.Mark();

            while (!char.IsWhiteSpace(reader.Current) && 
                reader.Current != '{' &&
                reader.Current != '}'&& 
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
             
                return new CssToken(TokenKind.Name, text, reader.MarkStart);
            }

            return new CssToken(TokenKind.String, text, reader.MarkStart);
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

            return new CssToken(TokenKind.Number, reader.Unmark(), reader.MarkStart);
        }

        private CssToken ReadTrivia()
        {
            reader.Mark();

            // \uFEFF : zero with non-breaking space

            while ((char.IsWhiteSpace(reader.Current) || reader.Current == '\uFEFF') && !reader.IsEof)
            {
                reader.Next();
            }

            return new CssToken(TokenKind.Whitespace, reader.Unmark(), reader.MarkStart);
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

            return new CssToken(TokenKind.Comment, reader.Unmark(), reader.MarkStart);
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

            return new CssToken(isDirective ? TokenKind.Directive : TokenKind.Comment, reader.Unmark(), reader.MarkStart);
        }

        public void Dispose()
        {
            reader.Dispose();
        }
    }
}