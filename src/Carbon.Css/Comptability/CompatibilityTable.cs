namespace Carbon.Css;

public readonly struct CompatibilityTable
{
    public CompatibilityTable(
        float chrome = 0,
        float edge = 0, 
        float firefox = 0,
        float safari = 0)
    {
        Chrome = chrome;
        Edge = edge;
        Firefox = firefox;
        Safari = safari;
    }

    public readonly float Chrome { get; }

    public readonly float Edge { get; }

    public readonly float Firefox { get; }

    public readonly float Safari { get; }

    public readonly bool IsDefined => Chrome > 0 || Firefox > 0 || Edge > 0 || Safari > 0;
}
