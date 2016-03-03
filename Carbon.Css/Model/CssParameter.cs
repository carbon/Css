namespace Carbon.Css
{
    public class CssParameter
    {
        public CssParameter(string name, CssValue defaultValue = null)
        {
            Name = name;
            DefaultValue = defaultValue;
        }

        public string Name { get; }

        public CssValue DefaultValue { get; }
    }
}