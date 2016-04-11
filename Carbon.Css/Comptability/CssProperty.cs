using System;
using System.Collections.Generic;

namespace Carbon.Css
{
    public class CssProperty
    {
        private readonly string name;
        private readonly CssModule module;
        private readonly CssCompatibility compatibility;

        public CssProperty(string name)
            : this(name, null, null)
        { }

        public CssProperty(string name, CssCompatibility compatibility)
            : this(name, null, compatibility)
        { }

        public CssProperty(string name, CssModule module = null, CssCompatibility compatibility = null)
        {
            #region Preconditions

            if (name == null) throw new ArgumentNullException("name");

            #endregion

            this.name = name;
            this.module = module;

            this.compatibility = compatibility;

            if (compatibility == null && module != null)
            {
                this.compatibility = module;
            }
        }

        public bool IsStandard => Module != null;

        public string Name => name;

        public CssModule Module => module;

        public CssCompatibility Compatibility
            => compatibility ?? CssCompatibility.Unknown;

 
        public bool NeedsExpansion(CssDeclaration declaration, Browser[] browsers)
        {
            if (browsers == null || browsers.Length == 0) return false;

            if (!Compatibility.HasPatches) return false;

            foreach (var browser in browsers)
            {
                if (Compatibility.HasPatch(declaration, browser)) return true;
            }

            return false;
        }

        public override int GetHashCode() => name.GetHashCode();

        public override bool Equals(object obj)
        {
            if (obj is string)
                return (string)obj == this.name;

            var a = obj as CssProperty;

            if (a == null) return false;

            return a.Name == this.name;
        }

        public override string ToString() => name;

        public static CssProperty Get(string name)
        {
            CssProperty propertyInfo;

            if (!Map.TryGetValue(name, out propertyInfo))
            {
                propertyInfo = new CssProperty(name);
            }

            return propertyInfo;
        }

        // Animations (Level 3) -----------------------------------------------------------------------------------------------------------
        public static readonly CssProperty Animation = new CssProperty("animation", CssModule.Animations3);
        public static readonly CssProperty AnimationDelay = new CssProperty("animation-delay", CssModule.Animations3);
        public static readonly CssProperty AnimationDirection = new CssProperty("animation-direction", CssModule.Animations3);
        public static readonly CssProperty AnimationDuration = new CssProperty("animation-duration", CssModule.Animations3);
        public static readonly CssProperty AnimationIterationCount = new CssProperty("animation-iteration-count", CssModule.Animations3);
        public static readonly CssProperty AnimationName = new CssProperty("animation-name", CssModule.Animations3);
        public static readonly CssProperty AnimationPlayState = new CssProperty("animation-play-state", CssModule.Animations3);
        public static readonly CssProperty AnimationTimingFunction = new CssProperty("animation-timing-function", CssModule.Animations3);

        public static readonly CssProperty Appearance = new CssProperty("appearance", new CssCompatibility(
            prefixed: new CompatibilityTable(chrome: 1, firefox: 1, safari: 3)
        ));

        public static readonly CssProperty Azimuth = new CssProperty("azimuth", CssModule.Core2_1);

        public static readonly CssProperty BackfaceVisibility = new CssProperty("backface-visibility", CssModule.Transforms3);

        // Backgrounds
        public static readonly CssProperty Background = new CssProperty("background", CssModule.Core1);
        public static readonly CssProperty BackgroundAttachment = new CssProperty("background-attachment", CssModule.Core1);

        public static readonly CssProperty BackgroundClip = new CssProperty("background-clip", CssModule.BackgroundsAndBorders3, new CssCompatibility(
            prefixed: new CompatibilityTable(chrome: 4, firefox: 4, safari: 4),
            standard: new CompatibilityTable(firefox: 4, chrome: 15, ie: 9, safari: 7)
        ));

        public static readonly CssProperty BackgroundColor = new CssProperty("background-color", CssModule.Core1);
        public static readonly CssProperty BackgroundImage = new CssProperty("background-image", CssModule.Core1);

        public static readonly CssProperty BackgroundOrigin = new CssProperty("background-origin", CssModule.BackgroundsAndBorders3, new CssCompatibility(
            standard: new CompatibilityTable(chrome: 1, firefox: 4, ie: 9, safari: 3)
        ));

        public static readonly CssProperty BackgroundPosition = new CssProperty("background-position", CssModule.Core1);
        public static readonly CssProperty BackgroundRepeat = new CssProperty("background-repeat", CssModule.Core1);
        public static readonly CssProperty BackgroundSize = new CssProperty("background-size", CssModule.BackgroundsAndBorders3);

        // Borders -------------------------------------------------------------------------------------------------------
        public static readonly CssCompatibility BorderImageCompatibility = new CssCompatibility(
            prefixed: new CompatibilityTable(chrome: 7, firefox: 3.5f, safari: 3),
            standard: new CompatibilityTable(chrome: 16, firefox: 15, ie: 11, safari: 6.1f)
        );

        public static readonly CssProperty Border = new CssProperty("border", CssModule.Core1);
        public static readonly CssProperty BorderBottom = new CssProperty("border-bottom", CssModule.Core1);
        public static readonly CssProperty BorderBottomColor = new CssProperty("border-bottom-color", CssModule.Core1);
        public static readonly CssProperty BorderBottomLeftRadius = new CssProperty("border-bottom-left-radius");
        public static readonly CssProperty BorderBottomRightRadius = new CssProperty("border-bottom-right-radius");
        public static readonly CssProperty BorderBottomStyle = new CssProperty("border-bottom-style");
        public static readonly CssProperty BorderBottomWidth = new CssProperty("border-bottom-width");
        public static readonly CssProperty BorderCollapse = new CssProperty("border-collapse");
        public static readonly CssProperty BorderColor = new CssProperty("border-color", CssModule.Core1);
        public static readonly CssProperty BorderImage = new CssProperty("border-image", BorderImageCompatibility);
        public static readonly CssProperty BorderImageOutset = new CssProperty("border-image-outset", BorderImageCompatibility);
        public static readonly CssProperty BorderImageRepeat = new CssProperty("border-image-repeat", BorderImageCompatibility);
        public static readonly CssProperty BorderImageSlice = new CssProperty("border-image-slice", BorderImageCompatibility);
        public static readonly CssProperty BorderImageSource = new CssProperty("border-image-source", BorderImageCompatibility);
        public static readonly CssProperty BorderImageWidth = new CssProperty("border-image-width", BorderImageCompatibility);

        public static readonly CssProperty BorderLeft = new CssProperty("border-left", CssModule.Core1);
        public static readonly CssProperty BorderLeftColor = new CssProperty("border-left-color");
        public static readonly CssProperty BorderLeftStyle = new CssProperty("border-left-style");
        public static readonly CssProperty BorderLeftWidth = new CssProperty("border-left-width");
        public static readonly CssProperty BorderRadius = new CssProperty("border-radius", CssModule.BackgroundsAndBorders3);
        public static readonly CssProperty BorderRight = new CssProperty("border-right", CssModule.Core1);
        public static readonly CssProperty BorderRightColor = new CssProperty("border-right-color");
        public static readonly CssProperty BorderRightStyle = new CssProperty("border-right-style");
        public static readonly CssProperty BorderRightWidth = new CssProperty("border-right-width");
        public static readonly CssProperty BorderSpacing = new CssProperty("border-spacing");
        public static readonly CssProperty BorderStyle = new CssProperty("border-style", CssModule.Core1);
        public static readonly CssProperty BorderTop = new CssProperty("border-top", CssModule.Core1);
        public static readonly CssProperty BorderTopColor = new CssProperty("border-top-color");
        public static readonly CssProperty BorderTopLeftRadius = new CssProperty("border-top-left-radius");
        public static readonly CssProperty BorderTopRightRadius = new CssProperty("border-top-right-radius");
        public static readonly CssProperty BorderTopStyle = new CssProperty("border-top-style");
        public static readonly CssProperty BorderTopWidth = new CssProperty("border-top-width", CssModule.Core1);
        public static readonly CssProperty BorderWidth = new CssProperty("border-width", CssModule.Core1);

        public static readonly CssProperty Bottom = new CssProperty("bottom", CssModule.Core1);
        public static readonly CssProperty BoxDecorationBreak = new CssProperty("box-decoration-break");

        public static readonly CssProperty BoxShadow = new CssProperty("box-shadow", CssModule.UI(3), new CssCompatibility(
            prefixed: new CompatibilityTable(chrome: 1, firefox: 3.5f, safari: 3.1f),
            standard: new CompatibilityTable(chrome: 10, firefox: 4, ie: 9, safari: 5.1f)
        ));

        public static readonly CssProperty BoxSizing = new CssProperty("box-sizing", CssModule.UI(3), new CssCompatibility(
            prefixed: new CompatibilityTable(chrome: 1, firefox: 1, ie: 8, safari: 3),
            standard: new CompatibilityTable(chrome: 10, firefox: 29, ie: 9, safari: 5.1f)
        ));

        // Breaks
        public static readonly CssProperty BreakAfter = new CssProperty("break-after");
        public static readonly CssProperty BreakBefore = new CssProperty("break-before");
        public static readonly CssProperty BreakInside = new CssProperty("break-inside");

        public static readonly CssProperty CaptionSide = new CssProperty("caption-side");
        public static readonly CssProperty Clear = new CssProperty("clear");

        public static readonly CssProperty Clip = new CssProperty("clip");

        // TODO: Confirm support
        public static readonly CssProperty ClipPath = new CssProperty("clip-path", new CssCompatibility(
            prefixed: new CompatibilityTable(chrome: 24f, safari: 7)   
        ));

        public static readonly CssProperty Color = new CssProperty("color", CssModule.Core1);

        public static readonly CssProperty ColumnCount = new CssProperty("column-count", CssModule.Columns3);
        public static readonly CssProperty ColumnFill = new CssProperty("column-fill", CssModule.Columns3);
        public static readonly CssProperty ColumnGap = new CssProperty("column-gap", CssModule.Columns3);
        public static readonly CssProperty ColumnRule = new CssProperty("column-rule", CssModule.Columns3);
        public static readonly CssProperty ColumnRuleColor = new CssProperty("column-rule-color", CssModule.Columns3);
        public static readonly CssProperty ColumnRuleStyle = new CssProperty("column-rule-style", CssModule.Columns3);
        public static readonly CssProperty ColumnRuleWidth = new CssProperty("column-rule-width", CssModule.Columns3);
        public static readonly CssProperty ColumnSpan = new CssProperty("column-span", CssModule.Columns3);
        public static readonly CssProperty ColumnWidth = new CssProperty("column-width", CssModule.Columns3);
        public static readonly CssProperty Columns = new CssProperty("columns", CssModule.Columns3);

        public static readonly CssProperty Content = new CssProperty("content", CssModule.Core2_1);

        // Counters ---------------------------------------------------------------------------------------
        public static readonly CssProperty CounterIncrement = new CssProperty("counter-increment");
        public static readonly CssProperty CounterReset = new CssProperty("counter-reset");

        public static readonly CssProperty Cursor = new CssProperty("cursor", new CursorCompatibility());
        public static readonly CssProperty Direction = new CssProperty("direction");
        public static readonly CssProperty Display = new CssProperty("display", CssModule.Core1);

        public static readonly CssProperty EmptyCells = new CssProperty("empty-cells");
        public static readonly CssProperty Filter = new CssProperty("filter");

        // Fit ---------------------------------------------------------------------------------------
        public static readonly CssProperty Fit = new CssProperty("fit");
        public static readonly CssProperty FitPosition = new CssProperty("fit-position");

        // Flex ---------------------------------------------------------------------------------------
        public static readonly CssProperty FlexAlign = new CssProperty("flex-align");
        public static readonly CssProperty FlexFlow = new CssProperty("flex-flow");
        public static readonly CssProperty FlexLinePack = new CssProperty("flex-line-pack");
        public static readonly CssProperty FlexOrder = new CssProperty("flex-order");
        public static readonly CssProperty FlexPack = new CssProperty("flex-pack");

        public static readonly CssProperty Float = new CssProperty("float", CssModule.Core1);
        public static readonly CssProperty FloatOffset = new CssProperty("float-offset");

        // Fonts -------------------------------------------------------------------------------------------
        public static readonly CssProperty Font = new CssProperty("font", CssModule.Core1);
        public static readonly CssProperty FontFamily = new CssProperty("font-family", CssModule.Core1);
        public static readonly CssProperty FontSize = new CssProperty("font-size", CssModule.Core1);
        public static readonly CssProperty FontSizeAdjust = new CssProperty("font-size-adjust");
        public static readonly CssProperty FontStretch = new CssProperty("font-stretch", CssModule.Fonts3);
        public static readonly CssProperty FontStyle = new CssProperty("font-style", CssModule.Core1);
        public static readonly CssProperty FontVariant = new CssProperty("font-variant", CssModule.Core1);
        public static readonly CssProperty FontWeight = new CssProperty("font-weight", CssModule.Core1);

        // Grids ---------------------------------------------------------------------------------------
        public static readonly CssCompatibility GridComptability = new CssCompatibility(
            prefixed: new CompatibilityTable(ie: 10)
        );

        public static readonly CssProperty GridColumns = new CssProperty("grid-columns", GridComptability);
        public static readonly CssProperty GridRows = new CssProperty("grid-rows", GridComptability);

        public static readonly CssProperty Height = new CssProperty("height", CssModule.Core1);

        // Hyphens -------------------------------------------------------------------------------------
        public static readonly CssProperty HyphenateAfter = new CssProperty("hyphenate-after");
        public static readonly CssProperty HyphenateBefore = new CssProperty("hyphenate-before");
        public static readonly CssProperty HyphenateCharacter = new CssProperty("hyphenate-character", CssModule.Text3);
        public static readonly CssProperty HyphenateLines = new CssProperty("hyphenate-lines");
        public static readonly CssProperty HyphenateResource = new CssProperty("hyphenate-resource");

        public static readonly CssProperty Hyphens = new CssProperty("hyphens", new CssModule(CssModuleType.Text, 3), new CssCompatibility(
            prefixed: new CompatibilityTable(chrome: 15, firefox: 6, ie: 10, safari: 5.1f),
            standard: new CompatibilityTable(firefox: 43)
        ));

        // https://developer.mozilla.org/en-US/docs/Web/CSS/hyphens

        public static readonly CssProperty InlineBoxAlign = new CssProperty("inline-box-align");


        public static readonly CssProperty Left = new CssProperty("left", CssModule.Core1);
        public static readonly CssProperty LetterSpacing = new CssProperty("letter-spacing");

        // Lines
        public static readonly CssProperty LineBreak = new CssProperty("line-break");
        public static readonly CssProperty LineHeight = new CssProperty("line-height", CssModule.Core1);

        // List Styles -------------------------------------------------------------------------------------------------------
        public static readonly CssProperty ListStyle = new CssProperty("list-style", CssModule.Core1);
        public static readonly CssProperty ListStyleImage = new CssProperty("list-style-image", CssModule.Core1);
        public static readonly CssProperty ListStylePosition = new CssProperty("list-style-position", CssModule.Core1);
        public static readonly CssProperty ListStyleType = new CssProperty("list-style-type", CssModule.Core1);

        // Margins -----------------------------------------------------------------------------------------------------------
        public static readonly CssProperty Margin = new CssProperty("margin", CssModule.Core1);
        public static readonly CssProperty MarginBottom = new CssProperty("margin-bottom", CssModule.Core1);
        public static readonly CssProperty MarginLeft = new CssProperty("margin-left", CssModule.Core1);
        public static readonly CssProperty MarginRight = new CssProperty("margin-right", CssModule.Core1);
        public static readonly CssProperty MarginTop = new CssProperty("margin-top", CssModule.Core1);

        // Marquee -----------------------------------------------------------------------------------------
        public static readonly CssProperty MarqueeDirection = new CssProperty("marquee-direction");
        public static readonly CssProperty MarqueeLoop = new CssProperty("marquee-loop");
        public static readonly CssProperty MarqueePlayCount = new CssProperty("marquee-play-count");
        public static readonly CssProperty MarqueeSpeed = new CssProperty("marquee-speed");
        public static readonly CssProperty MarqueeStyle = new CssProperty("marquee-style");

        public static readonly CssProperty MaxHeight = new CssProperty("max-height", CssModule.Core2_1);
        public static readonly CssProperty MaxWidth = new CssProperty("max-width", CssModule.Core2_1);

        public static readonly CssProperty MinHeight = new CssProperty("min-height", CssModule.Core2_1);
        public static readonly CssProperty MinWidth = new CssProperty("min-width", CssModule.Core2_1);

        // <= IE8 filter: alpha(opacity=xx)
        // IE8 introduced -ms-filter, which is synonymous with filter. Both are gone in IE10
        public static readonly CssProperty Opacity = new CssProperty("opacity", CssModule.Color3);

        public static readonly CssProperty Orphans = new CssProperty("orphans", new CssModule(CssModuleType.Core, 2.1f), new CssCompatibility(
            standard: new CompatibilityTable(ie: 8)
        ));

        // Outlines -------------------------------------------------------------------------------
        public static readonly CssProperty Outline = new CssProperty("outline", CssModule.Core2_1);
        public static readonly CssProperty OutlineColor = new CssProperty("outline-color", CssModule.Core2_1);
        public static readonly CssProperty OutlineOffset = new CssProperty("outline-offset", CssModule.Core2_1);
        public static readonly CssProperty OutlineStyle = new CssProperty("outline-style", CssModule.Core2_1);
        public static readonly CssProperty OutlineWidth = new CssProperty("outline-width", CssModule.Core2_1);

        // Overflow -------------------------------------------------------------------------------
        public static readonly CssProperty Overflow = new CssProperty("overflow", CssModule.Core2_1);
        public static readonly CssProperty OverflowStyle = new CssProperty("overflow-style");
        public static readonly CssProperty OverflowWrap = new CssProperty("overflow-wrap");
        public static readonly CssProperty OverflowX = new CssProperty("overflow-x", CssModule.Core2_1);
        public static readonly CssProperty OverflowY = new CssProperty("overflow-y", CssModule.Core2_1);

        // Padding -----------------------------------------------------------------------------------------------
        public static readonly CssProperty Padding = new CssProperty("padding", CssModule.Core1);
        public static readonly CssProperty PaddingBottom = new CssProperty("padding-bottom", CssModule.Core1);
        public static readonly CssProperty PaddingLeft = new CssProperty("padding-left", CssModule.Core1);
        public static readonly CssProperty PaddingRight = new CssProperty("padding-right", CssModule.Core1);
        public static readonly CssProperty PaddingTop = new CssProperty("padding-top", CssModule.Core1);

        public static readonly CssProperty Page = new CssProperty("page");

        // Page Breaks -------------------------------------------------------------------------------
        public static readonly CssProperty PageBreakAfter = new CssProperty("page-break-after");
        public static readonly CssProperty PageBreakBefore = new CssProperty("page-break-before");
        public static readonly CssProperty PageBreakInside = new CssProperty("page-break-inside");

        // Perspective
        public static readonly CssProperty Perspective = new CssProperty("perspective", CssModule.Transforms3);
        public static readonly CssProperty PerspectiveOrigin = new CssProperty("perspective-origin", CssModule.Transforms3);
        public static readonly CssProperty PerspectiveOriginX = new CssProperty("perspective-origin-x", CssModule.Transforms3);
        public static readonly CssProperty PerspectiveOriginY = new CssProperty("perspective-origin-y", CssModule.Transforms3);

        public static readonly CssProperty Position = new CssProperty("position", CssModule.Core1);

      
        public static readonly CssProperty Quotes = new CssProperty("quotes");
        public static readonly CssProperty Resize = new CssProperty("resize");
        public static readonly CssProperty Right = new CssProperty("right", CssModule.Core1);

        // Ruby (Level 3) ------------------------------------------------------------------------------------

        public static readonly CssModule RubyLevel3 = new CssModule(CssModuleType.Ruby, 3);

        public static readonly CssProperty RubyAlign = new CssProperty("ruby-align", RubyLevel3);
        public static readonly CssProperty RubyOverhang = new CssProperty("ruby-overhang", RubyLevel3);
        public static readonly CssProperty RubyPosition = new CssProperty("ruby-position", RubyLevel3);
        public static readonly CssProperty RubySpan = new CssProperty("ruby-span", RubyLevel3);

        public static readonly CssProperty Size = new CssProperty("size");
        public static readonly CssProperty Speak = new CssProperty("speak");

        public static readonly CssProperty TableLayout = new CssProperty("table-layout", CssModule.Core2_1);

        // Text ------------------------------------------------------------------------
        public static readonly CssProperty TextAlign = new CssProperty("text-align", CssModule.Core1);
        public static readonly CssProperty TextAlignLast = new CssProperty("text-align-last");
        public static readonly CssProperty TextDecoration = new CssProperty("text-decoration", CssModule.Core1);
        public static readonly CssProperty TextDecorationColor = new CssProperty("text-decoration-color");
        public static readonly CssProperty TextDecorationLine = new CssProperty("text-decoration-line");
        public static readonly CssProperty TextDecorationSkip = new CssProperty("text-decoration-skip");
        public static readonly CssProperty TextDecorationStyle = new CssProperty("text-decoration-style");
        public static readonly CssProperty TextEmphasis = new CssProperty("text-emphasis");
        public static readonly CssProperty TextEmphasisColor = new CssProperty("text-emphasis-color");
        public static readonly CssProperty TextEmphasisPosition = new CssProperty("text-emphasis-position");
        public static readonly CssProperty TextEmphasisStyle = new CssProperty("text-emphasis-style");
        public static readonly CssProperty TextHeight = new CssProperty("text-height");
        public static readonly CssProperty TextIndent = new CssProperty("text-indent", CssModule.Core1);
        public static readonly CssProperty TextJustify = new CssProperty("text-justify");
        public static readonly CssProperty TextOutline = new CssProperty("text-outline");

        public static readonly CssProperty TextShadow = new CssProperty("text-shadow", new CssCompatibility(
            standard: new CompatibilityTable(chrome: 2, firefox: 3.5f, ie: 10, safari: 4)
        ));

        public static readonly CssProperty TextSpaceCollapse = new CssProperty("text-space-collapse");
        public static readonly CssProperty TextTransform = new CssProperty("text-transform", CssModule.Core1);
        public static readonly CssProperty TextUnderlinePosition = new CssProperty("text-underline-position");
        public static readonly CssProperty TextWrap = new CssProperty("text-wrap");

        public static readonly CssProperty Top = new CssProperty("top", CssModule.Core1);

        // Transforms (Level 3) -------------------------------------------------------------------------------------------------------------------
        public static readonly CssProperty Transform = new CssProperty("transform", CssModule.Transforms3);
        public static readonly CssProperty TransformOrigin = new CssProperty("transform-origin", CssModule.Transforms3);
        public static readonly CssProperty TransformStyle = new CssProperty("transform-style", CssModule.Transforms3);

        // - Transitions (Level 3 ) ---------------------------------------------------------------------------------------------------------------

        public static readonly CssProperty Transition = new CssProperty("transition", CssModule.Transitions3);
        public static readonly CssProperty TransitionDelay = new CssProperty("transition-delay", CssModule.Transitions3);
        public static readonly CssProperty TransitionDuration = new CssProperty("transition-duration", CssModule.Transitions3);
        public static readonly CssProperty TransitionProperty = new CssProperty("transition-property", CssModule.Transitions3);
        public static readonly CssProperty TransitionTimingFunction = new CssProperty("transition-timing-function", CssModule.Transitions3);

        // - Unicode -------------------------------------------------------------------------
        public static readonly CssProperty UnicodeBidi = new CssProperty("unicode-bidi");
        public static readonly CssProperty UnicodeRange = new CssProperty("unicode-range");

        public static readonly CssProperty UserSelect = new CssProperty("user-select", new CssCompatibility(
            prefixed: new CompatibilityTable(chrome: 1, firefox: 1, ie: 10, safari: 3)
        ));

        public static readonly CssProperty VerticalAlign = new CssProperty("vertical-align", CssModule.Core1);
        public static readonly CssProperty Visibility = new CssProperty("visibility", CssModule.Core1);
        public static readonly CssProperty WhiteSpace = new CssProperty("white-space");
        public static readonly CssProperty Widows = new CssProperty("widows", CssModule.Core2_1);
        public static readonly CssProperty Width = new CssProperty("width", CssModule.Core1);

        // Words
        public static readonly CssProperty WordBreak = new CssProperty("word-break", CssModule.Text3);
        public static readonly CssProperty WordSpacing = new CssProperty("word-spacing", CssModule.Core1);
        public static readonly CssProperty WordWrap = new CssProperty("word-wrap", CssModule.Text3);

        public static readonly CssProperty ZIndex = new CssProperty("z-index", CssModule.Core1);

        public static readonly IDictionary<string, CssProperty> Map = new Dictionary<string, CssProperty> {
			// Animations
			{ "animation",                  Animation },
            { "animation-delay",            AnimationDelay },
            { "animation-direction",        AnimationDirection },
            { "animation-duration",         AnimationDuration },
            { "animation-iteration-count",  AnimationIterationCount },
            { "animation-name",             AnimationName },
            { "animation-play-state",       AnimationPlayState },
            { "animation-timing-function",  AnimationTimingFunction },

            { "appearance",     Appearance },
            { "azimuth",    Azimuth },
            { "backface-visibility", BackfaceVisibility },
            { "background", Background },
            { "background-attachment", BackgroundAttachment },
            { "background-clip", BackgroundClip },
            { "background-color", BackgroundColor },
            { "background-image", BackgroundImage },
            { "background-origin", BackgroundOrigin },
            { "background-position", BackgroundPosition },
            { "background-repeat", BackgroundRepeat },
            { "background-size", BackgroundSize },
            { "border", Border },
            { "border-bottom", BorderBottom },
            { "border-bottom-color", BorderBottomColor },
            { "border-bottom-left-radius", BorderBottomLeftRadius },
            { "border-bottom-right-radius", BorderBottomRightRadius },
            { "border-bottom-style", BorderBottomStyle },
            { "border-bottom-width", BorderBottomWidth },
            { "border-collapse", BorderCollapse },
            { "border-color", BorderColor },
            { "border-image", BorderImage },
            { "border-image-outset", BorderImageOutset },
            { "border-image-repeat", BorderImageRepeat },
            { "border-image-slice", BorderImageSlice },
            { "border-image-source", BorderImageSource },
            { "border-image-width", BorderImageWidth },
            { "border-left", BorderLeft },
            { "border-left-color", BorderLeftColor },
            { "border-left-style", BorderLeftStyle },
            { "border-left-width", BorderLeftWidth },
            { "border-radius", BorderRadius },
            { "border-right", BorderRight },
            { "border-right-color", BorderRightColor },
            { "border-right-style", BorderRightStyle },
            { "border-right-width", BorderRightWidth },
            { "border-spacing", BorderSpacing },
            { "border-style", BorderStyle },
            { "border-top", BorderTop },
            { "border-top-color", BorderTopColor },
            { "border-top-left-radius", BorderTopLeftRadius },
            { "border-top-right-radius", BorderTopRightRadius },
            { "border-top-style", BorderTopStyle },
            { "border-top-width", BorderTopWidth },
            { "border-width", BorderWidth },
            { "bottom", Bottom },
            { "box-decoration-break", BoxDecorationBreak },
            { "box-shadow", BoxShadow },
            { "box-sizing", BoxSizing },
            { "break-after", BreakAfter },
            { "break-before", BreakBefore },
            { "break-inside", BreakInside },
            { "caption-side", CaptionSide },
            { "clear", Clear },
            { "clip", Clip },
            { "clip-path", ClipPath },
            { "color", Color },
            { "column-count", ColumnCount },
            { "column-fill", ColumnFill },
            { "column-gap", ColumnGap },
            { "column-rule", ColumnRule },
            { "column-rule-color", ColumnRuleColor },
            { "column-rule-style", ColumnRuleStyle },
            { "column-rule-width", ColumnRuleWidth },
            { "column-span", ColumnSpan },
            { "column-width", ColumnWidth },
            { "columns", Columns },
            { "content", Content },
            { "counter-increment", CounterIncrement },
            { "counter-reset", CounterReset },
            { "cursor", Cursor },
            { "direction", Direction },
            { "display", Display },
            { "empty-cells", EmptyCells },
            { "filter", Filter },
            { "fit", Fit },
            { "fit-position", FitPosition },
            { "flex-align", FlexAlign },
            { "flex-flow", FlexFlow },
            { "flex-line-pack", FlexLinePack },
            { "flex-order", FlexOrder },
            { "flex-pack", FlexPack },
            { "float", Float },
            { "float-offset", FloatOffset },
            { "font", Font },
            { "font-family", FontFamily },
            { "font-size", FontSize },
            { "font-size-adjust", FontSizeAdjust },
            { "font-stretch", FontStretch },
            { "font-style", FontStyle },
            { "font-variant", FontVariant },
            { "font-weight", FontWeight },
            { "grid-columns", GridColumns },
            { "grid-rows", GridRows },
            { "height", Height },
            { "hyphenate-after", HyphenateAfter },
            { "hyphenate-before", HyphenateBefore },
            { "hyphenate-character", HyphenateCharacter },
            { "hyphenate-lines", HyphenateLines },
            { "hyphenate-resource", HyphenateResource },
            { "hyphens", Hyphens },
            { "inline-box-align", InlineBoxAlign },
            { "left", Left },
            { "letter-spacing", LetterSpacing },
            { "line-break", LineBreak },
            { "line-height", LineHeight },
            { "list-style", ListStyle },
            { "list-style-image", ListStyleImage },
            { "list-style-position", ListStylePosition },
            { "list-style-type", ListStyleType },
            { "margin", Margin },
            { "margin-bottom", MarginBottom },
            { "margin-left", MarginLeft },
            { "margin-right", MarginRight },
            { "margin-top", MarginTop },
            { "marquee-direction", MarqueeDirection },
            { "marquee-loop", MarqueeLoop },
            { "marquee-play-count", MarqueePlayCount },
            { "marquee-speed", MarqueeSpeed },
            { "marquee-style", MarqueeStyle },
            { "max-height", MaxHeight },
            { "max-width", MaxWidth },
            { "min-height", MinHeight },
            { "min-width", MinWidth },
            { "opacity", Opacity },
            { "orphans", Orphans },
            { "outline", Outline },
            { "outline-color", OutlineColor },
            { "outline-offset", OutlineOffset },
            { "outline-style", OutlineStyle },
            { "outline-width", OutlineWidth },
            { "overflow", Overflow },
            { "overflow-style", OverflowStyle },
            { "overflow-wrap", OverflowWrap },
            { "overflow-x", OverflowX },
            { "overflow-y", OverflowY },
            { "padding", Padding },
            { "padding-bottom", PaddingBottom },
            { "padding-left", PaddingLeft },
            { "padding-right", PaddingRight },
            { "padding-top", PaddingTop },
            { "page", Page },
            { "page-break-after", PageBreakAfter },
            { "page-break-before", PageBreakBefore },
            { "page-break-inside", PageBreakInside },
            { "perspective", Perspective },
            { "perspective-origin", PerspectiveOrigin },
            { "perspective-origin-x", PerspectiveOriginX },
            { "perspective-origin-y", PerspectiveOriginY },
            { "position", Position },
            { "quotes", Quotes },
            { "resize", Resize },
            { "right", Right },
            { "ruby-align", RubyAlign },
            { "ruby-overhang", RubyOverhang },
            { "ruby-position", RubyPosition },
            { "ruby-span", RubySpan },
            { "size", Size },
            { "speak", Speak },
            { "table-layout", TableLayout },
            { "text-align", TextAlign },
            { "text-align-last", TextAlignLast },
            { "text-decoration", TextDecoration },
            { "text-decoration-color", TextDecorationColor },
            { "text-decoration-line", TextDecorationLine },
            { "text-decoration-skip", TextDecorationSkip },
            { "text-decoration-style", TextDecorationStyle },
            { "text-emphasis", TextEmphasis },
            { "text-emphasis-color", TextEmphasisColor },
            { "text-emphasis-position", TextEmphasisPosition },
            { "text-emphasis-style", TextEmphasisStyle },
            { "text-height", TextHeight },
            { "text-indent", TextIndent },
            { "text-justify", TextJustify },
            { "text-outline", TextOutline },
            { "text-shadow", TextShadow },
            { "text-space-collapse", TextSpaceCollapse },
            { "text-transform", TextTransform },
            { "text-underline-position", TextUnderlinePosition },
            { "text-wrap", TextWrap },
            { "top", Top },
            { "transform", Transform },
            { "transform-origin", TransformOrigin },
            { "transform-style", TransformStyle },
            { "transition", Transition },
            { "transition-delay", TransitionDelay },
            { "transition-duration", TransitionDuration },
            { "transition-property", TransitionProperty },
            { "transition-timing-function", TransitionTimingFunction },
            { "unicode-bidi", UnicodeBidi },
            { "unicode-range", UnicodeRange },
            { "user-select", UserSelect },
            { "vertical-align", VerticalAlign },
            { "visibility", Visibility },
            { "white-space", WhiteSpace },
            { "widows", Widows },
            { "width", Width },
            { "word-break", WordBreak },
            { "word-spacing", WordSpacing },
            { "word-wrap", WordWrap },
            { "z-index", ZIndex }
        };
    }
}

