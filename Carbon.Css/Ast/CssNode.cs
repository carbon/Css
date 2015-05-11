using System;
using System.Collections;
using System.Collections.Generic;

namespace Carbon.Css
{
	public abstract class CssNode : IEnumerable<CssNode>
	{
		private static readonly CssNode[] EmptyNodeArray = new CssNode[0];

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

		internal Whitespace Leading { get; set; }

		internal Whitespace Trailing { get; set; }

		// ChildNodes

		public virtual IList<CssNode> Children => EmptyNodeArray;

		public bool HasChildren => Children?.Count > 0;

		public virtual CssNode CloneNode()
		{
			throw new NotImplementedException();
		}

		#region IEnumerator

		IEnumerator<CssNode> IEnumerable<CssNode>.GetEnumerator()
		{
			return Children.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Children.GetEnumerator();
		}

		#endregion
	}
}
