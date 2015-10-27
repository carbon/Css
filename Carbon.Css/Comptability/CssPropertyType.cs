namespace Carbon.Css
{
    public enum CssPropertyType
    {
        Unknown = 0,

        // Animations (Level 3) -----------
        Animation,
        AnimationDelay,
        AnimationDirection,
        AnimationDuration,
        AnimationIterationCount,
        AnimationName,
        AnimationPlayState,
        AnimationTimingFunction,
        // --------------------------------

        Appearance,

        BackfaceVisibility,

        // Backgrounds -----------
        Background,
        BackgroundAttachment,
        BackgroundClip,
        BackgroundColor,
        BackgroundImage,
        BackgroundOrigin,
        BackgroundPosition,
        BackgroundRepeat,
        BackgroundSize,

        // Borders -----------
        Border,
        BorderBottom,
        BorderBottomColor,
        BorderBottomLeftRadius,
        BorderBottomRightRadius,
        BorderBottomStyle,
        BorderBottomWidth,
        BorderCollapse,
        BorderColor,
        BorderImage,
        BorderImageOutset,
        BorderImageRepeat,
        BorderImageSlice,
        BorderImageSource,
        BorderImageWidth,

        BorderLeft,
        BorderLeftColor,
        BorderLeftStyle,
        BorderLeftWidth,
        BorderRadius,
        BorderRight,
        BorderRightColor,
        BorderRightStyle,
        BorderRightWidth,
        BorderSpacing,
        BorderStyle,
        BorderTop,
        BorderTopColor,
        BorderTopLeftRadius,
        BorderTopRightRadius,
        BorderTopStyle,
        BorderTopWidth,
        BorderWidth,

        Bottom,
        BoxDecorationBreak,

        BoxShadow,

        BoxSizing,

        // Breaks
        BreakAfter,
        BreakBefore,
        BreakInside,


        CaptionSide,
        Clear,

        Clip,

        // TODO: Confirm support
        ClipPath,

        Color,

        ColumnCount,
        ColumnFill,
        ColumnGap,
        ColumnRule,
        ColumnRuleColor,
        ColumnRuleStyle,
        ColumnRuleWidth,
        ColumnSpan,
        ColumnWidth,
        Columns,

        Content,

        // Counters ----------------------------------------------------
        CounterIncrement,
        CounterReset,

        Cursor,
        Direction,
        Display,

        EmptyCells,
        Filter,

        // Fit ---------------------------------------------------------
        Fit,
        FitPosition,

        // Flex --------------------------------------------------------
        FlexAlign,
        FlexFlow,
        FlexLinePack,
        FlexOrder,
        FlexPack,

        Float,
        FloatOffset,

        // Fonts -------------------------------------------------------
        Font,
        FontFamily,
        FontSize,
        FontSizeAdjust,
        FontStretch,
        FontStyle,
        FontVariant,
        FontWeight,

        // Grids -------------------------------------------------------
        GridColumns,
        GridRows,

        Height,

        // Hyphens -----------------------------------------------------
        HyphenateAfter,
        HyphenateBefore,
        HyphenateCharacter,
        HyphenateLines,
        HyphenateResource,

        Hyphens,

        InlineBoxAlign,
        Left,
        LetterSpacing,

        // Lines
        LineBreak,
        LineHeight,

        // List Styles -------------------------------------------------
        ListStyle,
        ListStyleImage,
        ListStylePosition,
        ListStyleType,

        // Margins -----------------------------------------------------
        Margin,
        MarginBottom,
        MarginLeft,
        MarginRight,
        MarginTop,

        // Marquee -----------------------------------------------------
        MarqueeDirection,
        MarqueeLoop,
        MarqueePlayCount,
        MarqueeSpeed,
        MarqueeStyle,

        MaxHeight,
        MaxWidth,

        MinHeight,
        MinWidth,

        Opacity,

        Orphans,

        // Outlines ----------------------------------------------------
        Outline,
        OutlineColor,
        OutlineOffset,
        OutlineStyle,
        OutlineWidth,

        // Overflow ----------------------------------------------------
        Overflow,
        OverflowStyle,
        OverflowWrap,
        OverflowX,
        OverflowY,

        // Padding -----------------------------------------------------
        Padding,
        PaddingBottom,
        PaddingLeft,
        PaddingRight,
        PaddingTop,

        Page,

        // Page Breaks -------------------------------------------------
        PageBreakAfter,
        PageBreakBefore,
        PageBreakInside,

        // Perspective
        Perspective,
        PerspectiveOrigin,
        PerspectiveOriginX,
        PerspectiveOriginY,

        Position,
        Quotes,
        Resize,
        Right,

        // Ruby (Level 3) ----------------------------------------------

        RubyAlign,
        RubyOverhang,
        RubyPosition,
        RubySpan,

        Size,
        Speak,

        TableLayout,

        // Text --------------------------------------------------------
        TextAlign,
        TextAlignLast,
        TextDecoration,
        TextDecorationColor,
        TextDecorationLine,
        TextDecorationSkip,
        TextDecorationStyle,
        TextEmphasis,
        TextEmphasisColor,
        TextEmphasisPosition,
        TextEmphasisStyle,
        TextHeight,
        TextIndent,
        TextJustify,
        TextOutline,
        TextShadow,
        TextSpaceCollapse,
        TextTransform,
        TextUnderlinePosition,
        TextWrap,

        Top,

        // Transforms (Level 3) ----------------------------------------
        Transform,
        TransformOrigin,
        TransformStyle,

        // - Transitions (Level 3 ) ------------------------------------

        Transition,
        TransitionDelay,
        TransitionDuration,
        TransitionProperty,
        TransitionTimingFunction,

        // - Unicode ---------------------------------------------------
        UnicodeBidi,
        UnicodeRange,

        UserSelect,

        VerticalAlign,
        Visibility,
        WhiteSpace,
        Widows,
        Width,

        // Words
        WordBreak,
        WordSpacing,
        WordWrap,

        ZIndex,
    }
}
