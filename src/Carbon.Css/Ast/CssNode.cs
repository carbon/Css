namespace Carbon.Css
{
	using System.IO;

	public abstract class CssNode
	{
		private readonly NodeKind kind;
		private readonly CssNode parent;

		public CssNode(NodeKind kind, CssNode parent = null)
		{
			this.kind = kind;
			this.parent = parent;
		}

		public NodeKind Kind
		{
			get { return kind; }
		}

		public Whitespace Leading { get; set; }

		public Whitespace Trailing { get; set; }
	}

	// SyntaxNode

	// Leading & Trailing Trivia
}
