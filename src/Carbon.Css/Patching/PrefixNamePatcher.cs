namespace Carbon.Css;

public sealed class PrefixNamePatcher : CssPatcher
{
    public override CssPatch Patch(BrowserInfo browser, CssDeclaration declaration) => new (
        name  : browser.Prefix + declaration.Name, 
        value : declaration.Value
    );
}