namespace Carbon.Css
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;

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

		public bool Childless
		{
			get { return children.Count == 0; }
		}

		public override IList<CssNode> Children
		{
			get { return children; }
		}

		public IEnumerable<CssDeclaration> FindDeclaration(string propertyName)
		{
			return children.OfType<CssDeclaration>().Where(d => d.Name == propertyName);
		}

		public CssDeclaration Get(string name)
		{
			return children.OfType<CssDeclaration>().FirstOrDefault(d => d.Name == name);
		}

		public int Count
		{
			get { return children.Count; }
		}

		public override string Text
		{
			get { return ""; }
		}

		public override CssNode CloneNode()
		{
			throw new NotImplementedException();
		}

		#region IList<CssNode> Members

		public int IndexOf(CssNode node)
		{
			return children.IndexOf(node);
		}

		public void Insert(int index, CssNode item)
		{
			item.Parent = this;

			children.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			children.RemoveAt(index);
		}

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

		public bool Remove(CssNode item)
		{
			return children.Remove(item);
		}

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

// A block starts with a left curly brace ({) and ends with the matching right curly brace (}).
// In between there may be any tokens, except that parentheses (( )), brackets ([ ]), and braces ({ }) must always occur in matching pairs and may be nested.
// Single (') and double quotes (") must also occur in matching pairs, and characters between them are parsed as a string.
// See Tokenization above for the definition of a string.