using System;
using System.Collections;
using System.Collections.Generic;

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

        public List<CssNode> Children => children;

        public CssBlockFlags Flags { get; set; }

        internal bool IsSimple => Flags == default;

        internal bool IsComplex => Flags != default;

        public bool HasChildren => children.Count > 0;

        public CssDeclaration? GetDeclaration(string name)
        {
            foreach (var child in children)
            {
                if (child is CssDeclaration declaration && declaration.Name.Equals(name, StringComparison.Ordinal))
                {
                    return declaration;
                }
            }

            return null;
        }
        
        #region List<CssNode> Members

        public int IndexOf(CssNode node) => children.IndexOf(node);

        public void Insert(int index, CssNode item)
        {
            item.Parent = this;

            children.Insert(index, item);
        }

        public void RemoveAt(int index) => children.RemoveAt(index);

        public CssNode this[int index]
        {
            get => children[index];
            set => children[index] = value;
        }

        public void Add(CssNode node)
        {
            node.Parent = this;

            children.Add(node);
        }

        public void AddRange(List<CssNode> nodes)
        {
            foreach (var node in nodes)
            {
                node.Parent = this;
            }

            children.AddRange(nodes);
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