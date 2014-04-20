namespace Carbon.Css
{
	using Carbon.Css.Parser;

	public class CssIdentifier : CssValue
	{
		private readonly string text;

		public CssIdentifier(CssToken token)
			: this(token.Text)
		{ }

		public CssIdentifier(string text)
			: base(NodeKind.Identifier) 
		{ 
		
			this.text = text;
		}

		public override string Text
		{
			get { return text; }
		}
	}
}