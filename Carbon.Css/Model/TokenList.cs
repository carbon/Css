namespace Carbon.Css
{

	using Carbon.Css.Parser;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;

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
			get { return string.Join("", this.Select(t => t.Text)); }
		}

		public override string ToString()
		{
			return string.Join(" ", this.Where(t => !t.IsTrivia).Select(t => t.Text));
		}
	}
}
