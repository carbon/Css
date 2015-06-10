using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Carbon.Css
{
	using Parser;

	public class TokenList : Collection<CssToken>
	{
		public TokenList() { }

		public TokenList(CssToken[] tokens)
			: base(tokens) { }

		private void AddRange(IEnumerable<CssToken> tokens)
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
