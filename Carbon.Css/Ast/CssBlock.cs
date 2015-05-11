using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Carbon.Css
{
	public class CssBlock : CssNode, IEnumerable<CssNode>
	{
		protected readonly List<CssNode> children;

		public CssBlock(NodeKind kind)
			: base(kind) 
		{
			this.children = new List<CssNode>();
		}

		public CssBlock(NodeKind kind, List<CssNode> children)
			: base(kind)
		{
			this.children = children;
		}

		public override IList<CssNode> Children => children;

		public int Count => children.Count;

		public bool Childless => Count == 0;

		public IEnumerable<CssDeclaration> FindDeclaration(string propertyName)
		{
			return children.OfType<CssDeclaration>().Where(d => d.Name == propertyName);
		}

		public CssDeclaration Get(string name)
		{
			return children.OfType<CssDeclaration>().FirstOrDefault(d => d.Name == name);
		}

		#region IList<CssNode> Members

		public int IndexOf(CssNode node) => children.IndexOf(node);

		public void Insert(int index, CssNode item)
		{
			item.Parent = this;

			children.Insert(index, item);
		}

		public void RemoveAt(int index) => children.RemoveAt(index);

		public CssNode this[int index]
		{
			get { return children[index]; }
			set { children[index] = value; }
		}

		public void Add(CssNode node)
		{
			node.Parent = this;

			children.Add(node);
		}

		public bool Remove(CssNode item) => children.Remove(item);

		IEnumerator<CssNode> IEnumerable<CssNode>.GetEnumerator() => children.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => children.GetEnumerator();

		#endregion
	}
}

// A block starts with a left curly brace ({) and ends with the matching right curly brace (}).
// In between there may be any tokens, except that parentheses (( )), brackets ([ ]), and braces ({ }) must always occur in matching pairs and may be nested.
// Single (') and double quotes (") must also occur in matching pairs, and characters between them are parsed as a string.
// See Tokenization above for the definition of a string.