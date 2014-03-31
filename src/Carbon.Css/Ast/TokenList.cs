using Carbon.Css.Parser;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Carbon.Css
{
	public class TokenList : Collection<Token>
	{
		public void AddRange(IEnumerable<Token> tokens)
		{
			if (tokens == null) return;

			foreach (var token in tokens)
			{
				this.Add(token);
			}
		}

		public override string ToString()
		{
			return string.Join(" ", this.Where(t => !t.IsTrivia).Select(t => t.Text));
		}

		public IEnumerable<CssPrimitiveValue> ToValues()
		{
			var names = new List<CssName>();

			foreach (var token in this)
			{
				if (token.IsTrivia) continue;

				if (token.Kind == TokenKind.Comma)
				{
					yield return CssPrimitiveValue.Parse(string.Join(" ", names.Select(n => n.Text)));
				}
				else
				{
					names.Add(new CssName(token.Text));
				}
			}

			yield return CssPrimitiveValue.Parse(string.Join(" ", names.Select(n => n.Text)));
		}

		public IEnumerable<CssSelector> ToSelectors()
		{
			var names = new TokenList();

			foreach (var token in this)
			{
				if (token.IsTrivia) continue;

				if (token.Kind == TokenKind.Comma)
				{
					yield return new CssSelector(names);

					names.Clear();
				}
				else
				{
					names.Add(token);
				}
			}

			yield return new CssSelector(names);
		}
	}
}
