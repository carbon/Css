namespace Carbon.Css.Parser.Tests
{
    public static class CssTokenExtensions
    {
        public static (TokenKind kind, string text) AsTuple(this CssToken token)
        {
            return (token.Kind, token.Text);
        }
    }
}
