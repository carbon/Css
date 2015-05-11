namespace Carbon.Css
{
	public sealed class KeyframesRule : CssRule
	{
		private readonly string name;

		public KeyframesRule(string name)
			: base(RuleType.Keyframes) { 
		
			this.name = name;
		}

		public string Name => name;

		// TODO: Keyframes

		public override CssNode CloneNode()
		{
			var rule = new KeyframesRule(name);

			foreach (var x in children)
			{
				rule.Add(x.CloneNode());
			}

			return rule;
		}
	}
}