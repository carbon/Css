namespace Carbon.Css
{
	using System.IO;
	using Carbon.Css.Parser;

	public class CssName : CssNode
	{
		private string text;

		public CssName(string text)
			: base(NodeKind.Name)
		{
			this.text = text;
		}

		public string Text
		{
			get { return text; }
		}

		public override string ToString()
		{
			return text;
		}
	}
}
