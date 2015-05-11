namespace Carbon.Css

{	public class CssDirective : CssNode
	{
		private readonly string name;
		private readonly string value;

		public CssDirective(string name, string value)
			: base(NodeKind.Directive) {

			this.name = name;
			this.value = value;
		}

		public string Name => name;

		public string Value => value;
	}
}



/*
EXAMPLES:
//= support IE 11+
//= support Safari 5.1+
//= support Chrome 20+
*/
