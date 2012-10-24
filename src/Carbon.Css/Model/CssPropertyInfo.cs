namespace Carbon.Css
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class Compatibility
	{
		public Compatibility() { }

		public Browser[] Prefixed { get; set; }

		public Browser[] Standard { get; set; }
	}

	public class CssPropertyInfo
	{
		private readonly string name;
		private readonly CssModule module;
		private readonly Compatibility compatibility;

		public CssPropertyInfo(string name)
			: this(name, null, null) { }

		public CssPropertyInfo(string name, Compatibility compatibility)
			: this(name, null, compatibility) { }

		public CssPropertyInfo(string name, CssModule module = null, Compatibility compatibility = null)
		{
			#region Preconditions

			if (name == null) throw new ArgumentNullException("name");

			#endregion

			this.name = name;

			this.module = module;

			this.compatibility = compatibility;

			if (compatibility == null && module != null)
			{
				this.compatibility = module.Compatibility;
			}
		}

		public string Vendor
		{
			get
			{
				if (name.StartsWith("-")) return name.Substring(1);

				return null;
			}
		}

		public bool IsStandard
		{
			get { return Module != null; }
		}

		public string Name
		{
			get { return name; }
		}

		public CssModule Module
		{
			get { return module; }
		}

		public string[] GetPrefixedPropertyNames()
		{
			if (compatibility == null || compatibility.Prefixed == null)
				return new string[0];

			return compatibility.Prefixed
				.Select(b => b.Prefix + name)
				.OrderByDescending(p => p)
				.Distinct()
				.ToArray();
		}

		public override int GetHashCode()
		{
			return this.name.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj is string)
				return (string)obj == this.name;

			var a = obj as CssPropertyInfo;

			if(a == null) return false;

			return a.Name == this.name;
		}

		public override string ToString()
		{
			return this.name;
		}






		public static CssPropertyInfo Get(string name)
		{
			CssPropertyInfo propertyInfo;

			if (!Map.TryGetValue(name, out propertyInfo))
			{
				propertyInfo = new CssPropertyInfo(name);
			}

			return propertyInfo;
		}




		// Animations (Level 3) -----------------------------------------------------------------------------------------------------------
		public static readonly CssPropertyInfo Animation				= new CssPropertyInfo("animation",					CssModule.Animations3);
		public static readonly CssPropertyInfo AnimationDelay			= new CssPropertyInfo("animation-delay",			CssModule.Animations3);
		public static readonly CssPropertyInfo AnimationDirection		= new CssPropertyInfo("animation-direction",		CssModule.Animations3);
		public static readonly CssPropertyInfo AnimationDuration		= new CssPropertyInfo("animation-duration",			CssModule.Animations3);
		public static readonly CssPropertyInfo AnimationIterationCount	= new CssPropertyInfo("animation-iteration-count",	CssModule.Animations3);
		public static readonly CssPropertyInfo AnimationName			= new CssPropertyInfo("animation-name",				CssModule.Animations3);
		public static readonly CssPropertyInfo AnimationPlayState		= new CssPropertyInfo("animation-play-state",		CssModule.Animations3);
		public static readonly CssPropertyInfo AnimationTimingFunction	= new CssPropertyInfo("animation-timing-function",	CssModule.Animations3);

		public static readonly CssPropertyInfo Appearance				= new CssPropertyInfo("appearance", new Compatibility { Prefixed = new[] { Browser.Chrome1, Browser.Firefox1, Browser.Safari3 } });

		public static readonly CssPropertyInfo BackfaceVisibility		= new CssPropertyInfo("backface-visibility", CssModule.Transforms3);

		// Backgrounds
		public static readonly CssPropertyInfo Background				= new CssPropertyInfo("background",				CssModule.Core1);
		public static readonly CssPropertyInfo BackgroundAttachment		= new CssPropertyInfo("background-attachment",	CssModule.Core1);
		public static readonly CssPropertyInfo BackgroundClip			= new CssPropertyInfo("background-clip",		CssModule.BackgroundsAndBorders3);
		public static readonly CssPropertyInfo BackgroundColor			= new CssPropertyInfo("background-color",		CssModule.Core1);
		public static readonly CssPropertyInfo BackgroundImage			= new CssPropertyInfo("background-image",		CssModule.Core1);
		public static readonly CssPropertyInfo BackgroundOrigin			= new CssPropertyInfo("background-origin",		CssModule.BackgroundsAndBorders3, new Compatibility { Standard = new[] { Browser.Chrome1, Browser.Firefox4, Browser.IE9, Browser.Safari3 } });
		public static readonly CssPropertyInfo BackgroundPosition		= new CssPropertyInfo("background-position",	CssModule.Core1);
		public static readonly CssPropertyInfo BackgroundRepeat			= new CssPropertyInfo("background-repeat",		CssModule.Core1);
		public static readonly CssPropertyInfo BackgroundSize			= new CssPropertyInfo("background-size",		CssModule.BackgroundsAndBorders3);

		// Borders -------------------------------------------------------------------------------------------------------
		public static readonly Compatibility BorderImageCompatibility = new Compatibility {
			Prefixed = new[] { Browser.Chrome7, Browser.Firefox(3.5f), Browser.Safari3 },
			Standard = new[] { Browser.Firefox(15) }
		};

		public static readonly CssPropertyInfo Border					= new CssPropertyInfo("border",					CssModule.Core1);
		public static readonly CssPropertyInfo BorderBottom				= new CssPropertyInfo("border-bottom",			CssModule.Core1);
		public static readonly CssPropertyInfo BorderBottomColor		= new CssPropertyInfo("border-bottom-color");
		public static readonly CssPropertyInfo BorderBottomLeftRadius	= new CssPropertyInfo("border-bottom-left-radius");
		public static readonly CssPropertyInfo BorderBottomRightRadius	= new CssPropertyInfo("border-bottom-right-radius");
		public static readonly CssPropertyInfo BorderBottomStyle		= new CssPropertyInfo("border-bottom-style");
		public static readonly CssPropertyInfo BorderBottomWidth		= new CssPropertyInfo("border-bottom-width");
		public static readonly CssPropertyInfo BorderCollapse			= new CssPropertyInfo("border-collapse");
		public static readonly CssPropertyInfo BorderColor				= new CssPropertyInfo("border-color");
		public static readonly CssPropertyInfo BorderImage				= new CssPropertyInfo("border-image",			BorderImageCompatibility);
		public static readonly CssPropertyInfo BorderImageOutset		= new CssPropertyInfo("border-image-outset",	BorderImageCompatibility);
		public static readonly CssPropertyInfo BorderImageRepeat		= new CssPropertyInfo("border-image-repeat",	BorderImageCompatibility);
		public static readonly CssPropertyInfo BorderImageSlice			= new CssPropertyInfo("border-image-slice",		BorderImageCompatibility);
		public static readonly CssPropertyInfo BorderImageSource		= new CssPropertyInfo("border-image-source",	BorderImageCompatibility);
		public static readonly CssPropertyInfo BorderImageWidth			= new CssPropertyInfo("border-image-width",		BorderImageCompatibility);

		public static readonly CssPropertyInfo BorderLeft				= new CssPropertyInfo("border-left",			CssModule.Core1);
		public static readonly CssPropertyInfo BorderLeftColor			= new CssPropertyInfo("border-left-color");
		public static readonly CssPropertyInfo BorderLeftStyle			= new CssPropertyInfo("border-left-style");
		public static readonly CssPropertyInfo BorderLeftWidth			= new CssPropertyInfo("border-left-width");
		public static readonly CssPropertyInfo BorderRadius				= new CssPropertyInfo("border-radius",			CssModule.BackgroundsAndBorders3);
		public static readonly CssPropertyInfo BorderRight				= new CssPropertyInfo("border-right",			CssModule.Core1);
		public static readonly CssPropertyInfo BorderRightColor			= new CssPropertyInfo("border-right-color");
		public static readonly CssPropertyInfo BorderRightStyle			= new CssPropertyInfo("border-right-style");
		public static readonly CssPropertyInfo BorderRightWidth			= new CssPropertyInfo("border-right-width");
		public static readonly CssPropertyInfo BorderSpacing			= new CssPropertyInfo("border-spacing");
		public static readonly CssPropertyInfo BorderStyle				= new CssPropertyInfo("border-style",			CssModule.Core1);
		public static readonly CssPropertyInfo BorderTop				= new CssPropertyInfo("border-top",				CssModule.Core1);
		public static readonly CssPropertyInfo BorderTopColor			= new CssPropertyInfo("border-top-color");
		public static readonly CssPropertyInfo BorderTopLeftRadius		= new CssPropertyInfo("border-top-left-radius");
		public static readonly CssPropertyInfo BorderTopRightRadius		= new CssPropertyInfo("border-top-right-radius");
		public static readonly CssPropertyInfo BorderTopStyle			= new CssPropertyInfo("border-top-style");
		public static readonly CssPropertyInfo BorderTopWidth			= new CssPropertyInfo("border-top-width",		CssModule.Core1);
		public static readonly CssPropertyInfo BorderWidth				= new CssPropertyInfo("border-width",			CssModule.Core1);

		public static readonly CssPropertyInfo Bottom					= new CssPropertyInfo("bottom");
		public static readonly CssPropertyInfo BoxDecorationBreak		= new CssPropertyInfo("box-decoration-break");

		public static readonly CssPropertyInfo BoxShadow = new CssPropertyInfo("box-shadow", CssModule.UI(3), new Compatibility {
			Prefixed = new[] { Browser.Chrome1, Browser.Firefox(3.5f), Browser.Safari(3.1f) },
			Standard = new[] { Browser.Chrome10, Browser.IE9, Browser.Opera(10.5f), Browser.Safari(5.1f) }
		});

		public static readonly CssPropertyInfo BoxSizing = new CssPropertyInfo("box-sizing", CssModule.UI(3), new Compatibility { 
			Prefixed = new[] { Browser.Chrome1, Browser.Firefox1, Browser.IE8, Browser.Safari3 },
			Standard = new[] { Browser.Chrome10, Browser.IE9, Browser.Opera(7), Browser.Safari(5.1f) }
		});

		// Breaks
		public static readonly CssPropertyInfo BreakAfter	= new CssPropertyInfo("break-after");
		public static readonly CssPropertyInfo BreakBefore	= new CssPropertyInfo("break-before");
		public static readonly CssPropertyInfo BreakInside	= new CssPropertyInfo("break-inside");


		public static readonly CssPropertyInfo CaptionSide = new CssPropertyInfo("caption-side");
		public static readonly CssPropertyInfo Clear = new CssPropertyInfo("clear");
		public static readonly CssPropertyInfo Clip = new CssPropertyInfo("clip");
		public static readonly CssPropertyInfo Color = new CssPropertyInfo("color", CssModule.Core1);

		public static readonly CssPropertyInfo ColumnCount		= new CssPropertyInfo("column-count",		CssModule.Columns3);
		public static readonly CssPropertyInfo ColumnFill		= new CssPropertyInfo("column-fill",		CssModule.Columns3);
		public static readonly CssPropertyInfo ColumnGap		= new CssPropertyInfo("column-gap",			CssModule.Columns3);
		public static readonly CssPropertyInfo ColumnRule		= new CssPropertyInfo("column-rule",		CssModule.Columns3);
		public static readonly CssPropertyInfo ColumnRuleColor	= new CssPropertyInfo("column-rule-color",	CssModule.Columns3);
		public static readonly CssPropertyInfo ColumnRuleStyle	= new CssPropertyInfo("column-rule-style",	CssModule.Columns3);
		public static readonly CssPropertyInfo ColumnRuleWidth	= new CssPropertyInfo("column-rule-width",	CssModule.Columns3);
		public static readonly CssPropertyInfo ColumnSpan		= new CssPropertyInfo("column-span",		CssModule.Columns3);
		public static readonly CssPropertyInfo ColumnWidth		= new CssPropertyInfo("column-width",		CssModule.Columns3);
		public static readonly CssPropertyInfo Columns			= new CssPropertyInfo("columns",			CssModule.Columns3);
		
		public static readonly CssPropertyInfo Content = new CssPropertyInfo("content", new Compatibility {
			Standard = new[] { Browser.Chrome1, Browser.Firefox1, Browser.IE8, Browser.Opera4, Browser.Safari1 }	
		});

		// Counters ---------------------------------------------------------------------------------------
		public static readonly CssPropertyInfo CounterIncrement = new CssPropertyInfo("counter-increment");
		public static readonly CssPropertyInfo CounterReset		= new CssPropertyInfo("counter-reset");

		public static readonly CssPropertyInfo Cursor		= new CssPropertyInfo("cursor");
		public static readonly CssPropertyInfo Direction	= new CssPropertyInfo("direction");
		public static readonly CssPropertyInfo Display		= new CssPropertyInfo("display");
		
		public static readonly CssPropertyInfo EmptyCells	= new CssPropertyInfo("empty-cells");
		public static readonly CssPropertyInfo Filter		= new CssPropertyInfo("filter");

		// Fit ---------------------------------------------------------------------------------------
		public static readonly CssPropertyInfo Fit			= new CssPropertyInfo("fit");
		public static readonly CssPropertyInfo FitPosition	= new CssPropertyInfo("fit-position");

		// Flex ---------------------------------------------------------------------------------------
		public static readonly CssPropertyInfo FlexAlign	= new CssPropertyInfo("flex-align");
		public static readonly CssPropertyInfo FlexFlow		= new CssPropertyInfo("flex-flow");
		public static readonly CssPropertyInfo FlexLinePack	= new CssPropertyInfo("flex-line-pack");
		public static readonly CssPropertyInfo FlexOrder	= new CssPropertyInfo("flex-order");
		public static readonly CssPropertyInfo FlexPack		= new CssPropertyInfo("flex-pack");
		
		public static readonly CssPropertyInfo Float		= new CssPropertyInfo("float",					CssModule.Core1);
		public static readonly CssPropertyInfo FloatOffset	= new CssPropertyInfo("float-offset");

		// Fonts -------------------------------------------------------------------------------------------
		public static readonly CssPropertyInfo Font				= new CssPropertyInfo("font",				CssModule.Core1);
		public static readonly CssPropertyInfo FontFamily		= new CssPropertyInfo("font-family",		CssModule.Core1);
		public static readonly CssPropertyInfo FontSize			= new CssPropertyInfo("font-size",			CssModule.Core1);
		public static readonly CssPropertyInfo FontSizeAdjust	= new CssPropertyInfo("font-size-adjust"		  );
		public static readonly CssPropertyInfo FontStretch		= new CssPropertyInfo("font-stretch",		CssModule.Fonts3); 
		public static readonly CssPropertyInfo FontStyle		= new CssPropertyInfo("font-style",			CssModule.Core1);
		public static readonly CssPropertyInfo FontVariant		= new CssPropertyInfo("font-variant",		CssModule.Core1);
		public static readonly CssPropertyInfo FontWeight		= new CssPropertyInfo("font-weight",		CssModule.Core1);

		// Grids ---------------------------------------------------------------------------------------
		public static readonly Compatibility GridComptability	= new Compatibility { Prefixed = new[] { Browser.IE10 } };

		public static readonly CssPropertyInfo GridColumns			= new CssPropertyInfo("grid-columns",	GridComptability);
		public static readonly CssPropertyInfo GridRows				= new CssPropertyInfo("grid-rows",		GridComptability);

		public static readonly CssPropertyInfo Height				= new CssPropertyInfo("height", CssModule.Core1);

		// Hyphens -------------------------------------------------------------------------------------
		public static readonly CssPropertyInfo HyphenateAfter		= new CssPropertyInfo("hyphenate-after");
		public static readonly CssPropertyInfo HyphenateBefore		= new CssPropertyInfo("hyphenate-before");
		public static readonly CssPropertyInfo HyphenateCharacter	= new CssPropertyInfo("hyphenate-character", CssModule.Text3);
		public static readonly CssPropertyInfo HyphenateLines		= new CssPropertyInfo("hyphenate-lines");
		public static readonly CssPropertyInfo HyphenateResource	= new CssPropertyInfo("hyphenate-resource");

		public static readonly CssPropertyInfo Hyphens				= new CssPropertyInfo("hyphens", new CssModule(CssModuleType.Text, 3), new Compatibility {
																	Prefixed = new[] { Browser.Chrome13, Browser.Firefox6, Browser.IE10, Browser.Safari(5.1f) },
																});

		public static readonly CssPropertyInfo InlineBoxAlign		= new CssPropertyInfo("inline-box-align");
		public static readonly CssPropertyInfo Left					= new CssPropertyInfo("left");
		public static readonly CssPropertyInfo LetterSpacing		= new CssPropertyInfo("letter-spacing");

		// Lines
		public static readonly CssPropertyInfo LineBreak			= new CssPropertyInfo("line-break");
		public static readonly CssPropertyInfo LineHeight			= new CssPropertyInfo("line-height", CssModule.Core1);
		
		// List Styles -------------------------------------------------------------------------------------------------------
		public static readonly CssPropertyInfo ListStyle			= new CssPropertyInfo("list-style",				CssModule.Core1);
		public static readonly CssPropertyInfo ListStyleImage		= new CssPropertyInfo("list-style-image",		CssModule.Core1);
		public static readonly CssPropertyInfo ListStylePosition	= new CssPropertyInfo("list-style-position",	CssModule.Core1);
		public static readonly CssPropertyInfo ListStyleType		= new CssPropertyInfo("list-style-type",		CssModule.Core1);

		// Margins -----------------------------------------------------------------------------------------------------------
		public static readonly CssPropertyInfo Margin				= new CssPropertyInfo("margin",					CssModule.Core1);
		public static readonly CssPropertyInfo MarginBottom			= new CssPropertyInfo("margin-bottom",			CssModule.Core1);
		public static readonly CssPropertyInfo MarginLeft			= new CssPropertyInfo("margin-left",			CssModule.Core1);
		public static readonly CssPropertyInfo MarginRight			= new CssPropertyInfo("margin-right",			CssModule.Core1);
		public static readonly CssPropertyInfo MarginTop			= new CssPropertyInfo("margin-top",				CssModule.Core1);

		// Marquee -----------------------------------------------------------------------------------------
		public static readonly CssPropertyInfo MarqueeDirection	= new CssPropertyInfo("marquee-direction");
		public static readonly CssPropertyInfo MarqueeLoop		= new CssPropertyInfo("marquee-loop");
		public static readonly CssPropertyInfo MarqueePlayCount = new CssPropertyInfo("marquee-play-count");
		public static readonly CssPropertyInfo MarqueeSpeed		= new CssPropertyInfo("marquee-speed");
		public static readonly CssPropertyInfo MarqueeStyle		= new CssPropertyInfo("marquee-style");

		public static readonly CssPropertyInfo MaxHeight		= new CssPropertyInfo("max-height",		CssModule.Core21);
		public static readonly CssPropertyInfo MaxWidth			= new CssPropertyInfo("max-width",		CssModule.Core21);

		public static readonly CssPropertyInfo MinHeight		= new CssPropertyInfo("min-height",		CssModule.Core21);
		public static readonly CssPropertyInfo MinWidth			= new CssPropertyInfo("min-width",		CssModule.Core21);

		// <= IE8 filter: alpha(opacity=xx)
		// IE8 introduced -ms-filter, which is synonymous with filter. Both are gone in IE10
		public static readonly CssPropertyInfo Opacity = new CssPropertyInfo("opacity", new Compatibility {
			Standard = new[] { Browser.Chrome1, Browser.Firefox1, Browser.IE9, Browser.Opera9, Browser.Safari(1.2f) } 
		});

		public static readonly CssPropertyInfo Orphans = new CssPropertyInfo("orphans", new CssModule(CssModuleType.Core, 2.1f), new Compatibility {
			Standard = new[] { Browser.IE8, Browser.Opera(9.2f) }	
		});

		// Outlines -------------------------------------------------------------------------------
		public static readonly CssPropertyInfo Outline			= new CssPropertyInfo("outline",		CssModule.Core21);
		public static readonly CssPropertyInfo OutlineColor		= new CssPropertyInfo("outline-color",	CssModule.Core21);
		public static readonly CssPropertyInfo OutlineOffset	= new CssPropertyInfo("outline-offset", CssModule.Core21);
		public static readonly CssPropertyInfo OutlineStyle		= new CssPropertyInfo("outline-style",	CssModule.Core21);
		public static readonly CssPropertyInfo OutlineWidth		= new CssPropertyInfo("outline-width",	CssModule.Core21);

		// Overflow -------------------------------------------------------------------------------
		public static readonly CssPropertyInfo Overflow			= new CssPropertyInfo("overflow",		CssModule.Core21);
		public static readonly CssPropertyInfo OverflowStyle	= new CssPropertyInfo("overflow-style");
		public static readonly CssPropertyInfo OverflowWrap		= new CssPropertyInfo("overflow-wrap");
		public static readonly CssPropertyInfo OverflowX		= new CssPropertyInfo("overflow-x",		CssModule.Core21);
		public static readonly CssPropertyInfo OverflowY		= new CssPropertyInfo("overflow-y",		CssModule.Core21);

		// Padding -----------------------------------------------------------------------------------------------
		public static readonly CssPropertyInfo Padding			= new CssPropertyInfo("padding",		CssModule.Core1);
		public static readonly CssPropertyInfo PaddingBottom	= new CssPropertyInfo("padding-bottom", CssModule.Core1);
		public static readonly CssPropertyInfo PaddingLeft		= new CssPropertyInfo("padding-left",	CssModule.Core1);
		public static readonly CssPropertyInfo PaddingRight		= new CssPropertyInfo("padding-right",	CssModule.Core1);
		public static readonly CssPropertyInfo PaddingTop		= new CssPropertyInfo("padding-top",	CssModule.Core1);
		
		public static readonly CssPropertyInfo Page				= new CssPropertyInfo("page");

		// Page Breaks -------------------------------------------------------------------------------
		public static readonly CssPropertyInfo PageBreakAfter	= new CssPropertyInfo("page-break-after");
		public static readonly CssPropertyInfo PageBreakBefore	= new CssPropertyInfo("page-break-before");
		public static readonly CssPropertyInfo PageBreakInside	= new CssPropertyInfo("page-break-inside");

		// Perspective
		public static readonly CssPropertyInfo Perspective			= new CssPropertyInfo("perspective",			CssModule.Transforms3);
		public static readonly CssPropertyInfo PerspectiveOrigin	= new CssPropertyInfo("perspective-origin",		CssModule.Transforms3);
		public static readonly CssPropertyInfo PerspectiveOriginX	= new CssPropertyInfo("perspective-origin-x",	CssModule.Transforms3);
		public static readonly CssPropertyInfo PerspectiveOriginY	= new CssPropertyInfo("perspective-origin-y",	CssModule.Transforms3);

		public static readonly CssPropertyInfo Position = new CssPropertyInfo("position");
		public static readonly CssPropertyInfo Quotes = new CssPropertyInfo("quotes");
		public static readonly CssPropertyInfo Resize = new CssPropertyInfo("resize");
		public static readonly CssPropertyInfo Right = new CssPropertyInfo("right");

		// Ruby (Level 3) ------------------------------------------------------------------------------------

		public static readonly CssModule RubyLevel3					= new CssModule(CssModuleType.Ruby, 3);

		public static readonly CssPropertyInfo RubyAlign				= new CssPropertyInfo("ruby-align",		RubyLevel3);
		public static readonly CssPropertyInfo RubyOverhang				= new CssPropertyInfo("ruby-overhang",	RubyLevel3);
		public static readonly CssPropertyInfo RubyPosition				= new CssPropertyInfo("ruby-position",	RubyLevel3);
		public static readonly CssPropertyInfo RubySpan					= new CssPropertyInfo("ruby-span",		RubyLevel3);

		public static readonly CssPropertyInfo Size						= new CssPropertyInfo("size");
		public static readonly CssPropertyInfo Speak					= new CssPropertyInfo("speak");

		public static readonly CssPropertyInfo TableLayout				= new CssPropertyInfo("table-layout", CssModule.Core21);

		// Text ------------------------------------------------------------------------
		public static readonly CssPropertyInfo TextAlign				= new CssPropertyInfo("text-align", CssModule.Core1);
		public static readonly CssPropertyInfo TextAlignLast			= new CssPropertyInfo("text-align-last");
		public static readonly CssPropertyInfo TextDecoration			= new CssPropertyInfo("text-decoration");
		public static readonly CssPropertyInfo TextDecorationColor		= new CssPropertyInfo("text-decoration-color");
		public static readonly CssPropertyInfo TextDecorationLine		= new CssPropertyInfo("text-decoration-line");
		public static readonly CssPropertyInfo TextDecorationSkip		= new CssPropertyInfo("text-decoration-skip");
		public static readonly CssPropertyInfo TextDecorationStyle		= new CssPropertyInfo("text-decoration-style");
		public static readonly CssPropertyInfo TextEmphasis				= new CssPropertyInfo("text-emphasis");
		public static readonly CssPropertyInfo TextEmphasisColor		= new CssPropertyInfo("text-emphasis-color");
		public static readonly CssPropertyInfo TextEmphasisPosition		= new CssPropertyInfo("text-emphasis-position");
		public static readonly CssPropertyInfo TextEmphasisStyle		= new CssPropertyInfo("text-emphasis-style");
		public static readonly CssPropertyInfo TextHeight				= new CssPropertyInfo("text-height");
		public static readonly CssPropertyInfo TextIndent				= new CssPropertyInfo("text-indent", CssModule.Core1);
		public static readonly CssPropertyInfo TextJustify				= new CssPropertyInfo("text-justify");
		public static readonly CssPropertyInfo TextOutline				= new CssPropertyInfo("text-outline");
		public static readonly CssPropertyInfo TextShadow				= new CssPropertyInfo("text-shadow", new Compatibility { Standard = new[] { Browser.Chrome(2), Browser.Firefox(3.5f), Browser.IE10, Browser.Safari(1.1f) } });
		public static readonly CssPropertyInfo TextSpaceCollapse		= new CssPropertyInfo("text-space-collapse");
		public static readonly CssPropertyInfo TextTransform			= new CssPropertyInfo("text-transform", CssModule.Core1);
		public static readonly CssPropertyInfo TextUnderlinePosition	= new CssPropertyInfo("text-underline-position");
		public static readonly CssPropertyInfo TextWrap					= new CssPropertyInfo("text-wrap");

		public static readonly CssPropertyInfo Top						= new CssPropertyInfo("top");

		// Transforms (Level 3) ------------------------------------------------------------------------
		public static readonly CssPropertyInfo Transform		= new CssPropertyInfo("transform",			CssModule.Transforms3);
		public static readonly CssPropertyInfo TransformOrigin	= new CssPropertyInfo("transform-origin",	CssModule.Transforms3);
		public static readonly CssPropertyInfo TransformStyle	= new CssPropertyInfo("transform-style",	CssModule.Transforms3);

		// - Transitions (Level 3 ) ------------------------------------------------------------------------------------------------

		public static readonly CssPropertyInfo Transition				= new CssPropertyInfo("transition",					CssModule.Transitions3);
		public static readonly CssPropertyInfo TransitionDelay			= new CssPropertyInfo("transition-delay",			CssModule.Transitions3);
		public static readonly CssPropertyInfo TransitionDuration		= new CssPropertyInfo("transition-duration",		CssModule.Transitions3);
		public static readonly CssPropertyInfo TransitionProperty		= new CssPropertyInfo("transition-property",		CssModule.Transitions3);
		public static readonly CssPropertyInfo TransitionTimingFunction = new CssPropertyInfo("transition-timing-function", CssModule.Transitions3);

		// - Unicode -------------------------------------------------------------------------
		public static readonly CssPropertyInfo UnicodeBidi		= new CssPropertyInfo("unicode-bidi");
		public static readonly CssPropertyInfo UnicodeRange		= new CssPropertyInfo("unicode-range");
		
		public static readonly CssPropertyInfo UserSelect = new CssPropertyInfo("user-select", new Compatibility {
			Prefixed = new[] { Browser.Chrome1, Browser.Firefox1, Browser.IE10, Browser.Safari3 }	
		});

		public static readonly CssPropertyInfo VerticalAlign	= new CssPropertyInfo("vertical-align", CssModule.Core1);
		public static readonly CssPropertyInfo Visibility		= new CssPropertyInfo("visibility");
		public static readonly CssPropertyInfo WhiteSpace		= new CssPropertyInfo("white-space");
		public static readonly CssPropertyInfo Widows			= new CssPropertyInfo("widows",			CssModule.Core21);
		public static readonly CssPropertyInfo Width			= new CssPropertyInfo("width",			CssModule.Core1);

		// Words
		public static readonly CssPropertyInfo WordBreak		= new CssPropertyInfo("word-break",		CssModule.Text3);
		public static readonly CssPropertyInfo WordSpacing		= new CssPropertyInfo("word-spacing",	CssModule.Core1);
		public static readonly CssPropertyInfo WordWrap			= new CssPropertyInfo("word-wrap",		CssModule.Text3);
		
		public static readonly CssPropertyInfo ZIndex			= new CssPropertyInfo("z-index");



		public static readonly IDictionary<string, CssPropertyInfo> Map = new Dictionary<string, CssPropertyInfo> {
			{ "animation", Animation },
			{ "animation-delay", AnimationDelay },
			{ "animation-direction", AnimationDirection },
			{ "animation-duration", AnimationDuration },
			{ "animation-iteration-count", AnimationIterationCount },
			{ "animation-name", AnimationName },
			{ "animation-play-state", AnimationPlayState },
			{ "animation-timing-function", AnimationTimingFunction },
			{ "appearance", Appearance },
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

