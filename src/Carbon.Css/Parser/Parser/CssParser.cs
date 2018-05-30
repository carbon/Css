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

        private CssToken Current => tokenizer.Current;

        private bool IsEnd => tokenizer.IsEnd;

        public CssToken Read()
        {
            return tokenizer.Read();
        }

        public CssToken Read(TokenKind expect, LexicalMode mode)
        {
            if (Current.Kind != expect)
            {
                throw new UnexpectedTokenException(mode, expect, Current);
            }

            return Read();
        }

        #endregion

        public IEnumerable<CssNode> ReadNodes()
        {
            while (!IsEnd)
            {
                ReadTrivia();

                yield return ReadNode();
            }
        }

        public CssNode ReadNode()
        {
            switch (Current.Kind)
            {
                case TokenKind.Directive               : return ReadDirective();
                case TokenKind.AtSymbol                : return ReadAtRule();
                case TokenKind.Dollar                  : return ReadAssignment();
                case TokenKind.InterpolatedStringStart : return ReadInterpolatedString();
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
            switch (Current.Kind)
            {
                case TokenKind.Name     : return ReadStyleRule();
                case TokenKind.AtSymbol : return ReadAtRule();

                default: throw new UnexpectedTokenException(LexicalMode.Rule, Current);
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

            var atName = Read();                        // read name

            ReadTrivia();

            switch (atName.Text)
            {
                case "charset"   : return ReadCharsetRule();
                case "import"    : return ReadImportRule();
                case "font-face" : return ReadFontFaceRule();
                case "media"     : return ReadMediaRule();
                case "page"      : return ReadPageRule();
                case "keyframes" : return ReadKeyframesRule();
                case "mixin"     : return ReadMixinBody();
                case "if"        : return ReadIfRule();
                case "for"       : return ReadForRule();
                case "each"      : return ReadEachRule();
                case "while"     : return ReadWhileRule();
            }

            TokenList text = null;

            if (Current.Kind == TokenKind.Name)
            {
                text = ReadSpan();
            }

            var rule = new UnknownRule(atName.Text, text);

            switch (Current.Kind)
            {
                case TokenKind.BlockStart: ReadBlock(rule); break; // {
                case TokenKind.Semicolon: tokenizer.Read(); break; // ;
            }

            return rule;
        }


        public PageRule ReadPageRule()
        {
            var rule = new PageRule();

            switch (Current.Kind)
            {
                case TokenKind.BlockStart: ReadBlock(rule); break; // {
                case TokenKind.Semicolon : tokenizer.Read(); break; // ;
            }

            return rule;

            /*
            @page {
                margin: 1cm;
            }
            */
        }

        public CharsetRule ReadCharsetRule()
        {
            // @charset "UTF-8";

            string selectorText = null;

            if (Current.Kind == TokenKind.Name)
            {
                selectorText = ReadSpan().ToString();
            }

            return new CharsetRule(selectorText);
        }

        public KeyframesRule ReadKeyframesRule()
        {
            // @media only screen and (min-width : 1600px) {

            var span = new TokenList();

            while (Current.Kind != TokenKind.BlockStart && !IsEnd)
            {
                span.Add(Read()); // Read the token
            }

            var rule = new KeyframesRule(span.ToString());

            ReadBlock(rule);

            return rule;
        }

        public CssRule ReadMediaRule()
        {
            // @media {

            var span = new TokenList();

            while (Current.Kind != TokenKind.BlockStart && !IsEnd)
            {
                span.Add(Read());
            }

            var rule = new MediaRule(span);

            ReadBlock(rule);

            return rule;
        }

        public CssRule ReadFontFaceRule()
        {
            // @font-face {

            while (Current.Kind != TokenKind.BlockStart && !IsEnd)
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

            if (Current.Kind == TokenKind.Semicolon)
            {
                Read();
            }

            rule.Trailing = ReadTrivia();

            return rule;
        }

        #endregion

        #region Sass Rules


        public IfBlock ReadIfRule()
        {
            // @font-face [expression] {

            ReadTrivia();

            var condition = ReadExpression();

            var rule = new IfBlock(condition);

            ReadBlock(rule);

            return rule;
        }

        // @each $shape in $shapes
        public EachBlock ReadEachRule()
        {
            ReadTrivia();

            var variable = ReadVariable();

            ReadName(); // ! in

            var enumerable = ReadExpression();

            var rule = new EachBlock(variable, enumerable);

            ReadBlock(rule);

            return rule;
        }

        // @for $i from 1 through $grid-columns
        public ForBlock ReadForRule()
        {
            ReadTrivia();

            var variable = ReadVariable();

            ReadName(); // ! in

            var start = ReadExpression();

            ReadName(); // through or to

            var end = ReadExpression();

            var rule = new ForBlock(variable, start, end);

            ReadBlock(rule);

            return rule;
        }

        public WhileBlock ReadWhileRule()
        {
            ReadTrivia();

            var condition = ReadExpression();

            var rule = new WhileBlock(condition);

            ReadBlock(rule);

            return rule;
        }

        public CssInterpolatedString ReadInterpolatedString()
        {
            Read(); // #{

            var expression = ReadExpression();

            Read(); // }

            return new CssInterpolatedString(expression) { Trailing = ReadTrivia() };
        }

        #endregion

        #region Values

        // Read comma seperated values

        public CssValue ReadValueList()
        {
            // : #fffff
            // : $oranges
            // : url(file.css);

            List<CssValue> values = null;

            CssValue first = CssValue.FromComponents(ReadComponents());

            while (Current.Kind == TokenKind.Comma)
            {
                Read();         // read ,

                ReadTrivia();   // read trivia

                if (values == null)
                {
                    values = new List<CssValue>();

                    values.Add(first);
                }

                values.Add(CssValue.FromComponents(ReadComponents()));
            }

            if (values == null) return first;

            return new CssValueList(values, ValueSeperator.Comma);
        }

        public IEnumerable<CssValue> ReadComponents()
        {
            while (!IsEnd)
            {
                var component = ReadComponent();

                if (Current.IsBinaryOperator)
                {
                    yield return ReadExpressionFrom(component);
                }
                else
                {
                    yield return component;
                }

                if (Current.Kind == TokenKind.BlockStart
                    || Current.Kind == TokenKind.BlockEnd
                    || Current.Kind == TokenKind.Semicolon
                    || Current.Kind == TokenKind.Comma
                    || Current.Kind == TokenKind.RightParenthesis)
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

            switch (Current.Kind)
            {
                case TokenKind.Dollar                  : return ReadVariable();
                case TokenKind.Number                  : return ReadNumberOrMeasurement();
                case TokenKind.InterpolatedStringStart : return ReadInterpolatedString();
            }

            CssToken value = Read();  // read string|number

            // Function
            // A functional notation is a type of component value that can represent more complex types or invoke special processing.
            // The syntax starts with the name of the function immediately followed by a left parenthesis (i.e. a FUNCTION token) followed by the argument(s) 
            // to the notation followed by a right parenthesis. 
            // White space is allowed, but optional, immediately inside the parentheses. 
            // If a function takes a list of arguments, the arguments are separated by a comma (‘,’) with optional whitespace before and after the comma.

            if (Current.Kind == TokenKind.LeftParenthesis)
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
            var value = double.Parse(tokenizer.Read().Text);   // read number

            if (Current.Kind == TokenKind.Unit)
            {
                var unit = CssUnit.Get(tokenizer.Read().Text);

                return new CssUnitValue(value, unit)
                {
                    Trailing = ReadTrivia()
                };
            }

            return new CssUnitValue(value, CssUnit.Number)
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

            if (Current.Kind == TokenKind.Semicolon)
            {
                Read(); // read ;
            }

            ReadTrivia();

            return new CssAssignment(name, value);
        }

        #endregion

        public CssSelector ReadSelector()
        {
            var list = new List<TokenList>();

            list.Add(ReadSpan());

            // Maybe a multi-selector
            while (Current.Kind == TokenKind.Comma)
            {
                Read();

                list.Add(ReadSpan());
            }

            // #id.hello { } 

            return new CssSelector(list);
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

            IReadOnlyList<CssParameter> parameters = (Current.Kind == TokenKind.LeftParenthesis)
                ? ReadParameterList()
                : (IReadOnlyList<CssParameter>)Array.Empty<CssParameter>();
            
            var mixin = new MixinNode(name.Text, parameters);

            ReadBlock(mixin);

            return mixin;
        }

        private List<CssParameter> ReadParameterList()
        {
            // ($color, $width: 1in)

            Read(); // (

            var list = new List<CssParameter>();

            while (Current.Kind != TokenKind.RightParenthesis && !IsEnd)
            {
                Read(TokenKind.Dollar, LexicalMode.Unknown);

                var name = Read();

                CssValue @default = null;

                ReadTrivia();

                if (Current.Kind == TokenKind.Colon)
                {
                    Read();         // read :

                    ReadTrivia();   // read trivia

                    @default = CssValue.FromComponents(ReadComponents());
                }

                ReadTrivia();

                if (Current.Kind == TokenKind.Comma)
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
            while (Current.Kind != TokenKind.BlockEnd && !IsEnd)
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

            if (Current.Kind == TokenKind.LeftParenthesis)
            {
                Read(TokenKind.LeftParenthesis, LexicalMode.Function);

                args = ReadValueList();

                Read(TokenKind.RightParenthesis, LexicalMode.Function);
            }

            var trivia = ReadTrivia();

            if (Current.Kind == TokenKind.Semicolon)
            {
                tokenizer.Read(); // Read ;
            }

            return new IncludeNode(name.Text, args)
            {
                Leading = ReadTrivia()
            };
        }

        #endregion

        public CssRule ReadRuleBlock(in CssSelector selector)
        {
            var rule = new StyleRule(selector);

            ReadBlock(rule);

            return rule;
        }

        public CssBlock ReadBlock(CssBlock block)
        {
            var blockStart = Read(TokenKind.BlockStart, LexicalMode.Block); // Read {

            ReadTrivia();

            while (Current.Kind != TokenKind.BlockEnd)
            {
                if (IsEnd) throw new UnbalancedBlock(LexicalMode.Block, blockStart);

                // A list of delarations or blocks

                if (Current.Kind == TokenKind.AtSymbol)
                {
                    Read(); // Read @

                    var name = tokenizer.Read(); // Name

                    switch (name.Text)
                    {
                        case "include" : block.Add(ReadInclude()); continue;
                        case "if"      : block.Add(ReadIfRule());  continue;
                    }
                }

                if (Current.Kind == TokenKind.Dollar)
                {
                    block.Add(ReadAssignment());

                    continue;
                }

                var span = ReadSpan();
                var spanList = new List<TokenList>();

                spanList.Add(span);

                while (Current.Kind == TokenKind.Comma)
                {
                    Read(); // ,

                    spanList.Add(ReadSpan());

                }

                switch (Current.Kind)
                {
                    case TokenKind.Colon      : block.Add(ReadDeclarationFromName(span));           break; // DeclarationName
                    case TokenKind.BlockStart : block.Add(ReadRuleBlock(new CssSelector(spanList))); break;
                    case TokenKind.BlockEnd   : break;

                    // TODO: Figure out where we missed reading the semicolon TEMP
                    case TokenKind.Semicolon    : tokenizer.Read(); break;

                    default: throw new UnexpectedTokenException(LexicalMode.Block, Current);
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

            if (Current.Kind == TokenKind.Semicolon)
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

            if (Current.Kind == TokenKind.Semicolon)
            {
                Read();                                     // read ;
            }

            ReadTrivia();

            return new CssDeclaration(name.ToString(), value);
        }

        public Trivia ReadTrivia()
        {
            if (IsEnd || !Current.IsTrivia) return null;

            var trivia = new Trivia();

            while (Current.IsTrivia && !IsEnd)
            {
                trivia.Add(Read());
            }

            return trivia;
        }

        public string ReadName()
        {
            string name = Read().Text;

            ReadTrivia();

            return name;
        }

        public TokenList ReadSpan()
        {
            var list = new TokenList();

            while (!IsEnd 
                && Current.Kind != TokenKind.Colon 
                && Current.Kind != TokenKind.BlockStart
                && Current.Kind != TokenKind.BlockEnd
                && Current.Kind != TokenKind.Semicolon
                && Current.Kind != TokenKind.Comma)
            {
                list.Add(Read());
            }

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
