namespace Carbon.Css
{
    public readonly struct CssParameter
    {
        public CssParameter(string name, CssValue? defaultValue = null)
        {
            Name = name;
            DefaultValue = defaultValue;
        }

        public string Name { get; }

        public CssValue? DefaultValue { get; }
    }
}