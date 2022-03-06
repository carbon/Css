namespace Carbon.Css.Parser;

public sealed class UnexpectedTokenException : SyntaxException
{
    public UnexpectedTokenException(LexicalMode mode, CssToken token)
        : base($"Unexpected token reading {mode}. Was '{token.Kind}'.")
    {
        Token = token;
    }

    public UnexpectedTokenException(LexicalMode mode, TokenKind expectedKind, CssToken token)
        : base($"Unexpected token at {token.Position} reading {mode}. Expected '{expectedKind}'. Was '{token.Kind}'", token.Position)
    {
        Token = token;
    }

    public UnexpectedTokenException(CssToken token)
        : base($"Unexpected token. Was '{token.Kind}:{token.Text}'.", token.Position)
    {
        Token = token;
    }

    public CssToken Token { get; }
}