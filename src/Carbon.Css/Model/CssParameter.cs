namespace Carbon.Css
{
    public readonly struct CssParameter
    {
        public CssParameter(string name, CssValue? defaultValue = null)
        {
            Name = name;
            DefaultValue = defaultValue;
        }

        public readonly string Name { get; }

        public readonly CssValue? DefaultValue { get; }
    }
}