namespace Carbon.Css.Parser
{
    public sealed class UnexpectedTokenException : SyntaxException
    {
        private readonly CssToken token;

        public UnexpectedTokenException(LexicalMode mode, CssToken token)
            : base($"Unexpected token reading {mode}. Was '{token.Kind}'.")
        {
            this.token = token;
        }

        public UnexpectedTokenException(LexicalMode mode, TokenKind expectedKind, CssToken token)
            : base($"Unexpected token at {token.Position} reading {mode}. Expected '{expectedKind}'. Was '{token.Kind}'.", token.Position)
        {
            this.token = token;
        }

        public UnexpectedTokenException(CssToken token)
            : base($"Unexpected token. Was '{token.Kind}:{token.Text}'.", token.Position)
        {
            this.token = token;
        }

        public CssToken Token => token;
    }
}
