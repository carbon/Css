namespace Carbon.Css;

public sealed class CssProperty : IEquatable<CssProperty>
{
    private readonly CssCompatibility? _compatibility;

    public CssProperty(string name)
        : this(name, null, null)
    { }

    public CssProperty(string name, CssCompatibility compatibility)
        : this(name, null, compatibility)
    { }

    public CssProperty(string name, CssModule? module = null, CssCompatibility? compatibility = null)
    {
        Name = name;
        Module = module;

        _compatibility = compatibility;

        if (compatibility is null && module != null)
        {
            _compatibility = module;
        }
    }

    public bool IsStandard => Module is not null;

    public string Name { get; }

    public CssModule? Module { get; }

    public CssCompatibility Compatibility => _compatibility ?? CssCompatibility.Default;

    public bool NeedsExpansion(CssDeclaration declaration, ReadOnlySpan<BrowserInfo> browsers)
    {
        if (browsers.IsEmpty)
        {
            return false;
        }

        if (!Compatibility.HasPatches) return false;

        foreach(ref readonly BrowserInfo browser in browsers)
        {          
            if (Compatibility.HasPatch(declaration, browser)) return true;
        }

        return false;
    }

    public override int GetHashCode() => Name.GetHashCode();

    public override bool Equals(object? obj)
    {
        if (obj is string text)
        {
            return Name.Equals(text, StringComparison.Ordinal);
        }

        return obj is CssProperty other && Equals(other);
    }

    public bool Equals(CssProperty? other)
    {
        if (other is null) return false;

        if (ReferenceEquals(this, other)) return true;

        return Name.Equals(other.Name, StringComparison.Ordinal);
    }

    public override string ToString() => Name;

    public static CssProperty Get(string name)
    {
        if (!Map.TryGetValue(name, out CssProperty? propertyInfo))
        {
            propertyInfo = new CssProperty(name);
        }

        return propertyInfo;
    }

    // Animations (Level 3) -----------------------------------------------------------------------------------------------------------
    public static readonly CssProperty Animation                = new("animation",                 CssModule.Animations3);
    public static readonly CssProperty AnimationDelay           = new("animation-delay",           CssModule.Animations3);
    public static readonly CssProperty AnimationDirection       = new("animation-direction",       CssModule.Animations3);
    public static readonly CssProperty AnimationDuration        = new("animation-duration",        CssModule.Animations3);
    public static readonly CssProperty AnimationIterationCount  = new("animation-iteration-count", CssModule.Animations3);
    public static readonly CssProperty AnimationName            = new("animation-name",            CssModule.Animations3);
    public static readonly CssProperty AnimationPlayState       = new("animation-play-state",      CssModule.Animations3);
    public static readonly CssProperty AnimationTimingFunction  = new("animation-timing-function", CssModule.Animations3);

    public static readonly CssProperty Appearance = new("appearance", new CssCompatibility(
        prefixed: new (chrome: 1,            firefox: 1,  safari: 3),
        standard: new (chrome: 84, edge: 83, firefox: 80, safari: 15.4f)
    ));

    public static readonly CssProperty AspectRatio = new("aspect-ratio", new CssCompatibility(
        standard: new(chrome: 88, edge: 88, firefox: 89, safari: 15)
    ));

    public static readonly CssProperty Azimuth = new("azimuth", CssModule.Core2_1);

    public static readonly CssProperty BackdropFilter = new("backdrop-filter", CssModule.Transforms3, new CssCompatibility(
      prefixed: new(safari: 9),
      standard: new(chrome: 76, edge: 79, firefox: 103)
    ));

    public static readonly CssProperty BackfaceVisibility = new("backface-visibility", CssModule.Transforms3, new CssCompatibility(
        prefixed: new (chrome: 12, firefox: 10, safari: 5.1f),
        standard: new (chrome: 16, edge: 12, firefox: 36, safari: 15.4f)
    ));

    // Backgrounds
    public static readonly CssProperty Background           = new("background", CssModule.Core1);
    public static readonly CssProperty BackgroundAttachment = new("background-attachment", CssModule.Core1);

    public static readonly CssProperty BackgroundClip = new("background-clip", CssModule.BackgroundsAndBorders3, new CssCompatibility(
        prefixed: new(chrome : 4,            firefox: 4, safari: 4),
        standard: new(chrome : 15, edge: 15, firefox: 4, safari: 7)
    ));

    public static readonly CssProperty BackgroundColor = new("background-color", CssModule.Core1);
    public static readonly CssProperty BackgroundImage = new("background-image", CssModule.Core1);

    public static readonly CssProperty BackgroundOrigin = new("background-origin", CssModule.BackgroundsAndBorders3, new CssCompatibility(
        standard: new (chrome: 1, edge: 9, firefox: 4, safari: 3)
    ));

    public static readonly CssProperty BackgroundPosition = new("background-position",  CssModule.Core1);
    public static readonly CssProperty BackgroundRepeat   = new("background-repeat",    CssModule.Core1);
    public static readonly CssProperty BackgroundSize     = new("background-size",      CssModule.BackgroundsAndBorders3);

    // Borders -------------------------------------------------------------------------------------------------------
    public static readonly CssCompatibility BorderImageCompatibility = new (
        prefixed: new (chrome: 7,            firefox: 3.5f, safari: 3),
        standard: new (chrome: 16, edge: 11, firefox: 15,   safari: 6.1f)
    );

    public static readonly CssProperty Border                  = new("border",                 CssModule.Core1);
    public static readonly CssProperty BorderBottom            = new("border-bottom",          CssModule.Core1);
    public static readonly CssProperty BorderBottomColor       = new("border-bottom-color",    CssModule.Core1);
    public static readonly CssProperty BorderBottomLeftRadius  = new("border-bottom-left-radius");
    public static readonly CssProperty BorderBottomRightRadius = new("border-bottom-right-radius");
    public static readonly CssProperty BorderBottomStyle       = new("border-bottom-style");
    public static readonly CssProperty BorderBottomWidth       = new("border-bottom-width");
    public static readonly CssProperty BorderCollapse          = new("border-collapse");
    public static readonly CssProperty BorderColor             = new("border-color",           CssModule.Core1);
    public static readonly CssProperty BorderImage             = new("border-image",           BorderImageCompatibility);
    public static readonly CssProperty BorderImageOutset       = new("border-image-outset",    BorderImageCompatibility);
    public static readonly CssProperty BorderImageRepeat       = new("border-image-repeat",    BorderImageCompatibility);
    public static readonly CssProperty BorderImageSlice        = new("border-image-slice",     BorderImageCompatibility);
    public static readonly CssProperty BorderImageSource       = new("border-image-source",    BorderImageCompatibility);
    public static readonly CssProperty BorderImageWidth        = new("border-image-width",     BorderImageCompatibility);

    public static readonly CssProperty BorderLeft              = new("border-left", CssModule.Core1);
    public static readonly CssProperty BorderLeftColor         = new("border-left-color");
    public static readonly CssProperty BorderLeftStyle         = new("border-left-style");
    public static readonly CssProperty BorderLeftWidth         = new("border-left-width");
    public static readonly CssProperty BorderRadius            = new("border-radius", CssModule.BackgroundsAndBorders3);
    public static readonly CssProperty BorderRight             = new("border-right", CssModule.Core1);
    public static readonly CssProperty BorderRightColor        = new("border-right-color");
    public static readonly CssProperty BorderRightStyle        = new("border-right-style");
    public static readonly CssProperty BorderRightWidth        = new("border-right-width");
    public static readonly CssProperty BorderSpacing           = new("border-spacing");
    public static readonly CssProperty BorderStyle             = new("border-style", CssModule.Core1);
    public static readonly CssProperty BorderTop               = new("border-top", CssModule.Core1);
    public static readonly CssProperty BorderTopColor          = new("border-top-color");
    public static readonly CssProperty BorderTopLeftRadius     = new("border-top-left-radius");
    public static readonly CssProperty BorderTopRightRadius    = new("border-top-right-radius");
    public static readonly CssProperty BorderTopStyle          = new("border-top-style");
    public static readonly CssProperty BorderTopWidth          = new("border-top-width", CssModule.Core1);
    public static readonly CssProperty BorderWidth             = new("border-width", CssModule.Core1);

    public static readonly CssProperty Bottom                  = new("bottom", CssModule.Core1);

    public static readonly CssProperty BoxDecorationBreak      = new("box-decoration-break");

    public static readonly CssProperty BoxShadow = new("box-shadow", CssModule.UI(3), new CssCompatibility(
        prefixed: new (chrome: 1,           firefox: 3.5f, safari: 3.1f),
        standard: new (chrome: 10, edge: 9, firefox: 4,    safari: 5.1f)
    ));

    public static readonly CssProperty BoxSizing = new("box-sizing", CssModule.UI(3), new CssCompatibility(
        prefixed: new (chrome: 1,  edge: 8, firefox: 1,  safari: 3),
        standard: new (chrome: 10, edge: 9, firefox: 29, safari: 5.1f)
    ));

    // Breaks
    public static readonly CssProperty BreakAfter  = new("break-after");
    public static readonly CssProperty BreakBefore = new("break-before");
    public static readonly CssProperty BreakInside = new("break-inside");

    public static readonly CssProperty CaptionSide = new("caption-side");
    public static readonly CssProperty Clear       = new ("clear");

    // Clipping
    public static readonly CssProperty Clip = new("clip", CssModule.Masking_1);

    // TODO: Confirm support
    public static readonly CssProperty ClipPath = new("clip-path", new CssCompatibility(
        prefixed: new (chrome: 24f, safari: 7),
        standard: new (firefox: 54)
    ));

    public static readonly CssProperty ClipRule = new ("clip-rule", CssModule.Masking_1);

    public static readonly CssProperty Color = new("color", CssModule.Core1);

    public static readonly CssProperty ColorMix = new("color-mix", new CssCompatibility(
        standard: new(chrome: 111, edge: 111, firefox: 113, safari: 16.2f)
    ));

    public static readonly CssProperty ColumnCount      = new("column-count",       CssModule.Columns3);
    public static readonly CssProperty ColumnFill       = new("column-fill",        CssModule.Columns3);
    public static readonly CssProperty ColumnGap        = new("column-gap",         CssModule.Columns3);
    public static readonly CssProperty ColumnRule       = new("column-rule",        CssModule.Columns3);
    public static readonly CssProperty ColumnRuleColor  = new("column-rule-color",  CssModule.Columns3);
    public static readonly CssProperty ColumnRuleStyle  = new("column-rule-style",  CssModule.Columns3);
    public static readonly CssProperty ColumnRuleWidth  = new("column-rule-width",  CssModule.Columns3);
    public static readonly CssProperty ColumnSpan       = new("column-span",        CssModule.Columns3);
    public static readonly CssProperty ColumnWidth      = new("column-width",       CssModule.Columns3);
    public static readonly CssProperty Columns          = new("columns",            CssModule.Columns3);

    // Containment -
    public static readonly CssProperty Contain       = new("contain",        CssModule.Containment1);
    public static readonly CssProperty Container     = new("container",      CssModule.Containment1);
    public static readonly CssProperty ContainerName = new("container-name", CssModule.Containment1);
    public static readonly CssProperty ContainerType = new("container-type", CssModule.Containment1);

    public static readonly CssProperty Content       = new("content", CssModule.Core2_1);

    // Counters -
    public static readonly CssProperty CounterIncrement = new("counter-increment");
    public static readonly CssProperty CounterReset     = new("counter-reset");

    public static readonly CssProperty Cursor    = new ("cursor", new CursorCompatibility());
    public static readonly CssProperty Direction = new ("direction");
    public static readonly CssProperty Display   = new ("display", CssModule.Core1);

    public static readonly CssProperty EmptyCells = new ("empty-cells");

    public static readonly CssProperty Filter = new ("filter", new CssCompatibility(
        prefixed: new (chrome: 18, edge: 13, firefox: 3.6f, safari: 6),
        standard: new (chrome: 53, edge: 79, firefox: 35,   safari: 9.1f)
    ));

    // SVG (Fill)
    public static readonly CssProperty Fill        = new("fill");
    public static readonly CssProperty FillOpacity = new("fill-opacity");

    // Fit ---------------------------------------------------------------------------------------
    public static readonly CssProperty Fit         = new("fit");
    public static readonly CssProperty FitPosition = new("fit-position");

    // Flex ---------------------------------------------------------------------------------------
    // https://www.w3.org/TR/css-flexbox-1/

    public static readonly CssProperty Flex          = new("flex",           CssModule.Flexbox1);
    public static readonly CssProperty FlexBasis     = new("flex-basis",     CssModule.Flexbox1);
    public static readonly CssProperty FlexDirection = new("flex-direction", CssModule.Flexbox1);
    public static readonly CssProperty FlexFlow      = new("flex-flow",      CssModule.Flexbox1);
    public static readonly CssProperty FlexGrow      = new("flex-grow",      CssModule.Flexbox1);
    public static readonly CssProperty FlexShrink    = new("flex-shrink",    CssModule.Flexbox1);
    public static readonly CssProperty FlexWrap      = new("flex-wrap",      CssModule.Flexbox1);

    public static readonly CssProperty Float         = new("float", CssModule.Core1);

    // Fonts -------------------------------------------------------------------------------------------
    public static readonly CssProperty Font           = new("font",           CssModule.Core1);
    public static readonly CssProperty FontFamily     = new("font-family",    CssModule.Core1);
    public static readonly CssProperty FontSize       = new("font-size",      CssModule.Core1);
    public static readonly CssProperty FontSizeAdjust = new("font-size-adjust");
    public static readonly CssProperty FontStretch    = new("font-stretch",   CssModule.Fonts3);
    public static readonly CssProperty FontStyle      = new("font-style",     CssModule.Core1);
    public static readonly CssProperty FontVariant    = new("font-variant",   CssModule.Core1);
    public static readonly CssProperty FontWeight     = new("font-weight",    CssModule.Core1);

    // Grids ---------------------------------------------------------------------------------------

    public static readonly CssProperty Grid                = new("grid");
    public static readonly CssProperty GridAutoColumns     = new("grid-auto-columns");
    public static readonly CssProperty GridAutoFlow        = new("grid-auto-flow");
    public static readonly CssProperty GridAutoRows        = new("grid-auto-rows");
    public static readonly CssProperty GridTemplateAreas   = new("grid-template-areas");
    public static readonly CssProperty GridTemplateColumns = new("grid-template-columns");
    public static readonly CssProperty GridTemplateRows    = new("grid-template-rows");

    public static readonly CssProperty GridColumns         = new("grid-columns");
    public static readonly CssProperty GridRows            = new("grid-rows");

    // 
    public static readonly CssProperty Height              = new("height", CssModule.Core1);

    // Hyphens -------------------------------------------------------------------------------------
    public static readonly CssProperty HyphenateAfter     = new("hyphenate-after");
    public static readonly CssProperty HyphenateBefore    = new("hyphenate-before");
    public static readonly CssProperty HyphenateCharacter = new("hyphenate-character", CssModule.Text3);
    public static readonly CssProperty HyphenateLines     = new("hyphenate-lines");
    public static readonly CssProperty HyphenateResource  = new("hyphenate-resource");

    public static readonly CssProperty Hyphens = new("hyphens", new CssModule(CssModuleType.Text, 3), new CssCompatibility(
        prefixed: new (chrome: 15, edge: 10, firefox: 6, safari: 5.1f),
        standard: new (chrome: 88,           firefox: 43)
    ));

    // https://developer.mozilla.org/en-US/docs/Web/CSS/hyphens

    public static readonly CssProperty InlineBoxAlign = new ("inline-box-align");

    public static readonly CssProperty Left          = new ("left", CssModule.Core1);
    public static readonly CssProperty LetterSpacing = new ("letter-spacing");

    // Lines
    public static readonly CssProperty LineBreak         = new ("line-break");
    public static readonly CssProperty LineHeight        = new ("line-height", CssModule.Core1);

    // List Styles -------------------------------------------------------------------------------------------------------
    public static readonly CssProperty ListStyle         = new ("list-style",          CssModule.Core1);
    public static readonly CssProperty ListStyleImage    = new ("list-style-image",    CssModule.Core1);
    public static readonly CssProperty ListStylePosition = new ("list-style-position", CssModule.Core1);
    public static readonly CssProperty ListStyleType     = new ("list-style-type",     CssModule.Core1);

    // Margins -----------------------------------------------------------------------------------------------------------
    public static readonly CssProperty Margin            = new ("margin",        CssModule.Core1);
    public static readonly CssProperty MarginBottom      = new ("margin-bottom", CssModule.Core1);
    public static readonly CssProperty MarginLeft        = new ("margin-left",   CssModule.Core1);
    public static readonly CssProperty MarginRight       = new ("margin-right",  CssModule.Core1);
    public static readonly CssProperty MarginTop         = new ("margin-top",    CssModule.Core1);

    // Masking -----------------------------------------------------------------------------------------
    public static readonly CssProperty Mask             = new ("mask",               CssModule.Masking_1);
    public static readonly CssProperty MaskBorder       = new ("mask-border",        CssModule.Masking_1);
    public static readonly CssProperty MaskBorderMode   = new ("mask-border-mode",   CssModule.Masking_1);
    public static readonly CssProperty MaskBorderOutset = new ("mask-border-outset", CssModule.Masking_1);
    public static readonly CssProperty MaskBorderRepeat = new ("mask-border-repeat", CssModule.Masking_1);
    public static readonly CssProperty MaskBorderSlice  = new ("mask-border-slice",  CssModule.Masking_1);
    public static readonly CssProperty MaskBorderSource = new ("mask-border-source", CssModule.Masking_1);
    public static readonly CssProperty MaskBorderWidth  = new ("mask-border-width",  CssModule.Masking_1);
    public static readonly CssProperty MaskClip         = new ("mask-clip",          CssModule.Masking_1);
    public static readonly CssProperty MaskComposite    = new ("mask-composite",     CssModule.Masking_1);
    public static readonly CssProperty MaskImage        = new ("mask-image",         CssModule.Masking_1);
    public static readonly CssProperty MaskMode         = new ("mask-mode",          CssModule.Masking_1);
    public static readonly CssProperty MaskOrigin       = new ("mask-origin",        CssModule.Masking_1);
    public static readonly CssProperty MaskPosition     = new ("mask-position",      CssModule.Masking_1);
    public static readonly CssProperty MaskRepeat       = new ("mask-repeat",        CssModule.Masking_1);
    public static readonly CssProperty MaskSize         = new ("mask-size",          CssModule.Masking_1);

    public static readonly CssProperty MaxHeight        = new ("max-height", CssModule.Core2_1);
    public static readonly CssProperty MaxWidth         = new ("max-width",  CssModule.Core2_1);

    public static readonly CssProperty MinHeight        = new ("min-height", CssModule.Core2_1);
    public static readonly CssProperty MinWidth         = new ("min-width",  CssModule.Core2_1);

    // <= IE8 filter: alpha(opacity=xx)
    // IE8 introduced -ms-filter, which is synonymous with filter. Both are gone in IE10
    public static readonly CssProperty Opacity = new ("opacity", CssModule.Color3);

    public static readonly CssProperty Order   = new ("order");

    public static readonly CssProperty Orphans = new ("orphans", new CssModule(CssModuleType.Core, 2.1f), new CssCompatibility(
        standard: new (chrome: 25, edge: 8, safari: 3.1f)
    ));

    // Outlines -------------------------------------------------------------------------------
    public static readonly CssProperty Outline       = new("outline",        CssModule.Core2_1);
    public static readonly CssProperty OutlineColor  = new("outline-color",  CssModule.Core2_1);
    public static readonly CssProperty OutlineOffset = new("outline-offset", CssModule.Core2_1);
    public static readonly CssProperty OutlineStyle  = new("outline-style",  CssModule.Core2_1);
    public static readonly CssProperty OutlineWidth  = new("outline-width",  CssModule.Core2_1);

    // Overflow -------------------------------------------------------------------------------
    public static readonly CssProperty Overflow      = new("overflow", CssModule.Core2_1);
    public static readonly CssProperty OverflowStyle = new("overflow-style");
    public static readonly CssProperty OverflowWrap  = new("overflow-wrap");
    public static readonly CssProperty OverflowX     = new("overflow-x", CssModule.Core2_1);
    public static readonly CssProperty OverflowY     = new("overflow-y", CssModule.Core2_1);

    // Padding -----------------------------------------------------------------------------------------------
    public static readonly CssProperty Padding       = new("padding",        CssModule.Core1);
    public static readonly CssProperty PaddingBottom = new("padding-bottom", CssModule.Core1);
    public static readonly CssProperty PaddingLeft   = new("padding-left",   CssModule.Core1);
    public static readonly CssProperty PaddingRight  = new("padding-right",  CssModule.Core1);
    public static readonly CssProperty PaddingTop    = new("padding-top",    CssModule.Core1);

    public static readonly CssProperty Page = new ("page");

    // Page Breaks -------------------------------------------------------------------------------
    public static readonly CssProperty PageBreakAfter  = new ("page-break-after");
    public static readonly CssProperty PageBreakBefore = new ("page-break-before");
    public static readonly CssProperty PageBreakInside = new ("page-break-inside");

    // Perspective
    public static readonly CssProperty Perspective        = new ("perspective",          CssModule.Transforms3);
    public static readonly CssProperty PerspectiveOrigin  = new ("perspective-origin",   CssModule.Transforms3);
    public static readonly CssProperty PerspectiveOriginX = new ("perspective-origin-x", CssModule.Transforms3);
    public static readonly CssProperty PerspectiveOriginY = new ("perspective-origin-y", CssModule.Transforms3);

    public static readonly CssProperty Position = new ("position", CssModule.Core1);

      
    public static readonly CssProperty Quotes = new ("quotes");
    public static readonly CssProperty Resize = new ("resize", CssModule.UI(4), new CssCompatibility(
        standard: new(chrome: 1, edge: 79, firefox: 4, safari: 3)   
    ));

    public static readonly CssProperty Right  = new ("right", CssModule.Core1);

    public static readonly CssProperty Rotate = new("rotate", new CssCompatibility(
        standard: new(chrome: 104, edge: 104, firefox: 72, safari: 14.1f)   
    ));

    // Ruby (Level 3) ------------------------------------------------------------------------------------

    public static readonly CssProperty RubyAlign    = new("ruby-align",    CssModule.Ruby3);
    public static readonly CssProperty RubyOverhang = new("ruby-overhang", CssModule.Ruby3);
    public static readonly CssProperty RubyPosition = new("ruby-position", CssModule.Ruby3);
    public static readonly CssProperty RubySpan     = new("ruby-span",     CssModule.Ruby3);

    public static readonly CssProperty Size  = new("size");
    public static readonly CssProperty Speak = new("speak");

    public static readonly CssProperty TableLayout = new ("table-layout", CssModule.Core2_1);

    // Stroke (SVG) ------------------------------------------------------------------------
    public static readonly CssProperty Stroke               = new("stroke");
    public static readonly CssProperty StrokeDashArray      = new("stroke-dasharray");
    public static readonly CssProperty StrokeDashOffset     = new("stroke-dashoffset");
    public static readonly CssProperty StrokeLinecap        = new("stroke-linecap");
    public static readonly CssProperty StrokeLinejoin       = new("stroke-linejoin");
    public static readonly CssProperty StrokeOpacity        = new("stroke-opacity");
    public static readonly CssProperty StrokeWidth          = new("stroke-width");

    // Text ------------------------------------------------------------------------
    public static readonly CssProperty TextAlign            = new("text-align", CssModule.Core1);
    public static readonly CssProperty TextAlignLast        = new("text-align-last");
    public static readonly CssProperty TextDecoration       = new("text-decoration", CssModule.Core1);
    public static readonly CssProperty TextDecorationColor  = new("text-decoration-color");
    public static readonly CssProperty TextDecorationLine   = new("text-decoration-line");
    public static readonly CssProperty TextDecorationSkip   = new("text-decoration-skip");
    public static readonly CssProperty TextDecorationStyle  = new("text-decoration-style");
    public static readonly CssProperty TextEmphasis         = new("text-emphasis");
    public static readonly CssProperty TextEmphasisColor    = new("text-emphasis-color");
    public static readonly CssProperty TextEmphasisPosition = new("text-emphasis-position");
    public static readonly CssProperty TextEmphasisStyle    = new("text-emphasis-style");
    public static readonly CssProperty TextHeight           = new("text-height");
    public static readonly CssProperty TextIndent           = new("text-indent", CssModule.Core1);
    public static readonly CssProperty TextJustify          = new("text-justify");
    public static readonly CssProperty TextOutline          = new("text-outline");

    public static readonly CssProperty TextShadow           = new("text-shadow", new CssCompatibility(
        standard: new (chrome: 2, edge: 10, firefox: 3.5f, safari: 4)
    ));

    public static readonly CssProperty TextSpaceCollapse     = new("text-space-collapse");
    public static readonly CssProperty TextTransform         = new("text-transform", CssModule.Core1);
    public static readonly CssProperty TextUnderlinePosition = new("text-underline-position");
    public static readonly CssProperty TextWrap              = new("text-wrap");

    public static readonly CssProperty Top = new ("top", CssModule.Core1);

    // Transforms (Level 3) -------------------------------------------------------------------------------------------------------------------
    public static readonly CssProperty Transform                = new("transform",        CssModule.Transforms3);
    public static readonly CssProperty TransformOrigin          = new("transform-origin", CssModule.Transforms3);
    public static readonly CssProperty TransformStyle           = new("transform-style",  CssModule.Transforms3);

    // - Transitions (Level 3 ) ---------------------------------------------------------------------------------------------------------------

    public static readonly CssProperty Transition               = new("transition",                 CssModule.Transitions3);
    public static readonly CssProperty TransitionDelay          = new("transition-delay",           CssModule.Transitions3);
    public static readonly CssProperty TransitionDuration       = new("transition-duration",        CssModule.Transitions3);
    public static readonly CssProperty TransitionProperty       = new("transition-property",        CssModule.Transitions3);
    public static readonly CssProperty TransitionTimingFunction = new("transition-timing-function", CssModule.Transitions3);

    // - Unicode -------------------------------------------------------------------------
    public static readonly CssProperty UnicodeBidi  = new("unicode-bidi");
    public static readonly CssProperty UnicodeRange = new("unicode-range");

    public static readonly CssProperty UserSelect = new("user-select", new CssCompatibility(
        prefixed: new (chrome: 1,  edge: 10, firefox: 1, safari: 3),
        standard: new (chrome: 54, edge: 79, firefox: 69)
    ));

    public static readonly CssProperty VerticalAlign = new("vertical-align", CssModule.Core1);
    public static readonly CssProperty Visibility    = new("visibility",     CssModule.Core1);
    public static readonly CssProperty WhiteSpace    = new("white-space",    CssModule.Text3);
    public static readonly CssProperty Widows        = new("widows",         CssModule.Core2_1);
    public static readonly CssProperty Width         = new("width",          CssModule.Core1);

    // Words
    public static readonly CssProperty WordBreak     = new("word-break",   CssModule.Text3);
    public static readonly CssProperty WordSpacing   = new("word-spacing", CssModule.Core1);
    public static readonly CssProperty WordWrap      = new("word-wrap",    CssModule.Text3);

    public static readonly CssProperty ZIndex        = new("z-index", CssModule.Core1);            

    public static readonly Dictionary<string, CssProperty> Map = new() {
        // Animations
        { "animation",                  Animation },
        { "animation-delay",            AnimationDelay },
        { "animation-direction",        AnimationDirection },
        { "animation-duration",         AnimationDuration },
        { "animation-iteration-count",  AnimationIterationCount },
        { "animation-name",             AnimationName },
        { "animation-play-state",       AnimationPlayState },
        { "animation-timing-function",  AnimationTimingFunction },
                                        
        { "appearance",                 Appearance },
        { "aspect-ratio",               AspectRatio },
        { "azimuth",                    Azimuth },
        { "backdrop-filter",            BackdropFilter },
        { "backface-visibility",        BackfaceVisibility },

        // Backgrounds
        { "background",                 Background },
        { "background-attachment",      BackgroundAttachment },
        { "background-clip",            BackgroundClip },
        { "background-color",           BackgroundColor },
        { "background-image",           BackgroundImage },
        { "background-origin",          BackgroundOrigin },
        { "background-position",        BackgroundPosition },
        { "background-repeat",          BackgroundRepeat },
        { "background-size",            BackgroundSize },

        // Borders
        { "border",                     Border },
        { "border-bottom",              BorderBottom },
        { "border-bottom-color",        BorderBottomColor },
        { "border-bottom-left-radius",  BorderBottomLeftRadius },
        { "border-bottom-right-radius", BorderBottomRightRadius },
        { "border-bottom-style",        BorderBottomStyle },
        { "border-bottom-width",        BorderBottomWidth },
        { "border-collapse",            BorderCollapse },
        { "border-color",               BorderColor },
        { "border-image",               BorderImage },
        { "border-image-outset",        BorderImageOutset },
        { "border-image-repeat",        BorderImageRepeat },
        { "border-image-slice",         BorderImageSlice },
        { "border-image-source",        BorderImageSource },
        { "border-image-width",         BorderImageWidth },
        { "border-left",                BorderLeft },
        { "border-left-color",          BorderLeftColor },
        { "border-left-style",          BorderLeftStyle },
        { "border-left-width",          BorderLeftWidth },
        { "border-radius",              BorderRadius },
        { "border-right",               BorderRight },
        { "border-right-color",         BorderRightColor },
        { "border-right-style",         BorderRightStyle },
        { "border-right-width",         BorderRightWidth },
        { "border-spacing",             BorderSpacing },
        { "border-style",               BorderStyle },
        { "border-top",                 BorderTop },
        { "border-top-color",           BorderTopColor },
        { "border-top-left-radius",     BorderTopLeftRadius },
        { "border-top-right-radius",    BorderTopRightRadius },
        { "border-top-style",           BorderTopStyle },
        { "border-top-width",           BorderTopWidth },
        { "border-width",               BorderWidth },

        { "bottom",                     Bottom },
        { "box-decoration-break",       BoxDecorationBreak },
        { "box-shadow",                 BoxShadow },
        { "box-sizing",                 BoxSizing },
        { "break-after",                BreakAfter },
        { "break-before",               BreakBefore },
        { "break-inside",               BreakInside },
        { "caption-side",               CaptionSide },
        { "clear",                      Clear },
                                        
        // Clipping                     
        { "clip",                   Clip },
        { "clip-path",              ClipPath },
        { "clip-rule",              ClipRule },
                                    
        { "color",                  Color },
        { "color-mix",              ColorMix },
        // Column
        { "column-count",           ColumnCount },
        { "column-fill",            ColumnFill },
        { "column-gap",             ColumnGap },
        { "column-rule",            ColumnRule },
        { "column-rule-color",      ColumnRuleColor },
        { "column-rule-style",      ColumnRuleStyle },
        { "column-rule-width",      ColumnRuleWidth },
        { "column-span",            ColumnSpan },
        { "column-width",           ColumnWidth },
                                    
        { "columns",                Columns },
        { "content",                Content },
        { "counter-increment",      CounterIncrement },
        { "counter-reset",          CounterReset },
        { "cursor",                 Cursor },
        { "direction",              Direction },
        { "display",                Display },
        { "empty-cells",            EmptyCells },
        { "fill",                   Fill },
        { "fill-opacity",           FillOpacity },
        { "filter",                 Filter },
        { "fit",                    Fit },
        { "fit-position",           FitPosition },

        // Containment
        { "contain",                Contain },
        { "container",              Container },
        { "container-name",         ContainerName },
        { "container-type",         ContainerType },

        // Flexbox
        { "flex",                   Flex },
        { "flex-basis",             FlexBasis },
        { "flex-direction",         FlexDirection },
        { "flex-flow",              FlexFlow },
        { "flex-grow",              FlexGrow },
        { "flex-shrink",            FlexShrink },
        { "flex-wrap",              FlexWrap },
                                        
        { "float",                  Float },

        // Font
        { "font",                   Font },
        { "font-family",            FontFamily },
        { "font-size",              FontSize },
        { "font-size-adjust",       FontSizeAdjust },
        { "font-stretch",           FontStretch },
        { "font-style",             FontStyle },
        { "font-variant",           FontVariant },
        { "font-weight",            FontWeight },
                                    
        // Grids                    
        { "grid",                   Grid },
        { "grid-auto-columns",      GridAutoColumns },
        { "grid-auto-flow",         GridAutoFlow },
        { "grid-auto-rows",         GridAutoRows },
        { "grid-template-areas",    GridTemplateAreas },
        { "grid-template-columns",  GridTemplateColumns },
        { "grid-template-rows",     GridTemplateRows },
        { "grid-columns",           GridColumns },
        { "grid-rows",              GridRows },

        { "height",                 Height },

        { "hyphenate-after",        HyphenateAfter },
        { "hyphenate-before",       HyphenateBefore },
        { "hyphenate-character",    HyphenateCharacter },
        { "hyphenate-lines",        HyphenateLines },
        { "hyphenate-resource",     HyphenateResource },
        { "hyphens",                Hyphens },
        { "inline-box-align",       InlineBoxAlign },
        { "left",                   Left },
        { "letter-spacing",         LetterSpacing },
        { "line-break",             LineBreak },
        { "line-height",            LineHeight },
        { "list-style",             ListStyle },
        { "list-style-image",       ListStyleImage },
        { "list-style-position",    ListStylePosition },
        { "list-style-type",        ListStyleType },
        { "margin",                 Margin },
        { "margin-bottom",          MarginBottom },
        { "margin-left",            MarginLeft },
        { "margin-right",           MarginRight },
        { "margin-top",             MarginTop },

        // Masking
        { "mask",               Mask },
        { "mask-border",        MaskBorder },
        { "mask-border-mode",   MaskBorderMode },
        { "mask-border-outset", MaskBorderOutset },
        { "mask-border-repeat", MaskBorderRepeat },
        { "mask-border-slice" , MaskBorderSlice },
        { "mask-border-source", MaskBorderSource },
        { "mask-border-width",  MaskBorderWidth },
        { "mask-clip",          MaskClip },
        { "mask-composite",     MaskComposite },
        { "mask-image",         MaskImage },
        { "mask-mode",          MaskMode },
        { "mask-origin",        MaskOrigin },
        { "mask-position",      MaskPosition },
        { "mask-repeat",        MaskRepeat },
        { "mask-size",          MaskSize },

        { "max-height",         MaxHeight },
        { "max-width",          MaxWidth },
        { "min-height",         MinHeight },
        { "min-width",          MinWidth },
        { "opacity",            Opacity },
        { "order",              Order },
        { "orphans",            Orphans },
        { "outline",            Outline },

        // Outline
        { "outline-color",  OutlineColor },
        { "outline-offset", OutlineOffset },
        { "outline-style",  OutlineStyle },
        { "outline-width",  OutlineWidth },

        // Overflow
        { "overflow",       Overflow },
        { "overflow-style", OverflowStyle },
        { "overflow-wrap",  OverflowWrap },
        { "overflow-x",     OverflowX },
        { "overflow-y",     OverflowY },

        // Padding
        { "padding",        Padding },
        { "padding-bottom", PaddingBottom },
        { "padding-left",   PaddingLeft },
        { "padding-right",  PaddingRight },
        { "padding-top",    PaddingTop },

        { "page",                 Page },
        { "page-break-after",     PageBreakAfter },
        { "page-break-before",    PageBreakBefore },
        { "page-break-inside",    PageBreakInside },

        // Perspective
        { "perspective",          Perspective },
        { "perspective-origin",   PerspectiveOrigin },
        { "perspective-origin-x", PerspectiveOriginX },
        { "perspective-origin-y", PerspectiveOriginY },

        { "position",             Position },
        { "quotes",               Quotes },
        { "resize",               Resize },
        { "right",                Right },
        { "rotate",               Rotate },

        // Ruby
        { "ruby-align",    RubyAlign },
        { "ruby-overhang", RubyOverhang },
        { "ruby-position", RubyPosition },
        { "ruby-span",     RubySpan },

        { "size",  Size },
        { "speak", Speak },

        // Strokes
        { "stroke",                  Stroke },
        { "stroke-dasharray",        StrokeDashArray },
        { "stroke-dashoffset",       StrokeDashOffset },
        { "stroke-linecap",          StrokeLinecap },
        { "stroke-linejoin",         StrokeLinejoin },
        { "stroke-opacity",          StrokeOpacity },
        { "stroke-width",            StrokeWidth },

        { "table-layout",            TableLayout },
        { "text-align",              TextAlign },
        { "text-align-last",         TextAlignLast },

        // Text decoration
        { "text-decoration",         TextDecoration },
        { "text-decoration-color",   TextDecorationColor },
        { "text-decoration-line",    TextDecorationLine },
        { "text-decoration-skip",    TextDecorationSkip },
        { "text-decoration-style",   TextDecorationStyle },
                                     
        { "text-emphasis",           TextEmphasis },
        { "text-emphasis-color",     TextEmphasisColor },
        { "text-emphasis-position",  TextEmphasisPosition },
        { "text-emphasis-style",     TextEmphasisStyle },
        { "text-height",             TextHeight },
        { "text-indent",             TextIndent },
        { "text-justify",            TextJustify },
        { "text-outline",            TextOutline },
        { "text-shadow",             TextShadow },
        { "text-space-collapse",     TextSpaceCollapse },
        { "text-transform",          TextTransform },
        { "text-underline-position", TextUnderlinePosition },
        { "text-wrap",               TextWrap },

        { "top", Top },

        // Transform
        { "transform",                  Transform },
        { "transform-origin",           TransformOrigin },
        { "transform-style",            TransformStyle },
        { "transition",                 Transition },
        { "transition-delay",           TransitionDelay },
        { "transition-duration",        TransitionDuration },
        { "transition-property",        TransitionProperty },

        { "transition-timing-function", TransitionTimingFunction },
        { "unicode-bidi",               UnicodeBidi },
        { "unicode-range",              UnicodeRange },
        { "user-select",                UserSelect },
        { "vertical-align",             VerticalAlign },
        { "visibility",                 Visibility },
        { "white-space",                WhiteSpace },
        { "widows",                     Widows },
        { "width",                      Width },
        { "word-break",                 WordBreak },
        { "word-spacing",               WordSpacing },
        { "word-wrap",                  WordWrap },
        { "z-index",                    ZIndex }
    };
}