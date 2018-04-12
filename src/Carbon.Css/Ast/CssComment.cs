namespace Carbon.Css
{
	public sealed class CssComment : CssNode
	{
		public CssComment(string text)
			: base(NodeKind.Comment)
        {
            Text = text;
		}

		public string Text { get; }
    }
}