using System.Collections.Generic;

namespace Carbon.Css
{
	public class CssRoot : CssNode
	{
		protected readonly List<CssNode> children;

		public CssRoot()
			: this(new List<CssNode>()) { }

		public CssRoot(List<CssNode> children)
			: base(NodeKind.Document)
		{
			this.children = children;
		}

		#region Children

		public void RemoveChild(CssNode node)
		{
			node.Parent = null;

			children.Remove(node);
		}

		public void AddChild(CssNode node)
		{
			node.Parent = this;

			children.Add(node);
		}

		public void InsertChild(int index, CssNode node)
		{
			node.Parent = this;

			children.Insert(index, node);
		}

		public List<CssNode> Children => children;

		#endregion
	}
}
