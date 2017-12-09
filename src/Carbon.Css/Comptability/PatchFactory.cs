namespace Carbon.Css
{
    internal static class PatchFactory
    {
        public static readonly CssPatcher PrefixName         = new PrefixNamePatcher();
        public static readonly CssPatcher PrefixNameAndValue = new PrefixNameAndValuePatcher();

        // transform 0.04s linear, opacity 0.04s linear, visibility 0.04s linear;
        // -webkit-transform 0.04s linear, opacity 0.04s linear, visibility 0.04s linear;

        public static CssValue PatchValue(CssValue value, in BrowserInfo browser)
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

    public readonly struct CssPatch
    {
        public CssPatch(string name, CssValue value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }

        public CssValue Value { get; }
    }

    public abstract class CssPatcher
    {
        public abstract CssPatch Patch(BrowserInfo browser, CssDeclaration declaration);
    }

    public sealed class PrefixNamePatcher : CssPatcher
    {
        public override CssPatch Patch(BrowserInfo browser, CssDeclaration declaration) => new CssPatch(
            name    : browser.Prefix + declaration.Name, 
            value   : declaration.Value
        );
    }

    public sealed class PrefixNameAndValuePatcher : CssPatcher
    {
        public override CssPatch Patch(BrowserInfo browser, CssDeclaration declaration) => new CssPatch(
            name  : browser.Prefix + declaration.Name, 
            value : PatchFactory.PatchValue(declaration.Value, browser)
        );
    }
}
