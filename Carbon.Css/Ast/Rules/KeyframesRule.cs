namespace Carbon.Css
{
	using System.IO;

	public class KeyframesRule : CssRule
	{
		private readonly string name;

		public KeyframesRule(string name)
			: base(RuleType.Keyframes) { 
		
			this.name = name;
		}

		public string Name
		{
			get { return name; }
		}

		// TODO: Keyframes

		public override CssNode CloneNode()
		{
			var rule = new KeyframesRule(this.name);

			foreach (var x in children)
			{
				rule.Add(x.CloneNode());
			}

			return rule;
		}
	}
}