namespace Carbon.Css
{
    public sealed class CursorCompatibility : CssCompatibility
    {
        public CursorCompatibility()
            : base() { }

        public override bool HasPatch(CssDeclaration declaration, in BrowserInfo browser)
        {
            return CssCursor.NeedsPatch(declaration.Value.ToString(), browser);
        }

        public override CssPatch GetPatch(CssDeclaration declaration, in BrowserInfo browser)
        {
            return new CssPatch(declaration.Name, new CssString(browser.Prefix + declaration.Value.ToString()));
        }

        public override bool HasPatches => true;
    }
}
