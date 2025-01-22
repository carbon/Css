using System.Diagnostics.CodeAnalysis;

namespace Carbon.Css.Selectors;

public sealed class TagSelector : Selector
{
    [SetsRequiredMembers]
    public TagSelector(string value)
        : base(CssSelectorType.Tag) 
    {
        Text = value;
    }
}
