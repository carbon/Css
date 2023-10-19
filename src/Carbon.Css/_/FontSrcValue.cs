namespace Carbon.Css;

internal readonly struct FontSrcValue(string url, string format)
{
    public readonly string Url { get; } = url;

    public readonly string Format { get; } = format;

    public readonly override string ToString() => $"url('{Url}') format('{Format}')";
}

/*
    src: url('../fonts/cm-billing-webfont.eot?#iefix') format('embedded-opentype'),
         url('../fonts/cm-billing-webfont.woff') format('woff');
*/
