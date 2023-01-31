namespace Carbon.Css;

public abstract class CssPatcher
{
    public abstract CssPatch Patch(BrowserInfo browser, CssDeclaration declaration);
}
