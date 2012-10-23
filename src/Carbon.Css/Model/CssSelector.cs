namespace Carbon.Css
{
	using System;

	public class CssSelector
	{
		private readonly string text;

		public CssSelector(string text)
		{
			#region Preconditions

			if (text == null) throw new ArgumentNullException("text");

			#endregion

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