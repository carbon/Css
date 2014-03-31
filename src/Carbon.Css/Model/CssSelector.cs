namespace Carbon.Css
{
	using Carbon.Css.Parser;
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Text;

	public class CssSelector : ICssSelector
	{
		private readonly string text;

		public CssSelector(TokenList tokens)
		{
			var sb = new StringBuilder();

			Token last = new Token(TokenKind.String, "", 0);

			foreach (var token in tokens)
			{
				if (token.IsTrivia) continue;

				if (sb.Length > 0 && last.Kind != TokenKind.Colon)
				{
					sb.Append(" ");
				}

				sb.Append(token.Text);

				last = token;
			}

			this.text = sb.ToString();

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

	public class CssSelectorList : Collection<CssSelector>, ICssSelector
	{
		public string Text
		{
			get { return ToString(); }
		}

		public override string ToString()
		{
			return string.Join(", ", this.Select(item => item.Text));
		}
	}


	public interface ICssSelector
	{
		string Text { get; }
	}

	// #id
	// .className
	// .className, .anotherName
}