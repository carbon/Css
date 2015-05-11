namespace Carbon.Css
{
	public class FontSrcValue
	{
		private readonly string url;
		private readonly string format;

		public FontSrcValue(string url, string format)
		{
			this.url = url;
			this.format = format;
		}

		public string Url => url;

		public string Format => format;

		public override string ToString()
		{
			return $"url('{url}') format('{format}')";
		}
	}
}

/*
    src: url('../fonts/cm-billing-webfont.eot?#iefix') format('embedded-opentype'),
         url('../fonts/cm-billing-webfont.woff') format('woff');
*/
