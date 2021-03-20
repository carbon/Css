using System;

namespace Carbon.Css
{
    public sealed class BrowserPrefix : IEquatable<BrowserPrefix>
    {
        public static readonly BrowserPrefix Moz    = new (BrowserPrefixKind.Moz,    "-moz-");
        public static readonly BrowserPrefix MS     = new (BrowserPrefixKind.Ms,     "-ms-");
        public static readonly BrowserPrefix Opera  = new (BrowserPrefixKind.O,      "-o-");
        public static readonly BrowserPrefix Webkit = new (BrowserPrefixKind.Webkit, "-webkit-");

        private BrowserPrefix(BrowserPrefixKind kind, string text)
        {
            Kind = kind;
            Text = text;
        }

        public BrowserPrefixKind Kind { get; }

        public string Text { get; }
       
        public static implicit operator string(BrowserPrefix prefix) => prefix.Text;

        public bool Equals(BrowserPrefix? other) => other is not null && Kind == other.Kind;

        public override bool Equals(object? obj)
        {
            return obj is BrowserPrefix other && Equals(other);
        }

        public override int GetHashCode() => (int)Kind;

        public static bool operator ==(BrowserPrefix left, BrowserPrefix right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BrowserPrefix left, BrowserPrefix right)
        {
            return !left.Equals(right);
        }
    }
}
