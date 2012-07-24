namespace Carbon.Css
{
	public class CssSelector
	{
		private readonly string text;

		public CssSelector(string text)
		{
			this.text = text.Replace('\t', ' ').Trim();
		}

		public string Text
		{
			get { return text; }
		}

		public override string ToString()
		{
			return text;
		}
	}
}