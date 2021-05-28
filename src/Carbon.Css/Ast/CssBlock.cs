using System;
using System.Collections;
using System.Collections.Generic;

namespace Carbon.Css
{
    public class CssBlock : CssNode, IEnumerable<CssNode>
    {
        protected readonly List<CssNode> _children;

        public CssBlock(NodeKind kind)
            : base(kind)
        {
            _children = new List<CssNode>();
           
        }

        public CssBlock(NodeKind kind, List<CssNode> children)
            : base(kind)
        {
            _children = children;
        }

        public List<CssNode> Children => _children;

        public CssBlockFlags Flags { get; set; }

        internal bool IsSimple => Flags == default;

        internal bool IsComplex => Flags != default;

        public bool HasChildren => _children.Count > 0;

        public CssDeclaration? GetDeclaration(string name)
        {
            foreach (var child in _children)
            {
                if (child is CssDeclaration declaration && declaration.Name.Equals(name, StringComparison.Ordinal))
                {
                    return declaration;
                }
            }

            return null;
        }
        
        #region List<CssNode> Members

        public int IndexOf(CssNode node) => _children.IndexOf(node);

        public void Insert(int index, CssNode item)
        {
            item.Parent = this;

            _children.Insert(index, item);
        }

        public void RemoveAt(int index) => _children.RemoveAt(index);

        public CssNode this[int index]
        {
            get => _children[index];
            set => _children[index] = value;
        }

        public void Add(CssNode node)
        {
            node.Parent = this;

            _children.Add(node);
        }

        public void AddRange(List<CssNode> nodes)
        {
            foreach (var node in nodes)
            {
                node.Parent = this;
            }

            _children.AddRange(nodes);
        }

        public bool Remove(CssNode item) => _children.Remove(item);

        IEnumerator<CssNode> IEnumerable<CssNode>.GetEnumerator() => _children.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _children.GetEnumerator();

        #endregion
    }


}

// A block starts with a left curly brace ({) and ends with the matching right curly brace (}).
// In between there may be any tokens, except that parentheses (( )), brackets ([ ]), and braces ({ }) must always occur in matching pairs and may be nested.
// Single (') and double quotes (") must also occur in matching pairs, and characters between them are parsed as a string.
// See Tokenization above for the definition of a string.