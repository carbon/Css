namespace Carbon.Css.Parser;

public sealed class UnbalancedBlock : SyntaxException
{
    public UnbalancedBlock(CssToken startToken)
        : base("The block is unclosed, '}' expected", startToken.Position)
    { }
}