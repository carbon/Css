using System;

namespace Carbon.Css
{
    public readonly struct BrowserInfo
    {
        public BrowserInfo(BrowserType type, float version)
        {
            Type = type;
            Version = version;
        }

        public BrowserType Type { get; }

        public float Version { get; }

        public BrowserPrefix Prefix => GetPrefix(Type);

        public static BrowserInfo Chrome(float version)  => new BrowserInfo(BrowserType.Chrome, version);
        public static BrowserInfo Firefox(float version) => new BrowserInfo(BrowserType.Firefox, version);
        public static BrowserInfo Safari(float version)  => new BrowserInfo(BrowserType.Safari, version);
        public static BrowserInfo Opera(float version)   => new BrowserInfo(BrowserType.Opera, version);
        public static BrowserInfo IE(float version)      => new BrowserInfo(BrowserType.IE, version);

        public static readonly BrowserInfo Chrome1  = Chrome(1);
        public static readonly BrowserInfo Chrome4  = Chrome(4);
        public static readonly BrowserInfo Chrome7  = Chrome(7);
        public static readonly BrowserInfo Chrome10 = Chrome(10);
        public static readonly BrowserInfo Chrome13 = Chrome(13);
        public static readonly BrowserInfo Chrome26 = Chrome(26);
        public static readonly BrowserInfo Chrome36 = Chrome(36);
        public static readonly BrowserInfo Chrome50 = Chrome(50);

        public static readonly BrowserInfo Firefox1  = Firefox(1);
        public static readonly BrowserInfo Firefox4  = Firefox(4);
        public static readonly BrowserInfo Firefox5  = Firefox(5);
        public static readonly BrowserInfo Firefox6  = Firefox(6);
        public static readonly BrowserInfo Firefox9  = Firefox(9);
        public static readonly BrowserInfo Firefox10 = Firefox(10);
        public static readonly BrowserInfo Firefox16 = Firefox(16);
        public static readonly BrowserInfo Firefox20 = Firefox(20);
        public static readonly BrowserInfo Firefox21 = Firefox(21);
        public static readonly BrowserInfo Firefox29 = Firefox(29);

        public static readonly BrowserInfo IE6       = IE(6);
        public static readonly BrowserInfo IE7       = IE(7);
        public static readonly BrowserInfo IE8       = IE(8);
        public static readonly BrowserInfo IE9       = IE(9);
        public static readonly BrowserInfo IE10      = IE(10);
        public static readonly BrowserInfo IE11      = IE(11);
        public static readonly BrowserInfo IE12      = IE(12); // edge

        public static readonly BrowserInfo Opera4  = Opera(3);
        public static readonly BrowserInfo Opera9  = Opera(9);
        public static readonly BrowserInfo Opera15 = Opera(15); // Based on Chromium

        public static readonly BrowserInfo Safari1 = Safari(1);
        public static readonly BrowserInfo Safari3 = Safari(3);
        public static readonly BrowserInfo Safari4 = Safari(4);
        public static readonly BrowserInfo Safari5 = Safari(5);
        public static readonly BrowserInfo Safari6 = Safari(6);
        public static readonly BrowserInfo Safari7 = Safari(7);

        public static BrowserPrefix GetPrefix(BrowserType type)
        {
            switch (type)
            {
                case BrowserType.Chrome  : return BrowserPrefix.Webkit;
                case BrowserType.Firefox : return BrowserPrefix.Moz;
                case BrowserType.IE      : return BrowserPrefix.MS;
                case BrowserType.Opera   : return BrowserPrefix.Opera;
                case BrowserType.Safari  : return BrowserPrefix.Webkit;

                default: throw new Exception("Unexpected browser: " + type);
            }
        }

        public override string ToString()
        {
            return Type + "/" + Version;
        }
    }

    public readonly struct BrowserPrefix : IEquatable<BrowserPrefix>
    {
        public static readonly BrowserPrefix Moz    = new BrowserPrefix(BrowserPrefixKind.Moz,    "-moz-");
        public static readonly BrowserPrefix MS     = new BrowserPrefix(BrowserPrefixKind.Ms,     "-ms-");
        public static readonly BrowserPrefix Opera  = new BrowserPrefix(BrowserPrefixKind.O,      "-o-");
        public static readonly BrowserPrefix Webkit = new BrowserPrefix(BrowserPrefixKind.Webkit, "-webkit-");

        private BrowserPrefix(BrowserPrefixKind kind, string text)
        {
            Kind = kind;
            Text = text;
        }

        public BrowserPrefixKind Kind { get; }

        public string Text { get; }
       
        public static implicit operator string(BrowserPrefix prefix) => prefix.Text;

        #region Equality

        public bool Equals(BrowserPrefix other) => Kind == other.Kind;

        public override int GetHashCode() => (int)Kind;

        #endregion
    }

    [Flags]
    public enum BrowserPrefixKind
    {
        None = 0,
        Moz = 1,
        Ms = 2,
        O = 4,
        Webkit = 8
    }

    [Flags]
    public enum BrowserType
    {
        Unknown = 0,
        IE = 1,
        Firefox = 2,
        Safari = 4,
        Chrome = 8,
        Opera = 16
    }
}
