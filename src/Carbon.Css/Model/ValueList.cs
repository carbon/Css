using System.Collections;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Carbon.Css;

public sealed class CssSequence : CssValue, IEnumerable<CssValue>
{
    private readonly List<CssValue> _children;

    public CssSequence()
        : base(NodeKind.Sequence)
    {
        _children = new List<CssValue>();
    }

    public CssSequence(int capacity)
        : base(NodeKind.Sequence)
    {
        _children = new List<CssValue>(capacity);
    }

    public CssSequence(params CssValue[] items)
        : base(NodeKind.Sequence)
    {
        _children = new List<CssValue>(items);
    }

    public void Add(CssValue item)
    {
        _children.Add(item);
    }

    public void AddRange(IEnumerable<CssValue> items)
    {
        _children.AddRange(items);
    }

    public int Count => _children.Count;

    public CssValue this[int index] => _children[index];

    [SkipLocalsInit]
    public override string ToString()
    {
        var sb = new ValueStringBuilder(stackalloc char[32]);

        WriteTo(ref sb);

        return sb.ToString();
    }

    internal override void WriteTo(scoped ref ValueStringBuilder sb)
    {
        for (int i = 0; i < _children.Count; i++)
        {
            CssValue item = _children[i];

            item.WriteTo(ref sb);

            // Skip trailing trivia
            if ((item.Trailing != null || item.Kind is NodeKind.Sequence) && (i + 1) != _children.Count)
            {
                sb.Append(' ');
            }
        }
    }

    internal override void WriteTo(TextWriter writer)
    {
        for (int i = 0; i < _children.Count; i++)
        {
            CssValue item = _children[i];

            item.WriteTo(writer);

            // Skip trailing trivia
            if ((item.Trailing != null || item.Kind is NodeKind.Sequence) && (i + 1) != _children.Count)
            {
                writer.Write(' ');
            }
        }
    }

    public bool Contains(NodeKind kind)
    {
        foreach (var token in _children)
        {
            if (token.Kind == kind) return true;
        }

        return false;
    }

    public IEnumerator<CssValue> GetEnumerator() => _children.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _children.GetEnumerator();
}
