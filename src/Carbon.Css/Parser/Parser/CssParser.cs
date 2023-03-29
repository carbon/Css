using System.Globalization;
using System.IO;

namespace Carbon.Css.Parser;

public sealed partial class CssParser : IDisposable
{
    private int depth = 1;
    private readonly CssTokenizer _tokenizer;

    public CssParser(TextReader textReader)
        : this(new CssTokenizer(new SourceReader(textReader)))
    { }

    public CssParser(string text)
        : this(new CssTokenizer(new SourceReader(new StringReader(text))))
    { }

    public CssParser(CssTokenizer tokenizer)
    {
        _tokenizer = tokenizer;
    }

    public IEnumerable<CssRule> ReadRules()
    {
        foreach (var node in ReadNodes())
        {
            if (node.Kind is NodeKind.Rule)
            {
                yield return (CssRule)node;
            }
        }
    }

    #region Tokenizer Helpers

    private CssToken Current => _tokenizer.Current;

    private bool IsEnd => _tokenizer.IsEnd;

    public CssToken Consume()
    {
        return _tokenizer.Consume();
    }

    public bool ConsumeIf(CssTokenKind kind)
    {
        if (_tokenizer.Current.Kind == kind)
        {
            _tokenizer.Consume();

            return true;
        }

        return false;
    }

    public CssToken Consume(CssTokenKind expect, LexicalMode mode)
    {
        if (Current.Kind != expect)
        {
            throw new UnexpectedTokenException(mode, expect, Current);
        }

        return Consume();
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
            case CssTokenKind.Directive               : return ReadDirective();
            case CssTokenKind.AtSymbol                : return ReadAtRule();
            case CssTokenKind.Dollar                  : return ReadAssignment();
            case CssTokenKind.InterpolatedStringStart : return ReadInterpolatedString();
        }

        var selector = ReadSelector();

        return ReadRuleBlock(selector);
    }

    public CssNode ReadDirective()
    {
        // skip '//= '
        var text = Consume().Text.AsSpan(3).TrimStart();
            
        int spaceIndex = text.IndexOf(' ');

        //= support Safari 5.1

        return new CssDirective(
            name  : spaceIndex > 0 ? text[0..spaceIndex].Trim().ToString() : text.ToString(),
            value : spaceIndex > 0 ? text.Slice(spaceIndex + 1).Trim().ToString() : null
        );
    }

    public CssNode ReadRule()
    {
        return Current.Kind switch
        {
            CssTokenKind.Name     => ReadStyleRule(),
            CssTokenKind.AtSymbol => ReadAtRule(),
            _                     => throw new UnexpectedTokenException(LexicalMode.Rule, Current),
        };
    }

    #region At Rules

    public CssNode ReadAtRule()
    {
        // ATKEYWORD S* any* [ block | ';' S* ];
        // @{keyword} ... 

        // @import "subs.css";
        // @media print {

        Consume(CssTokenKind.AtSymbol, LexicalMode.Rule); // Read @

        var atName = Consume();                        // read name

        ReadTrivia();

        switch (atName.Text)
        {
            case "charset"   : return ReadCharsetRule();
            case "import"    : return ReadImportRule();
            case "font-face" : return ReadFontFaceRule();
            case "media"     : return ReadMediaRule();
            case "page"      : return ReadPageRule();
            case "supports"  : return ReadSupportsRule();
            case "container" : return ReadContainerRule(); 
            case "keyframes" : return ReadKeyframesRule();
            case "mixin"     : return ReadMixinBody();
            case "if"        : return ReadIfRule();
            case "for"       : return ReadForRule();
            case "each"      : return ReadEachRule();
            case "while"     : return ReadWhileRule();
        }

        TokenList? text = null;

        if (Current.Kind is CssTokenKind.Name)
        {
            text = ReadTokenSpan();
        }

        var rule = new UnknownRule(atName.Text, text);

        switch (Current.Kind)
        {
            case CssTokenKind.BlockStart : ReadBlock(rule); break; // {
            case CssTokenKind.Semicolon  : Consume();       break; // ;
        }

        return rule;
    }


    public PageRule ReadPageRule()
    {
        var rule = new PageRule();
            
        // TODO: read optional selector

        switch (Current.Kind)
        {
            case CssTokenKind.BlockStart: ReadBlock(rule);  break; // {
            case CssTokenKind.Semicolon : _tokenizer.Consume(); break; // ;
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

        string? selectorText = null;

        if (Current.Kind is CssTokenKind.Name)
        {
            selectorText = ReadTokenSpan().ToString();
        }

        return new CharsetRule(selectorText.AsMemory());
    }

    public KeyframesRule ReadKeyframesRule()
    {        
        var span = new TokenList();

        while (Current.Kind is not CssTokenKind.BlockStart && !IsEnd)
        {
            span.Add(Consume()); // Read the token
        }

        var rule = new KeyframesRule(span.ToString());

        ReadBlock(rule);

        return rule;
    }

    public MediaRule ReadMediaRule()
    {
        // @media {

        var span = new TokenList();

        while (Current.Kind is not CssTokenKind.BlockStart && !IsEnd)
        {
            span.Add(Consume());
        }

        var rule = new MediaRule(span);

        ReadBlock(rule);

        return rule;
    }

    public ContainerRule ReadContainerRule()
    {
        // @container {

        var span = new TokenList();

        while (Current.Kind is not CssTokenKind.BlockStart && !IsEnd)
        {
            span.Add(Consume());
        }

        var rule = new ContainerRule(span);

        ReadBlock(rule);

        return rule;
    }

    public SupportsRule ReadSupportsRule()
    {
        // @supports {

        var span = new TokenList();

        while (Current.Kind is not CssTokenKind.BlockStart && !IsEnd)
        {
            span.Add(Consume());
        }

        var rule = new SupportsRule(span);

        ReadBlock(rule);

        return rule;
    }

    public FontFaceRule ReadFontFaceRule()
    {
        // @font-face {

        while (Current.Kind != CssTokenKind.BlockStart && !IsEnd)
        {
            Consume();
        }

        var rule = new FontFaceRule();

        // Declarations

        ReadBlock(rule);

        return rule;
    }

    public ImportRule ReadImportRule()
    {
        var value = ReadValueList();

        var rule = new ImportRule(CssUrlValue.Parse(value.ToString().AsSpan()));

        ConsumeIf(CssTokenKind.Semicolon); // ? ;

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
        var variables = new List<CssVariable>();

        do
        {
            ReadTrivia();

            variables.Add(ReadVariable());

        } while (ConsumeIf(CssTokenKind.Comma));

        ReadName(); // ! in

        var enumerable = ReadExpression();

        var rule = new EachBlock(variables, enumerable);

        ReadBlock(rule);

        return rule;
    }

    // @for $i from 1 through $grid-columns
    public ForBlock ReadForRule()
    {
        ReadTrivia();

        var variable = ReadVariable();

        ReadName(); // ! in

        CssValue start = ReadExpression();

        string name = ReadName(); // through (inclusive) or to (exclusive)

        CssValue end = ReadExpression();

        var rule = new ForBlock(variable, start, end, isInclusive: name is "through");

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
        Consume(); // #{

        var expression = ReadExpression();

        Consume(); // }

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

        List<CssValue>? values = null;

        CssValue first = CssValue.FromComponents(ReadComponents());
            
        while (ConsumeIf(CssTokenKind.Comma)) // ? ,
        {
            ReadTrivia(); // ? {trivia}

            values ??= new List<CssValue> {
                first
            };
            
            values.Add(CssValue.FromComponents(ReadComponents()));
        }

        if (values is null) return first;

        return new CssValueList(values, CssValueSeperator.Comma);
    }

    public IEnumerable<CssValue> ReadComponents()
    {
        while (!IsEnd)
        {
            yield return ReadExpression();
                
            if (Current.Kind 
                is CssTokenKind.BlockStart
                or CssTokenKind.BlockEnd
                or CssTokenKind.Semicolon
                or CssTokenKind.Comma
                or CssTokenKind.RightParenthesis)
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
            case CssTokenKind.Dollar                  : return ReadVariable();
            case CssTokenKind.Number                  : return ReadNumberOrMeasurement();
            case CssTokenKind.InterpolatedStringStart : return ReadInterpolatedString();
        }

        CssToken value = Consume();  // read string|number

        // Function
        // A functional notation is a type of component value that can represent more complex types or invoke special processing.
        // The syntax starts with the name of the function immediately followed by a left parenthesis (i.e. a FUNCTION token) followed by the argument(s) 
        // to the notation followed by a right parenthesis. 
        // White space is allowed, but optional, immediately inside the parentheses. 
        // If a function takes a list of arguments, the arguments are separated by a comma (‘,’) with optional whitespace before and after the comma.

        if (Current.Kind is CssTokenKind.LeftParenthesis)
        {
            return ReadFunctionCall(value);
        }

        // ReadString (consider context)

        // :-ms-input-placeholder
        // #id:before
        // :link
        // :not

        return new CssString(value, trailing: ReadTrivia());
    }

    public CssFunction ReadFunctionCall(CssToken name)
    {
        Consume();              // ! (

        ReadTrivia();           // ? trivia

        var args = ReadValueList();

        Consume(CssTokenKind.RightParenthesis, LexicalMode.Function); // )

        if (name.Text is "url")
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
        double value = double.Parse(_tokenizer.Consume().Text, CultureInfo.InvariantCulture);   // read number

        if (Current.Kind is CssTokenKind.Unit)
        {
            var unit = _tokenizer.Consume().Text;

            return new CssUnitValue(value, unit) {
                Trailing = ReadTrivia()
            };
        }

        var result = CssValue.Number(value);

        result.Trailing = ReadTrivia();
            
        return result;
    }

    public CssVariable ReadVariable()
    {
        Consume(CssTokenKind.Dollar, LexicalMode.Value);              // read $

        var symbol = Consume(CssTokenKind.Name, LexicalMode.Value);   // read symbol

        return new CssVariable(symbol) {
            Leading = ReadTrivia()
        };
    }

    #endregion

    #region Variables

    public CssAssignment ReadAssignment()
    {
        Consume(CssTokenKind.Dollar, LexicalMode.Assignment);          // ! $

        var name = Consume(CssTokenKind.Name, LexicalMode.Assignment); // ! {name}

        ReadTrivia();

        Consume(CssTokenKind.Colon, LexicalMode.Assignment);           // ! :

        ReadTrivia();                                                  // read trivia


        CssValue value = _tokenizer.Current.Kind switch {
            CssTokenKind.LeftParenthesis => ReadMap(),
            _                            => ReadValueList()
        };

        ConsumeIf(CssTokenKind.Semicolon);                             // ? ;

        ReadTrivia();

        return new CssAssignment(name, value);
    }

    public CssMap ReadMap()
    {
        // ("regular": 400, "medium": 500, "bold": 700)

        var map = new CssMap();

        Consume(CssTokenKind.LeftParenthesis, LexicalMode.Value); // (

        while (_tokenizer.Current.Kind != CssTokenKind.RightParenthesis)
        {
            ReadTrivia();

            var key = Consume();            

            Consume(CssTokenKind.Colon, LexicalMode.Value);

            ReadTrivia();

            var value = CssValue.FromComponents(ReadComponents());

            map.Add(key.Text.Trim('"'), value);

            _ = ConsumeIf(CssTokenKind.Comma);
        }

        Consume(CssTokenKind.RightParenthesis, LexicalMode.Value); // )

        return map;
    }

    #endregion

    public CssSelector ReadSelector()
    {
        List<CssSequence>? list = null;

        var span = ReadValueSpan();

        // Maybe a multi-selector
        while (ConsumeIf(CssTokenKind.Comma)) // ? ,
        {
            list ??= new List<CssSequence> {
                span
            };            
                
            ReadTrivia(); // trailing whitespace

            list.Add(ReadValueSpan());
        }

        // #id.hello { } 

        return new CssSelector(list ?? (IReadOnlyList<CssSequence>) new[] { span });
    }

    public StyleRule ReadStyleRule()
    {
        var rule = new StyleRule(ReadSelector());

        ReadBlock(rule);

        rule.Depth = depth;


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
        var name = Consume(); // name or string

        ReadTrivia();

        IReadOnlyList<CssParameter> parameters = Current.Kind is CssTokenKind.LeftParenthesis
            ? ReadParameterList()
            : Array.Empty<CssParameter>();
            
        var mixin = new MixinNode(name.Text, parameters);

        ReadBlock(mixin);

        return mixin;
    }

    private List<CssParameter> ReadParameterList()
    {
        // ($color, $width: 1in)

        Consume(); // ! (

        var list = new List<CssParameter>();

        while (Current.Kind is not CssTokenKind.RightParenthesis && !IsEnd)
        {
            Consume(CssTokenKind.Dollar, LexicalMode.Unknown);

            var name = Consume();

            CssValue? @default = null;

            ReadTrivia();

            if (ConsumeIf(CssTokenKind.Colon)) // ? :
            {
                ReadTrivia();               // ? {trivia}

                @default = CssValue.FromComponents(ReadComponents());
            }

            ReadTrivia();

            if (ConsumeIf(CssTokenKind.Comma)) // ? ,
            {
                ReadTrivia();
            }

            list.Add(new CssParameter(name.Text, @default));
        }

        Consume(CssTokenKind.RightParenthesis, LexicalMode.Unknown); // ! )

        ReadTrivia();

        return list;
    }

    public IEnumerable<CssDeclaration> ReadDeclartions()
    {
        while (Current.Kind is not CssTokenKind.BlockEnd && !IsEnd)
        {
            yield return ReadDeclaration();
        }
    }

    public IncludeNode ReadInclude()
    {
        // @include name(args)

        ReadTrivia();

        var name = _tokenizer.Consume(); // ! {name}

        ReadTrivia();

        CssValue? args = null;

        if (ConsumeIf(CssTokenKind.LeftParenthesis)) // ? (
        {
            args = ReadValueList();

            Consume(CssTokenKind.RightParenthesis, LexicalMode.Function); // ! )
        }

        _ = ReadTrivia();

        ConsumeIf(CssTokenKind.Semicolon); // ? ;

        return new IncludeNode(name.Text, args)
        {
            Leading = ReadTrivia()
        };
    }

    #endregion

    public CssRule ReadRuleBlock(in CssSelector selector)
    {
        var rule = new StyleRule(selector)
        {
            Depth = depth
        };

        ReadBlock(rule);
            
        return rule;
    }

    public CssBlock ReadBlock(CssBlock block)
    {
        depth++;

        var blockStart = Consume(CssTokenKind.BlockStart, LexicalMode.Block); // ! {

        ReadTrivia();

        while (Current.Kind is not CssTokenKind.BlockEnd)
        {
            if (IsEnd) throw new UnbalancedBlock(blockStart);

            // A list of delarations or blocks

            if (ConsumeIf(CssTokenKind.AtSymbol)) // ? @
            {
                var name = _tokenizer.Consume(); // Name

                switch (name.Text)
                {
                    case "include" :
                        block.Flags |= CssBlockFlags.HasIncludes;
                        block.Add(ReadInclude()); continue;
                    case "if"      : block.Add(ReadIfRule());  continue;
                    case "page"    : block.Add(ReadPageRule()); continue;
                    case "media"   : 
                        block.Flags |= CssBlockFlags.HasChildMedia;
                        block.Add(ReadMediaRule()); continue;
                    default        : throw new Exception($"Unexpected rule reading block. Was {name.Text}");
                }
            }

            if (Current.Kind is CssTokenKind.Dollar)
            {
                block.Add(ReadAssignment());

                continue;
            }


            var span = ReadValueSpan();

            List<CssSequence>? spanList = null;
                
            while (ConsumeIf(CssTokenKind.Comma)) // ? ,
            {
                spanList ??= new List<CssSequence> { span };
                    
                ReadTrivia(); // ? {trivia}

                spanList.Add(ReadValueSpan());
            }

            switch (Current.Kind)
            {
                case CssTokenKind.Colon: 
                    block.Add(ReadDeclarationFromName(span[0].ToString()!));  break; // DeclarationName
                case CssTokenKind.BlockStart:
                    block.Flags |= CssBlockFlags.HasChildBlocks;
                    block.Add(ReadRuleBlock(new CssSelector(spanList ?? (IReadOnlyList<CssSequence>) new[] { span }))); 
                    break;
                case CssTokenKind.BlockEnd: 
                    break;

                // TODO: Figure out where we missed reading the semicolon TEMP
                case CssTokenKind.Semicolon    : _tokenizer.Consume(); break;

                default: throw new UnexpectedTokenException(LexicalMode.Block, Current);
            }
        }

        _tokenizer.Consume(); // read }

        block.Trailing = ReadTrivia();

        depth--;

        return block;
    }

    public CssDeclaration ReadDeclaration()
    {
        var name = ReadName();                              // ! {name}

        ReadTrivia();                                       // ? trivia

        Consume(CssTokenKind.Colon, LexicalMode.Declaration);  // ! :

        ReadTrivia();                                       // TODO: read as leading trivia

        var value = ReadValueList();                        // read value (value or valuelist)

        ConsumeIf(CssTokenKind.Semicolon);                     // ? ;


        ReadTrivia();

        return new CssDeclaration(name, value);
    }

    public CssDeclaration ReadDeclarationFromName(string name)
    {
        Consume(CssTokenKind.Colon, LexicalMode.Declaration); // ! :
            
        ReadTrivia();                                         // TODO: read as leading trivia

        var value = ReadValueList();                          // read value (value or cssvariable)
            
        ConsumeIf(CssTokenKind.Semicolon);                    // ? ;

        ReadTrivia();

        return new CssDeclaration(name, value);
    }

    public Trivia? ReadTrivia()
    {
        if (IsEnd || !Current.IsTrivia)
        {
            return null;
        }

        var trivia = new Trivia();

        while (Current.IsTrivia && !IsEnd)
        {
            trivia.Add(Consume());
        }

        return trivia;
    }

    public string ReadName()
    {
        string name = Consume().Text;

        ReadTrivia();

        return name;
    }

    public CssSequence ReadValueSpan()
    {
        var list = new CssSequence();

        while (!IsEnd
            && !(Current.Kind is CssTokenKind.Colon
                              or CssTokenKind.BlockStart
                              or CssTokenKind.BlockEnd
                              or CssTokenKind.Semicolon
                              or CssTokenKind.Comma))
        {
            if (Current.Kind is CssTokenKind.InterpolatedStringStart)
            {
                list.Add(ReadInterpolatedString());
            }
            else if (Current.Kind is CssTokenKind.Ampersand)
            {
                var ambersand = Consume();

                list.Add(new CssReference(ambersand) { Trailing = ReadTrivia() });
            }
            else
            {
                var text = Consume(); 

                list.Add(new CssString(text, trailing: ReadTrivia()));
            }
        }

        // throw new Exception(string.Join(Environment.NewLine, list));
            
        return list;
    }

    public TokenList ReadTokenSpan()
    {
        var list = new TokenList();

        while (!IsEnd 
            && !(Current.Kind is CssTokenKind.Colon 
                              or CssTokenKind.BlockStart
                              or CssTokenKind.BlockEnd
                              or CssTokenKind.Semicolon
                              or CssTokenKind.Comma))
        {
            list.Add(Consume());
        }

        return list;
    }

    public void Dispose()
    {
        _tokenizer.Dispose();
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