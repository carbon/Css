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

        public readonly float Chrome;

        public readonly float Firefox;

        public readonly float IE;

        public readonly float Safari;

        public bool IsDefined => Chrome > 0 || Firefox > 0 || IE > 0 || Safari > 0;
    }
}
