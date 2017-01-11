using System.Collections.Generic;

namespace Carbon.Css
{
    public class CssParameter
    {
        public static readonly List<CssParameter> EmptyList = new List<CssParameter>();

        public CssParameter(string name, CssValue defaultValue = null)
        {
            Name = name;
            DefaultValue = defaultValue;
        }

        public string Name { get; }

        public CssValue DefaultValue { get; }
    }
}