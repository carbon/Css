namespace Carbon.Css
{
	using System.IO;

	public class MediaRule : CssRule
	{
		private readonly string ruleText;

		public MediaRule(string ruleText)
			: base(RuleType.Media) {

			this.ruleText = ruleText;
		}

		public string RuleText
		{
			get { return ruleText; }
		}

	}
}