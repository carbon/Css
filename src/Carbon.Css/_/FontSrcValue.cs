namespace Carbon.Css
{
    internal readonly struct FontSrcValue
    {
        public FontSrcValue(string url, string format)
        {
            Url = url;
            Format = format;
        }

        public string Url { get; }

        public string Format { get; }

        public override string ToString() => $"url('{Url}') format('{Format}')";
    }
}

/*
    src: url('../fonts/cm-billing-webfont.eot?#iefix') format('embedded-opentype'),
         url('../fonts/cm-billing-webfont.woff') format('woff');
*/
