namespace Carbon.Css
{
	using System.Collections;
	using System.Collections.Generic;

	public abstract class CssNode : IEnumerable<CssNode>
	{
		private readonly NodeKind kind;
		private readonly CssNode parent;

		protected readonly List<CssNode> children = new List<CssNode>();

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


		public List<CssNode> Children
		{
			get { return children; }
		}

		internal Whitespace Leading { get; set; }

		internal Whitespace Trailing { get; set; }

		#region IEnumerator

		IEnumerator<CssNode> IEnumerable<CssNode>.GetEnumerator()
		{
			return children.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return children.GetEnumerator();
		}

		#endregion
	}
}
