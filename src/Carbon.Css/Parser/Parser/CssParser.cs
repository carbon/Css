using System;
using System.Collections.Generic;
using System.IO;

namespace Carbon.Css.Parser
{
    public sealed partial class CssParser : IDisposable
    {
        private readonly CssTokenizer tokenizer;
        private readonly LexicalModeContext context = new LexicalModeContext(LexicalMode.Unknown);

        public CssParser(TextReader textReader)
            : this(new CssTokenizer(new SourceReader(textReader)))
        { }

        public CssParser(string text)
            : this(new CssTokenizer(new SourceReader(new StringReader(text))))
        { }

        public CssParser(CssTokenizer tokenizer)
        {
            this.tokenizer = tokenizer;
        }

        public IEnumerable<CssRule> ReadRules()
        {
            foreach (var node in ReadNodes())
            {
                if (node.Kind == NodeKind.Rule)
                {
                    yield return (CssRule)node;
                }
            }
        }

        #region Tokenizer Helpers

        private CssToken current => tokenizer.Current;

        private bool isEnd => tokenizer.IsEnd;

        public CssToken Read()
        {
            return tokenizer.Read();
        }

        public CssToken Read(TokenKind expect, LexicalMode mode)
        {
            if (current.Kind != expect)
            {
                throw new UnexpectedTokenException(mode, expect, current);
            }

            return Read();
        }

        #endregion

        public IEnumerable<CssNode> ReadNodes()
        {
            while (!isEnd)
            {
                ReadTrivia();

                yield return ReadNode();
            }
        }

        public CssNode ReadNode()
        {
            switch (current.Kind)
            {
                case TokenKind.Directive : return ReadDirective();
                case TokenKind.AtSymbol  : return ReadAtRule();
                case TokenKind.Dollar    : return ReadAssignment();
            }

            var selector = ReadSelector();

            return ReadRuleBlock(selector);
        }

        public CssNode ReadDirective()
        {
            var text = Read().Text;

            // READ: //=
            var parts = text.Substring(3).TrimStart().Split(Seperators.Space, 2);

            //= support Safari 5.1
            return new CssDirective(
                name: parts[0].Trim(),
                value: parts.Length > 1 ? parts[1].Trim() : null
            );
        }

        public CssNode ReadRule()
        {
            switch (current.Kind)
            {
                case TokenKind.Name     : return ReadStyleRule();
                case TokenKind.AtSymbol : return ReadAtRule();

                default: throw new UnexpectedTokenException(LexicalMode.Rule, current);
            }
        }

        #region At Rules

        public CssNode ReadAtRule()
        {
            // ATKEYWORD S* any* [ block | ';' S* ];
            // @{keyword} ... 

            // @import "subs.css";
            // @media print {

            Read(TokenKind.AtSymbol, LexicalMode.Rule); // Read @

            var ruleType = RuleType.Unknown;

            var atName = Read();                        // read name

            ReadTrivia();

            switch (atName.Text)
            {
                case "charset"   : ruleType = RuleType.Charset; break;
                case "import"    : return ReadImportRule();
                case "font-face" : return ReadFontFaceRule();
                case "media"     : return ReadMediaRule();
                case "page"      : ruleType = RuleType.Page; break;
                case "keyframes" : return ReadKeyframesRule();
                case "mixin"     : return ReadMixinBody();
                case "if"        : return ReadIfRule();
            }

            string selectorText = null;

            if (current.Kind == TokenKind.Name)
            {
                selectorText = ReadSpan().ToString();
            }

            var rule = new AtRule(atName.Text, ruleType, selectorText ?? "");

            switch (current.Kind)
            {
                case TokenKind.BlockStart: ReadBlock(rule); break; // {
                case TokenKind.Semicolon: tokenizer.Read(); break; // ;
            }

            return rule;
        }

        public CssRule ReadKeyframesRule()
        {
            // @media only screen and (min-width : 1600px) {

            var span = new TokenList();

            while (current.Kind != TokenKind.BlockStart && !isEnd)
            {
                span.Add(Read()); // Read the token
            }

            var rule = new KeyframesRule(span.RawText.Trim());

            ReadBlock(rule);

            return rule;
        }

        public CssRule ReadMediaRule()
        {
            // @media {

            var span = new TokenList();

            while (current.Kind != TokenKind.BlockStart && !isEnd)
            {
                span.Add(Read());
            }

            var rule = new MediaRule(span.RawText.Trim());

            ReadBlock(rule);

            return rule;
        }

        public CssRule ReadFontFaceRule()
        {
            // @font-face {

            while (current.Kind != TokenKind.BlockStart && !isEnd)
            {
                Read();
            }

            var rule = new FontFaceRule();

            // Declarations

            ReadBlock(rule);

            return rule;
        }

        public CssRule ReadImportRule()
        {
            var value = ReadValueList();

            var rule = new ImportRule(CssUrlValue.Parse(value.ToString()));

            if (current.Kind == TokenKind.Semicolon)
            {
                Read();
            }

            rule.Trailing = ReadTrivia();

            return rule;
        }

        public IfBlock ReadIfRule()
        {
            // @font-face [expression] {

            ReadTrivia();

            var condition = ReadExpression();

            var rule = new IfBlock(condition);

            ReadBlock(rule);

            return rule;
        }

        #endregion

        #region Values

        // Read comma seperated values

        public CssValue ReadValueList()
        {
            // : #fffff
            // : $oranges
            // : url(file.css);

            var values = new List<CssValue>();

            do
            {
                if (current.Kind == TokenKind.Comma)    // read the comma & trailing whitespace
                {
                    Read();

                    ReadTrivia();
                }

                values.Add(CssValue.FromComponents(ReadComponents()));

            } while (current.Kind == TokenKind.Comma);

            var trivia = ReadTrivia(); // Trialing trivia

            if (values.Count == 1) return values[0];

            var list = new CssValueList(values, ValueSeperator.Comma);

            return list;
        }

        public IEnumerable<CssValue> ReadComponents()
        {
            while (!isEnd)
            {
                var component = ReadComponent();

                if (current.IsBinaryOperator)
                {
                    yield return ReadExpressionFrom(component);
                }
                else
                {
                    yield return component;
                }

                if (current.Kind == TokenKind.BlockStart
                    || current.Kind == TokenKind.BlockEnd
                    || current.Kind == TokenKind.Semicolon
                    || current.Kind == TokenKind.Comma
                    || current.Kind == TokenKind.RightParenthesis)
                {
                    break;
                }
            }
        }

        public CssValue ReadComponent()
        {
            // Variable
            // Number
            // Measurement
            // Function
            // Expression

            switch (current.Kind)
            {
                case TokenKind.Dollar: return ReadVariable();
                case TokenKind.Number: return ReadNumberOrMeasurement();
            }

            var value = Read();  // read string|number

            // Function
            // A functional notation is a type of component value that can represent more complex types or invoke special processing.
            // The syntax starts with the name of the function immediately followed by a left parenthesis (i.e. a FUNCTION token) followed by the argument(s) 
            // to the notation followed by a right parenthesis. 
            // White space is allowed, but optional, immediately inside the parentheses. 
            // If a function takes a list of arguments, the arguments are separated by a comma (‘,’) with optional whitespace before and after the comma.

            if (current.Kind == TokenKind.LeftParenthesis)
            {
                return ReadFunctionCall(value);
            }

            // ReadString (consider context)

            // :-ms-input-placeholder
            // #id:before
            // :link
            // :not

            return new CssString(value) {
                Trailing = ReadTrivia()
            };
        }

        public CssFunction ReadFunctionCall(CssToken name)
        {
            Read();                 // (

            ReadTrivia();           // trivia

            var args = ReadValueList();

            Read(TokenKind.RightParenthesis, LexicalMode.Function); // )

            if (name.Text == "url")
            {
                return new CssUrl(name, args) {
                    Trailing = ReadTrivia()
                };
            }

            return new CssFunction(name.Text, args) {
                Trailing = ReadTrivia()
            };
        }

        public CssValue ReadNumberOrMeasurement()
        {
            var value = float.Parse(tokenizer.Read().Text);   // read number

            if (current.Kind == TokenKind.Unit)
            {
                var unit = CssUnit.Get(tokenizer.Read().Text);

                return new CssMeasurement(value, unit)
                {
                    Trailing = ReadTrivia()
                };
            }

            return new CssNumber(value)
            {
                Trailing = ReadTrivia()
            };
        }

        public CssVariable ReadVariable()
        {
            Read(TokenKind.Dollar, LexicalMode.Value);              // read $

            var symbol = Read(TokenKind.Name, LexicalMode.Value);   // read symbol

            return new CssVariable(symbol)
            {
                Leading = ReadTrivia()
            };
        }

        #endregion

        #region Variables

        public CssAssignment ReadAssignment()
        {
            Read(TokenKind.Dollar, LexicalMode.Assignment);             // read $

            var name = Read(TokenKind.Name, LexicalMode.Assignment);    // read name

            ReadTrivia();

            Read(TokenKind.Colon, LexicalMode.Assignment);              // read :

            ReadTrivia();                                               // read trivia

            var value = ReadValueList();

            if (current.Kind == TokenKind.Semicolon)
            {
                Read(); // read ;
            }

            ReadTrivia();

            return new CssAssignment(name, value);
        }

        #endregion

        public CssSelector ReadSelector()
        {
            // #id.hello { } 

            var span = new TokenList();

            while (current.Kind != TokenKind.BlockStart && !isEnd)
            {
                var token = tokenizer.Read();

                // Consider multiselectors
                // if (token.Kind == TokenKind.Comma) ;

                span.Add(token);
            }

            return new CssSelector(span);
        }

        public StyleRule ReadStyleRule()
        {
            var rule = new StyleRule(ReadSelector());

            ReadBlock(rule);

            return rule;
        }

        #region Mixins

        /*
		@mixin left($dist) {
		  float: left;
		  margin-left: $dist;
		}
		*/

        public MixinNode ReadMixinBody()
        {
            var name = Read(); // name or string

            ReadTrivia();

            var parameters = new List<CssParameter>();

            if (current.Kind == TokenKind.LeftParenthesis)
            {
                parameters = ReadParameterList();
            }

            var mixin = new MixinNode(name.Text, parameters);

            ReadBlock(mixin);

            return mixin;
        }

        public List<CssParameter> ReadParameterList()
        {
            // ($color, $width: 1in)

            Read(); // (

            var list = new List<CssParameter>();

            while (current.Kind != TokenKind.RightParenthesis && !isEnd)
            {
                Read(TokenKind.Dollar, LexicalMode.Unknown);

                var name = Read();

                CssValue @default = null;

                ReadTrivia();

                if (current.Kind == TokenKind.Colon)
                {
                    Read();         // read :

                    ReadTrivia();   // read trivia

                    @default = CssValue.FromComponents(ReadComponents());
                }

                ReadTrivia();

                if (current.Kind == TokenKind.Comma)
                {
                    Read(); // read ,
                }

                ReadTrivia();

                list.Add(new CssParameter(name.Text, @default));
            }

            Read(TokenKind.RightParenthesis, LexicalMode.Unknown); // read )

            ReadTrivia();

            return list;
        }

        public IEnumerable<CssDeclaration> ReadDeclartions()
        {
            while (current.Kind != TokenKind.BlockEnd && !isEnd)
            {
                yield return ReadDeclaration();
            }
        }

        public IncludeNode ReadInclude()
        {
            // @include name(args)

            ReadTrivia();

            var name = tokenizer.Read(); // Read the name

            ReadTrivia();

            CssValue args = null;

            if (current.Kind == TokenKind.LeftParenthesis)
            {
                Read(TokenKind.LeftParenthesis, LexicalMode.Function);

                args = ReadValueList();

                Read(TokenKind.RightParenthesis, LexicalMode.Function);
            }

            var trivia = ReadTrivia();

            if (current.Kind == TokenKind.Semicolon)
            {
                tokenizer.Read(); // Read ;
            }

            return new IncludeNode(name.Text, args)
            {
                Leading = ReadTrivia()
            };
        }

        #endregion

        public CssRule ReadRuleBlock(CssSelector selector)
        {
            var rule = new StyleRule(selector);

            ReadBlock(rule);

            return rule;
        }

        public CssBlock ReadBlock(CssBlock block)
        {
            var blockStart = Read(TokenKind.BlockStart, LexicalMode.Block); // Read {

            ReadTrivia();

            while(current.Kind != TokenKind.BlockEnd)
            {
                if (isEnd) throw new UnbalancedBlock(LexicalMode.Block, blockStart);

                // A list of delarations or blocks

                if (current.Kind == TokenKind.AtSymbol)
                {
                    Read(); // Read @

                    var name = tokenizer.Read(); // Name

                    switch (name.Text)
                    {
                        case "include" : block.Add(ReadInclude()); continue;
                        case "if"      : block.Add(ReadIfRule());  continue;
                    }
                }

                if (current.Kind == TokenKind.Dollar)
                {
                    block.Add(ReadAssignment());

                    continue;

                }

                var statement = ReadSpan();

                switch (current.Kind)
                {
                    case TokenKind.Colon      : block.Add(ReadDeclarationFromName(statement));        break; // DeclarationName
                    case TokenKind.BlockStart : block.Add(ReadRuleBlock(new CssSelector(statement))); break;
                    case TokenKind.BlockEnd   : break;

                    // TODO: Figure out where we missed reading the semicolon TEMP
                    case TokenKind.Semicolon    : tokenizer.Read(); break;

                    default: throw new UnexpectedTokenException(LexicalMode.Block, current);
                }
            }

            tokenizer.Read(); // read }

            block.Trailing = ReadTrivia();

            return block;
        }

        public CssDeclaration ReadDeclaration()
        {
            var name = ReadName();                          // read name

            ReadTrivia();                                   // read trivia

            Read(TokenKind.Colon, LexicalMode.Declaration); // read :

            ReadTrivia();                                   // TODO: read as leading trivia

            var value = ReadValueList();                    // read value (value or valuelist)

            if (current.Kind == TokenKind.Semicolon)
            {
                tokenizer.Read();                           // read ;
            }

            ReadTrivia();

            return new CssDeclaration(name, value);
        }

        public CssDeclaration ReadDeclarationFromName(TokenList name)
        {
            Read(TokenKind.Colon, LexicalMode.Declaration); // read :

            ReadTrivia();                                   // TODO: read as leading trivia

            var value = ReadValueList();                    // read value (value or cssvariable)

            if (current.Kind == TokenKind.Semicolon)
            {
                Read();                                     // read ;
            }

            ReadTrivia();

            return new CssDeclaration(name.ToString(), value);
        }

        public Trivia ReadTrivia()
        {
            if (isEnd || !current.IsTrivia) return null;

            var trivia = new Trivia();

            while (current.IsTrivia && !isEnd)
            {
                trivia.Add(Read());
            }

            return trivia;
        }

        public string ReadName()
        {
            string name;

            // Allow leading : on selector identifiers
            if (current.Kind == TokenKind.Colon)
            {
                name = Read().Text + Read().Text;
            }
            else
            {
                name = Read().Text;
            }

            ReadTrivia();

            return name;
        }

        public TokenList ReadSpan()
        {
            var list = new TokenList();

            while (!isEnd)
            {
                list.Add(Read());

                if (current.Kind == TokenKind.Colon
                    || current.Kind == TokenKind.BlockStart
                    || current.Kind == TokenKind.BlockEnd
                    || current.Kind == TokenKind.Semicolon)
                {
                    break;
                }
            }

            ReadTrivia(); // Trialing trivia

            return list;
        }

        public void Dispose()
        {
            tokenizer.Dispose();
        }
    }
}

/*
stylesheet  : [ CDO | CDC | S | statement ]*;
statement   : ruleset | at-rule;
at-rule     : ATKEYWORD S* any* [ block | ';' S* ];
block       : '{' S* [ any | block | ATKEYWORD S* | ';' S* ]* '}' S*;
ruleset     : selector? '{' S* declaration? [ ';' S* declaration? ]* '}' S*;
selector    : any+;
declaration : property S* ':' S* value;
property    : IDENT;
value       : [ any | block | ATKEYWORD S* ]+;
any         : [ IDENT | NUMBER | PERCENTAGE | DIMENSION | STRING
              | DELIM | URI | HASH | UNICODE-RANGE | INCLUDES
              | DASHMATCH | ':' | FUNCTION S* [any|unused]* ')'
              | '(' S* [any|unused]* ')' | '[' S* [any|unused]* ']'
              ] S*;
unused      : block | ATKEYWORD S* | ';' S* | CDO S* | CDC S*;
*/
