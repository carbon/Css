using Carbon.Css.Parser;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Carbon.Css
{
	public class Whitespace : Collection<Token>
	{
		public override string ToString()
		{
			return string.Join("", this.Select(t => t.ToString()));
		}
	}
}
