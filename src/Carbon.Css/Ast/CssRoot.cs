using System.Collections.Generic;

namespace Carbon.Css;

public class CssRoot : CssNode
{
    protected readonly List<CssNode> _children;

    public CssRoot()
        : this(new List<CssNode>())
    { }

    public CssRoot(List<CssNode> children)
        : base(NodeKind.Document)
    {
        _children = children;
    }

    #region Children

    public void RemoveChild(CssNode node)
    {
        node.Parent = null;

        _children.Remove(node);
    }

    public void AddChild(CssNode node)
    {
        node.Parent = this;

        _children.Add(node);
    }

    public void InsertChild(int index, CssNode node)
    {
        node.Parent = this;

        _children.Insert(index, node);
    }

    public List<CssNode> Children => _children;

    #endregion
}