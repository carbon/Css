namespace Carbon.Css.Parser
{
    public sealed class UnexpectedModeChange : SyntaxException
    {
        // "Current mode is:" + current + ". Leaving " + mode + "."

        private readonly int position;

        public UnexpectedModeChange(LexicalMode currentMode, LexicalMode leavingMode, int position)
            : base($"Unexpected mode change. Expected '{leavingMode}'. Was {currentMode}.")
        {
            this.position = position;
        }
    }
}
