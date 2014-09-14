namespace Carbon.Css
{
	using System;
	using System.Text;

	using Carbon.Css.Parser;

	public class CssSelector
	{
		private readonly string text;

		public CssSelector(TokenList tokens)
		{
			var sb = new StringBuilder();

			CssToken? last = null;

			foreach (var token in tokens)
			{
				if (token.IsTrivia)
				{
					sb.Append(" ");
				}
				else
				{
					sb.Append(token.Text);
				}

				last = token;
			}

			this.text = sb.ToString().Trim();
		}

		public CssSelector(string text)
		{
			#region Preconditions

			if (text == null) throw new ArgumentNullException("text");

			#endregion

			this.text = text;
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

	// #id
	// .className
	// .className, .anotherName
}