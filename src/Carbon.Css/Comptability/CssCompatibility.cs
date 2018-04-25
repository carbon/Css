namespace Carbon.Css
{
    public sealed class CursorCompatibility : CssCompatibility
    {
        public CursorCompatibility()
            : base() { }

        public override bool HasPatch(CssDeclaration declaration, in BrowserInfo browser)
            => CssCursor.NeedsPatch(declaration.Value.ToString(), browser);

        public override CssPatch GetPatch(CssDeclaration declaration, in BrowserInfo browser)
            => new CssPatch(declaration.Name, new CssString(browser.Prefix + declaration.Value.ToString()));

        public override bool HasPatches => true;
    }

    public class CssCompatibility
    {
        public static readonly CssCompatibility All     = new CssCompatibility();
        public static readonly CssCompatibility Default = new CssCompatibility();

        public CssCompatibility(
            CompatibilityTable prefixed = default,
            CompatibilityTable standard = default)
        {
            Prefixed = prefixed;
            Standard = standard;
        }

        public bool PatchValues { get; set; }

        public CompatibilityTable Prefixed { get; }

        public CompatibilityTable Standard { get; }

        public virtual CssPatch GetPatch(CssDeclaration declaration, in BrowserInfo browser)
        {
            if (PatchValues)
            {
                return PatchFactory.PrefixNameAndValue.Patch(browser, declaration);
            }

            return PatchFactory.PrefixNameAndValue.Patch(browser, declaration);
        }

        public virtual bool HasPatch(CssDeclaration declaration, in BrowserInfo browser) => IsPrefixed(browser);

        public bool IsPrefixed(in BrowserInfo browser)
        {
            switch (browser.Type)
            {
                case BrowserType.Chrome  : return Prefixed.Chrome > 0f && !IsStandard(browser);
                case BrowserType.Firefox : return Prefixed.Firefox > 0f && !IsStandard(browser);
                case BrowserType.IE      : return Prefixed.IE > 0f && !IsStandard(browser);
                case BrowserType.Safari  : return Prefixed.Safari > 0f && !IsStandard(browser);
            }

            return false;
        }

        public bool IsStandard(in BrowserInfo browser)
        {
            switch (browser.Type)
            {
                case BrowserType.Chrome  : return Standard.Safari != 0 && Standard.Chrome <= browser.Version;
                case BrowserType.Firefox : return Standard.Firefox != 0 && Standard.Firefox <= browser.Version;
                case BrowserType.IE      : return Standard.IE != 0 && Standard.IE <= browser.Version;
                case BrowserType.Safari  : return Standard.Safari != 0 && Standard.Safari <= browser.Version;
            }

            return false;
        }
        
        public virtual bool HasPatches => Prefixed.IsDefined;

        // Rewrite
    }
}
