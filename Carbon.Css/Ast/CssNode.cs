namespace Carbon.Css
{
	using System.Collections.Generic;
	using System.IO;

	public abstract class CssNode
	{
		private readonly NodeKind kind;
		private readonly CssNode parent;

		private readonly IList<CssNode> children = new List<CssNode>();

		public CssNode(NodeKind kind, CssNode parent = null)
		{
			this.kind = kind;
			this.parent = parent;
		}

		public NodeKind Kind
		{
			get { return kind; }
		}

		public abstract string Text { get; }


		internal IList<CssNode> Children
		{
			get { return children; }
		}

		internal Whitespace Leading { get; set; }

		internal Whitespace Trailing { get; set; }
	}

	// SyntaxNode

	// Leading & Trailing Trivia
}
