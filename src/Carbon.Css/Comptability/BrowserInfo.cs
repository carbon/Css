using System;

namespace Carbon.Css;

public readonly struct BrowserInfo
{
    public BrowserInfo(BrowserType type, float version)
    {
        Type = type;
        Version = version;
    }

    public BrowserType Type { get; }

    public float Version { get; }

    public readonly BrowserPrefix Prefix => GetPrefix(Type);

    public static BrowserInfo Chrome(float version)  => new (BrowserType.Chrome, version);
    public static BrowserInfo Edge(float version)    => new(BrowserType.Edge, version);

    public static BrowserInfo Firefox(float version) => new (BrowserType.Firefox, version);
    public static BrowserInfo Safari(float version)  => new (BrowserType.Safari, version);

    public static readonly BrowserInfo Chrome1   = Chrome(1);
    public static readonly BrowserInfo Chrome4   = Chrome(4);
    public static readonly BrowserInfo Chrome7   = Chrome(7);
    public static readonly BrowserInfo Chrome10  = Chrome(10);
    public static readonly BrowserInfo Chrome13  = Chrome(13);
    public static readonly BrowserInfo Chrome26  = Chrome(26);
    public static readonly BrowserInfo Chrome36  = Chrome(36);
    public static readonly BrowserInfo Chrome50  = Chrome(50);

    public static readonly BrowserInfo Firefox1  = Firefox(1);
    public static readonly BrowserInfo Firefox4  = Firefox(4);
    public static readonly BrowserInfo Firefox5  = Firefox(5);
    public static readonly BrowserInfo Firefox6  = Firefox(6);
    public static readonly BrowserInfo Firefox9  = Firefox(9);
    public static readonly BrowserInfo Firefox10 = Firefox(10);
    public static readonly BrowserInfo Firefox16 = Firefox(16);
    public static readonly BrowserInfo Firefox20 = Firefox(20);
    public static readonly BrowserInfo Firefox21 = Firefox(21);
    public static readonly BrowserInfo Firefox29 = Firefox(29);

    public static readonly BrowserInfo Safari1   = Safari(1);
    public static readonly BrowserInfo Safari3   = Safari(3);
    public static readonly BrowserInfo Safari4   = Safari(4);
    public static readonly BrowserInfo Safari5   = Safari(5);
    public static readonly BrowserInfo Safari6   = Safari(6);
    public static readonly BrowserInfo Safari7   = Safari(7);
    public static readonly BrowserInfo Safari10  = Safari(10);
    public static readonly BrowserInfo Safari13  = Safari(13);
    public static readonly BrowserInfo Safari15  = Safari(15);

    public static BrowserPrefix GetPrefix(BrowserType type) => type switch
    {
        BrowserType.Chrome  => BrowserPrefix.Webkit,
        BrowserType.Firefox => BrowserPrefix.Moz,
        BrowserType.Edge    => BrowserPrefix.Webkit, // Edge is based on Chromium as of v88
        BrowserType.Safari  => BrowserPrefix.Webkit,
        _                   => throw new Exception($"Unexpected browser. Was {type}")
    };
        
    public override string ToString() => $"{Type}/{Version}";
}