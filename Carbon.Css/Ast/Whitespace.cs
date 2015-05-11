using System.Collections.ObjectModel;
using System.Text;

namespace Carbon.Css
{
	using Parser;

	public class Whitespace : Collection<CssToken>
	{
		public override string ToString()
		{
			var sb = new StringBuilder();

			foreach (var token in this)
			{
				sb.Append(token.ToString());
			}

			return sb.ToString();
		}
	}
}