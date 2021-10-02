namespace Carbon.Css.Parser;

public sealed class UnexpectedModeChange : SyntaxException
{
    // "Current mode is:" + current + ". Leaving " + mode + "."

    public UnexpectedModeChange(LexicalMode currentMode, LexicalMode leavingMode, int position)
        : base($"Unexpected mode change. Expected '{leavingMode}'. Was {currentMode}.", position) { }
}