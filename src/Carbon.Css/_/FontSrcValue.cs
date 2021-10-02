namespace Carbon.Css;

internal readonly struct FontSrcValue
{
    public FontSrcValue(string url, string format)
    {
        Url = url;
        Format = format;
    }

    public readonly string Url { get; }

    public readonly string Format { get; }

    public readonly override string ToString() => $"url('{Url}') format('{Format}')";
}

/*
    src: url('../fonts/cm-billing-webfont.eot?#iefix') format('embedded-opentype'),
         url('../fonts/cm-billing-webfont.woff') format('woff');
*/
