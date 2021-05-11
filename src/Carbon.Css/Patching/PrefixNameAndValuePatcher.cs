namespace Carbon.Css
{
    public sealed class PrefixNameAndValuePatcher : CssPatcher
    {
        public override CssPatch Patch(BrowserInfo browser, CssDeclaration declaration) => new (
            name  : browser.Prefix + declaration.Name, 
            value : PatchFactory.PatchValue(declaration.Value, browser)
        );
    }
}
