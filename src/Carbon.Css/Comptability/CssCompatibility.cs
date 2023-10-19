namespace Carbon.Css;

public class CssCompatibility(
    CompatibilityTable prefixed = default,
    CompatibilityTable standard = default,
    bool patchValues = false)
{
    public static readonly CssCompatibility All     = new();
    public static readonly CssCompatibility Default = new();

    public CompatibilityTable Prefixed { get; } = prefixed;

    public CompatibilityTable Standard { get; } = standard;

    public bool PatchValues { get; } = patchValues;

    public virtual CssPatch GetPatch(CssDeclaration declaration, in BrowserInfo browser)
    {
        if (PatchValues)
        {
            return PatchFactory.PrefixNameAndValue.Patch(browser, declaration);
        }

        return PatchFactory.PrefixName.Patch(browser, declaration);
    }

    public virtual bool HasPatch(CssDeclaration declaration, in BrowserInfo browser) => IsPrefixed(browser);

    public bool IsPrefixed(in BrowserInfo browser) => browser.Type switch
    {
        BrowserType.Chrome  => Prefixed.Chrome > 0f  && !IsStandard(browser),
        BrowserType.Firefox => Prefixed.Firefox > 0f && !IsStandard(browser),
        BrowserType.Edge    => Prefixed.Edge > 0f    && !IsStandard(browser),
        BrowserType.Safari  => Prefixed.Safari > 0f  && !IsStandard(browser),
        _                   => false
    };
        
    public bool IsStandard(in BrowserInfo browser) => browser.Type switch
    {
        BrowserType.Chrome  => Standard.Safari != 0 && Standard.Chrome <= browser.Version,
        BrowserType.Edge    => Standard.Edge != 0 && Standard.Edge <= browser.Version,
        BrowserType.Firefox => Standard.Firefox != 0 && Standard.Firefox <= browser.Version,
        BrowserType.Safari  => Standard.Safari != 0 && Standard.Safari <= browser.Version,
        _                   => false
    };
        
    public virtual bool HasPatches => Prefixed.IsDefined;
}