namespace Carbon.Css

{	public class CssDirective : CssNode
	{
		public CssDirective()
			: base(NodeKind.Directive) { }

		public string Name { get; set; }

		public string Value { get; set; }

		public override string Text
		{
			get { return ""; }
		}
	}
}



/*
EXAMPLES:
//= support IE 11+
//= support Safari 5.1+
//= support Chrome 20+
*/
