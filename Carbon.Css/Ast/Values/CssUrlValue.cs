namespace Carbon.Css
{
	using System;

	public class CssUrlValue // : CssPrimitiveValue
	{
		// url('')

		private readonly string value;

		public CssUrlValue(string value)
		{
			this.value = value;
		}

		public CssUrlValue(byte[] data, string contentType)
		{
			// Works for resources only up to 32k in size in IE8.

			this.value = "data:" + contentType + ";base64," + Convert.ToBase64String(data);
		}

		public string Value
		{
			get { return value; }
		}


		#region Helpers

		public bool IsPath
		{
			get { return !value.Contains(":"); }
		}

		public string GetAbsolutePath(string basePath) /* /styles/ */
		{
			if (!IsPath) throw new ArgumentException("Has scheme:" + value.Split(':')[0]);

			// Already absolute
			if (value.StartsWith("/")) return value;

			// "http://dev/styles/"
			var baseUri = new Uri("http://dev/" + basePath.TrimStart('/'));

			// Absolute path
			return new Uri(baseUri, relativeUri: value).AbsolutePath;
		}

		#endregion

		public static CssUrlValue Parse(string text)
		{
			var value = text.Replace("url", "").Trim('(', ')').Trim('\'', '\"');

			return new CssUrlValue(value);
		}

		public override string ToString()
		{
			// "url(" + value + ")";

			return string.Format("url('{0}')", value);
		}
	}
}
