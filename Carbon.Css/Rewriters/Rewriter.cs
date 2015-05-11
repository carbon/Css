namespace Carbon.Css
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;

	public class RewriterCollection : Collection<ICssRewriter>
	{
		public IEnumerable<CssRule> Rewrite(CssRule rule, int index = 0)
		{
			if (this.Count == 0)
			{
				yield return rule;

				yield break;
			}

			// TODO: Pass along in order

			// Chain


			foreach (var r in this[index].Rewrite(rule))
			{
				if (this.Count > index + 1)
				{
					foreach(var r2 in Rewrite(r, ++index))
					{
						yield return r2;
					}
				}
				else
				{
					yield return r;
				}
			}
			
		}
	}
}