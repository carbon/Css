namespace Carbon.Css
{

	using Carbon.Css.Parser;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Text;

	public class TokenList : Collection<CssToken>
	{
		public TokenList() { }

		public TokenList(params CssToken[] tokens)
			: base(tokens) { }

		public void AddRange(IEnumerable<CssToken> tokens)
		{
			if (tokens == null) return;
			
			this.AddRange(tokens);
		}

		public string RawText
		{
			get 
			{
				var sb = new StringBuilder();

				foreach (var token in this)
				{
					sb.Append(token.Text);
				}

				return sb.ToString();
			}
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			
			foreach (var token in this)
			{
				if (token.IsTrivia) continue;

				if (sb.Length != 0) sb.Append(" ");

				sb.Append(token.Text);
			}

			return sb.ToString();
		}
	}
}
