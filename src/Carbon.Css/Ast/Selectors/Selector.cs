using System.Text;

using Carbon.Css.Parser;

namespace Carbon.Css.Selectors;

public class Selector : CssNode
{
    public Selector(CssSelectorType type)
        : base(NodeKind.Selector)
    {
        Type = type;
    }

    public CssSelectorType Type { get; }

    public required string Text { get; set; }

    public CssValue? Arguments { get; set; }

    public CombinatorType Combinator { get; set; }

    public Selector? Next { get; set; }

    public bool HasNestingParent
    {
        get
        {
            if (Type is CssSelectorType.NestingParent) return true;

            Selector? test = this;

            while ((test = test.Next) != null)
            {
                if (test.Type is CssSelectorType.NestingParent)
                {
                    return true;
                }
            }

            return false;
        }
    }

    // IsSimple
    // IsCompound

    public override string ToString()
    {
        var sb = new ValueStringBuilder(128);

        WriteTo(ref sb);

        return sb.ToString();
    }

    internal void WriteTo(ref ValueStringBuilder sb)
    {
        sb.Append(Text);

        if (Arguments != null)
        {
            sb.Append('(');
            Arguments.WriteTo(ref sb);
            sb.Append(')');
        }

        if (Next != null)
        {
            sb.Append(Combinator switch {
                CombinatorType.SubsequentSibling => " ~ ",
                CombinatorType.AdjacentSibling   => " + ",
                CombinatorType.Descendant        => " ",
                CombinatorType.Child             => " > ",
                _                                => ""
            });

            Next.WriteTo(ref sb);
        }
    }

    public static CssSelector Parse(string text)
    {
        using var parser = new CssParser(text);

        return parser.ReadSelector();
    }
}