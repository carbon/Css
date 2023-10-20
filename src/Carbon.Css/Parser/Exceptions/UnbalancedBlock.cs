namespace Carbon.Css.Parser;

public sealed class UnbalancedBlock(CssToken startToken) 
    : SyntaxException("The block is unclosed, '}' expected", startToken.Position)
{
}