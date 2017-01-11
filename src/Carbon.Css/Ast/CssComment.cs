namespace Carbon.Css
{
	public class CssComment : CssNode
	{
		public CssComment(string text)
			: base(NodeKind.Comment)
        {
            Text = text;
		}

		public string Text { get; }
    }
}