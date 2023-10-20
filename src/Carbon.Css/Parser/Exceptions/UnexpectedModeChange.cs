namespace Carbon.Css.Parser;

public sealed class UnexpectedModeChange(LexicalMode currentMode, LexicalMode leavingMode, int position)
    : SyntaxException($"Unexpected mode change. Expected '{leavingMode}'. Was {currentMode}.", position)
{
}