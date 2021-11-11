using System;

namespace Carbon.Css;

public sealed class CharsetRule : CssRule
{
    public CharsetRule(ReadOnlyMemory<char> text)
    {
        Encoding = text;
    }

    public override RuleType Type => RuleType.Charset;

    public ReadOnlyMemory<char> Encoding { get; }
}

// @charset "UTF-8";

// https://www.iana.org/assignments/character-sets/character-sets.xhtml