namespace Carbon.Css.Parser.Tests;

public class TokenizerTests
{
    [Fact]
    public void TokenizeAnd()
    {
        var tokens = GetTokens(
            """
            &:nth-child(odd) {
            background-color: red;
            };
            """);

        Assert.Equal((CssTokenKind.Ampersand,        "&"),           tokens[0].AsTuple());
        Assert.Equal((CssTokenKind.Name,             ":nth-child"),  tokens[1].AsTuple());
        Assert.Equal((CssTokenKind.LeftParenthesis,  "("),           tokens[2].AsTuple());
        Assert.Equal((CssTokenKind.Name,             "odd"),         tokens[3].AsTuple());
        Assert.Equal((CssTokenKind.RightParenthesis, ")"),           tokens[4].AsTuple());
    }

    [Fact]
    public void BackenizeBackToBackPseudoClasses()
    {
        var tokens = GetTokens(
            """
            &:last-child:after {
            background-color: rgba($bg-color, $shade);
            };
            """);

        Assert.Equal((CssTokenKind.Ampersand, "&"),           tokens[0].AsTuple());
        Assert.Equal((CssTokenKind.Name,      ":last-child"), tokens[1].AsTuple());
        Assert.Equal((CssTokenKind.Name,      ":after"),      tokens[2].AsTuple());
    }

    [Fact]
    public void TokenizeSimpleMixin()
    {
        var tokens = GetTokens(
            """
            @mixin hi { 
            color: red;
            }
            """);

        Assert.Equal((CssTokenKind.AtSymbol, "@"),   tokens[0].AsTuple());
        Assert.Equal((CssTokenKind.Name, "mixin"),   tokens[1].AsTuple());
        Assert.Equal((CssTokenKind.Whitespace, " "), tokens[2].AsTuple());
        Assert.Equal((CssTokenKind.Name, "hi"),      tokens[3].AsTuple());
        Assert.Equal((CssTokenKind.Whitespace, " "), tokens[4].AsTuple());
        Assert.Equal((CssTokenKind.BlockStart, "{"), tokens[5].AsTuple());
        Assert.Equal((CssTokenKind.Whitespace),      tokens[6].Kind);
        Assert.Equal((CssTokenKind.Name, "color"),   tokens[7].AsTuple());
        Assert.Equal((CssTokenKind.Colon, ":"),      tokens[8].AsTuple());
        Assert.Equal((CssTokenKind.Whitespace, " "), tokens[9].AsTuple());
        Assert.Equal((CssTokenKind.String, "red"),   tokens[10].AsTuple());
        Assert.Equal((CssTokenKind.Semicolon, ";"),  tokens[11].AsTuple());
        Assert.Equal((CssTokenKind.Whitespace),      tokens[12].Kind);
        Assert.Equal((CssTokenKind.BlockEnd, "}"),   tokens[13].AsTuple());
    }

    [Fact]
    public void TokenizeComplexMixin()
    {
        var tokens = GetTokens(
            """
            @mixin blerg { 
            a {
            	color: pink;

            	&:hover { color: #000; }
            }
            }
            """);

        Assert.Equal((CssTokenKind.AtSymbol, "@"), tokens[0].AsTuple());
        Assert.Equal((CssTokenKind.Name, "mixin"), tokens[1].AsTuple());
        Assert.Equal((CssTokenKind.Whitespace, " "), tokens[2].AsTuple());
        Assert.Equal((CssTokenKind.Name, "blerg"), tokens[3].AsTuple());
        Assert.Equal((CssTokenKind.Whitespace, " "), tokens[4].AsTuple());
        Assert.Equal((CssTokenKind.BlockStart, "{"), tokens[5].AsTuple());
        Assert.Equal((CssTokenKind.Whitespace), tokens[6].Kind);
        Assert.Equal((CssTokenKind.Name, "a"), tokens[7].AsTuple());
        Assert.Equal(CssTokenKind.Whitespace, tokens[8].Kind);
        Assert.Equal((CssTokenKind.BlockStart, "{"), tokens[9].AsTuple());
        Assert.Equal(CssTokenKind.Whitespace, tokens[10].Kind);
        Assert.Equal((CssTokenKind.Name, "color"), tokens[11].AsTuple());
        Assert.Equal((CssTokenKind.Colon, ":"), tokens[12].AsTuple());
        Assert.Equal((CssTokenKind.Whitespace, " "), tokens[13].AsTuple());
        Assert.Equal((CssTokenKind.String, "pink"), tokens[14].AsTuple());
        Assert.Equal((CssTokenKind.Semicolon, ";"), tokens[15].AsTuple());
        Assert.Equal(CssTokenKind.Whitespace, tokens[16].Kind);
        Assert.Equal((CssTokenKind.Ampersand, "&"), tokens[17].AsTuple());
        Assert.Equal((CssTokenKind.Name, ":hover"), tokens[18].AsTuple());
        Assert.Equal((CssTokenKind.Whitespace, " "), tokens[19].AsTuple());
        Assert.Equal((CssTokenKind.BlockStart, "{"), tokens[20].AsTuple());
        Assert.Equal(CssTokenKind.Whitespace, tokens[21].Kind);

        Assert.Equal((CssTokenKind.Name, "color"), tokens[22].AsTuple());
        Assert.Equal((CssTokenKind.Colon, ":"), tokens[23].AsTuple());
        Assert.Equal((CssTokenKind.Whitespace, " "), tokens[24].AsTuple());
        Assert.Equal((CssTokenKind.String, "#000"), tokens[25].AsTuple());
        Assert.Equal((CssTokenKind.Semicolon, ";"), tokens[26].AsTuple());
        Assert.Equal((CssTokenKind.Whitespace, " "), tokens[27].AsTuple());
        Assert.Equal((CssTokenKind.BlockEnd, "}"), tokens[28].AsTuple());
        Assert.Equal(CssTokenKind.Whitespace, tokens[29].Kind);
        Assert.Equal((CssTokenKind.BlockEnd, "}"), tokens[30].AsTuple());
    }

    [Fact]
    public void TokenizeMediaRule()
    {
        var tokens = GetTokens(
            """
            @media (min-width: 700px) { 
            @include blerg;

            div { 
            	background-color: $bgColor;
            	@include hi;
            }
            }
            """);

        Assert.Equal((CssTokenKind.AtSymbol, "@"),         tokens[0].AsTuple());
        Assert.Equal((CssTokenKind.Name, "media"),         tokens[1].AsTuple());
        Assert.Equal((CssTokenKind.Whitespace, " "),       tokens[2].AsTuple());
        Assert.Equal((CssTokenKind.LeftParenthesis, "("),  tokens[3].AsTuple());
        Assert.Equal((CssTokenKind.Name, "min-width"),     tokens[4].AsTuple());
        Assert.Equal((CssTokenKind.Colon, ":"),            tokens[5].AsTuple());
        Assert.Equal((CssTokenKind.Whitespace, " "),       tokens[6].AsTuple());
        Assert.Equal((CssTokenKind.Number, "700"),         tokens[7].AsTuple());
        Assert.Equal((CssTokenKind.Unit, "px"),            tokens[8].AsTuple());
        Assert.Equal((CssTokenKind.RightParenthesis, ")"), tokens[9].AsTuple());
        Assert.Equal((CssTokenKind.Whitespace, " "),       tokens[10].AsTuple());
        Assert.Equal((CssTokenKind.BlockStart, "{"),       tokens[11].AsTuple());
        Assert.Equal((CssTokenKind.Whitespace),            tokens[12].Kind);

        Assert.Equal((CssTokenKind.AtSymbol, "@"),         tokens[13].AsTuple());
        Assert.Equal((CssTokenKind.Name, "include"),       tokens[14].AsTuple());
        Assert.Equal((CssTokenKind.Whitespace, " "),       tokens[15].AsTuple());
        Assert.Equal((CssTokenKind.Name, "blerg"),         tokens[16].AsTuple());

        Assert.Equal((CssTokenKind.Semicolon, ";"),        tokens[17].AsTuple());
    }
       
    private static List<CssToken> GetTokens(string text)
    {
        using var tokenizer = new CssTokenizer(text);

        var tokens = new List<CssToken>();

        do
        {
            tokens.Add(tokenizer.Consume());
        }
        while (!tokenizer.IsEnd);

        return tokens;
    }

    [Fact]
    public void CalcTest4()
    {
        var tokens = GetTokens("calc(100% / 3 - 2 * 1em - 2 * 1px)");
            
        Assert.Equal(27, tokens.Count);

        Assert.Equal((CssTokenKind.LeftParenthesis, "("),   tokens[1].AsTuple());
        Assert.Equal((CssTokenKind.Number,          "100"), tokens[2].AsTuple());
        Assert.Equal((CssTokenKind.Unit,            "%"),   tokens[3].AsTuple());
        Assert.Equal((CssTokenKind.Whitespace,      " "),   tokens[4].AsTuple());
        Assert.Equal((CssTokenKind.Divide,          "/"),   tokens[5].AsTuple());
        Assert.Equal((CssTokenKind.Whitespace,      " "),   tokens[6].AsTuple());

        /*
		Assert.Equal(tokens[6].Kind, TokenKind.Number);
		Assert.Equal(tokens[7].Kind, TokenKind.Whitespace);
		Assert.Equal(tokens[8].Kind, TokenKind.Subtract);
		Assert.Equal(tokens[9].Kind, TokenKind.Add);
		*/

        var styles = "main { margin: 0.5in; width: calc(100% / 3 - 2 * 1em - 2 * 1px); }";

        Assert.Equal(
            """
            main {
              margin: 0.5in;
              width: calc(100% / 3 - 2 * 1em - 2 * 1px);
            }
            """, StyleSheet.Parse(styles).ToString());
    }
}