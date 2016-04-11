namespace Carbon.Css
{
    public struct CompatibilityTable
    {
        public CompatibilityTable(float chrome = 0, float firefox = 0, float ie = 0, float safari = 0)
        {
            Chrome = chrome;
            Firefox = firefox;
            IE = ie;
            Safari = safari;
        }

        public float Chrome { get; }

        public float Firefox { get; }

        public float IE { get; }

        public float Safari { get; }

        public bool IsDefined => Chrome > 0 || Firefox > 0 || IE > 0 || Safari > 0;
    }


    public struct Patch
    {
        public Patch(string name, CssValue value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }

        public CssValue Value { get; }
    }

    public interface ICssPatcher
    {
        Patch Patch(Browser browser, CssDeclaration declaration);
    }

    public class PrefixName : ICssPatcher
    {
        public Patch Patch(Browser browser, CssDeclaration declaration)
            => new Patch(browser.Prefix + declaration.Name, declaration.Value);
    }

    public class PrefixNameAndValuePatcher : ICssPatcher
    {
        public Patch Patch(Browser browser, CssDeclaration declaration)
            => new Patch(browser.Prefix + declaration.Name, CssPatcher.PatchValue(declaration.Value, browser));
    }

 
    public static class CssPatcher
    {

        public static readonly ICssPatcher PrefixName = new PrefixName();
        public static readonly ICssPatcher PrefixNameAndValue = new PrefixNameAndValuePatcher();

        // transform 0.04s linear, opacity 0.04s linear, visibility 0.04s linear;
        // -webkit-transform 0.04s linear, opacity 0.04s linear, visibility 0.04s linear;

        public static CssValue PatchValue(CssValue value, Browser browser)
        {
            if (value.Kind != NodeKind.ValueList) return value;

            var a = (CssValueList)value;

            var list = new CssValueList(a.Seperator);

            foreach (var node in a)
            {
                if (node.Kind == NodeKind.ValueList) // For comma seperated componented lists
                {
                    list.Add(PatchValue(node, browser));
                }
                else if (node.Kind == NodeKind.String && node.ToString() == "transform")
                {
                    list.Add(new CssString(browser.Prefix.Text + "transform"));
                }
                else
                {
                    list.Add(node);
                }
            }

            return list;
        }

    }

    public class CursorCompatibility : CssCompatibility
    {
        public static new readonly CssCompatibility All = new CssCompatibility();

        public CursorCompatibility()
            : base() { }

        public override bool HasPatch(CssDeclaration declaration, Browser browser)
        {
            return CssCursor.NeedsPatch(declaration.Value.ToString(), browser);
          
        }

        public override Patch GetPatch(Browser browser, CssDeclaration declaration)
        {
            return new Patch(declaration.Name, new CssString(browser.Prefix + declaration.Value.ToString()));

        }

        public override bool HasPatches => true;
    }

    public class CssCompatibility
    {
        public static readonly CssCompatibility All = new CssCompatibility();
        public static readonly CssCompatibility Unknown = new CssCompatibility();

        public CssCompatibility(CompatibilityTable prefixed = new CompatibilityTable(), CompatibilityTable standard = new CompatibilityTable())
        {
            Prefixed = prefixed;
            Standard = standard;
        }

        public bool PatchValues { get; set; }

        public CompatibilityTable Prefixed { get; }

        public CompatibilityTable Standard { get; }

        public virtual Patch GetPatch(Browser browser, CssDeclaration declaration)
        {

            if (PatchValues)
            {
                return CssPatcher.PrefixNameAndValue.Patch(browser, declaration);

            }

            return CssPatcher.PrefixNameAndValue.Patch(browser, declaration);
        }

        public virtual bool HasPatch(CssDeclaration declaration, Browser browser)
            => IsPrefixed(browser);

        public bool IsPrefixed(Browser browser)
        {
            switch (browser.Type)
            {
                case BrowserType.Chrome     : return Prefixed.Chrome > 0f && !IsStandard(browser);
                case BrowserType.Firefox    : return Prefixed.Firefox > 0f && !IsStandard(browser);
                case BrowserType.IE         : return Prefixed.IE > 0f && !IsStandard(browser);
                case BrowserType.Safari     : return Prefixed.Safari > 0f && !IsStandard(browser);
            }

            return false;
        }

        public bool IsStandard(Browser browser)
        {
            switch (browser.Type)
            {
                case BrowserType.Chrome     : return Standard.Safari != 0 && Standard.Chrome <= browser.Version;
                case BrowserType.Firefox    : return Standard.Firefox != 0 && Standard.Firefox <= browser.Version;
                case BrowserType.IE         : return Standard.IE != 0 && Standard.IE <= browser.Version;
                case BrowserType.Safari     : return Standard.Safari != 0 && Standard.Safari <= browser.Version;
            }

            return false;
        }


        public virtual bool HasPatches => Prefixed.IsDefined;

        // Rewrite
    }
}
