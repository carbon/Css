namespace Carbon.Css
{
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
}