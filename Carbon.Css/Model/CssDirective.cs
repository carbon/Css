namespace Carbon.Css
{
	//= supports IE > 10
	public class CssDirective : CssRule
	{
		public CssDirective()
			: base(RuleType.Directive, NodeKind.Directive) { }

		public string Name { get; set; }

		public string Value { get; set; }
	}
}
