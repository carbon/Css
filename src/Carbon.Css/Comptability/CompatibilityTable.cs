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

        public float Chrome { get; }

        public float Firefox { get; }

        public float IE { get; }

        public float Safari { get; }

        public bool IsDefined => Chrome > 0 || Firefox > 0 || IE > 0 || Safari > 0;
    }
}
