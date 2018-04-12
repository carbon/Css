using System.Collections.Generic;

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

            var list = new List<CssValue>();

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

            return new CssValueList(list, a.Seperator);
        }
    }
}
