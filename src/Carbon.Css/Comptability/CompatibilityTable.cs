namespace Carbon.Css
{
    public readonly struct CompatibilityTable
    {
        public CompatibilityTable(float chrome = 0, float firefox = 0, float ie = 0, float safari = 0)
        {
            Chrome = chrome;
            Firefox = firefox;
            IE = ie;
            Safari = safari;
        }

        public readonly float Chrome { get; }

        public readonly float Firefox { get; }

        public readonly float IE { get; }

        public readonly float Safari { get; }

        public readonly bool IsDefined => Chrome > 0 || Firefox > 0 || IE > 0 || Safari > 0;
    }
}
