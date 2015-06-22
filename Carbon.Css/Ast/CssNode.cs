using System;

namespace Carbon.Css
{
	public abstract class CssNode 
	{
		private readonly NodeKind kind;
		private CssNode parent;

		public CssNode(NodeKind kind, CssNode parent = null)
		{
			this.kind = kind;
			this.parent = parent;
		}

		public NodeKind Kind => kind;

		public CssNode Parent
		{
			get { return parent; }
			set { parent = value; }
		}

		internal Trivia Leading { get; set; }

		internal Trivia Trailing { get; set; }

		// ChildNodes

		public virtual CssNode CloneNode()
		{
			throw new NotImplementedException();
		}
	}
}
