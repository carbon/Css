using System;

namespace Carbon.Css
{
    public static class CssBoxAlignmentExtensions
    {
        public static string Canonicalize(this CssBoxAlignment alignment, BoxLayoutMode mode)
        {
            if (mode.HasFlag(BoxLayoutMode.Flex))
            {
                switch (alignment)
                {
                    case CssBoxAlignment.Start : return "flex-start";
                    case CssBoxAlignment.End   : return "flex-end";
                }
            }

            return Canonicalize(alignment);
        }

        public static string Canonicalize(this CssBoxAlignment alignment) => alignment switch
        {
            CssBoxAlignment.Start         => "start",
            CssBoxAlignment.End           => "end",
            CssBoxAlignment.Center        => "center",

            CssBoxAlignment.SelfStart     => "self-start",
            CssBoxAlignment.SelfEnd       => "self-end",

            // Baseline
            CssBoxAlignment.Baseline      => "baseline",
            CssBoxAlignment.FirstBaseline => "first baseline",
            CssBoxAlignment.LastBaseline  => "last baseline",

            // Distributed
            CssBoxAlignment.SpaceAround   => "space-around",
            CssBoxAlignment.SpaceBetween  => "space-between",
            CssBoxAlignment.SpaceEvenly   => "space-evenly",
            CssBoxAlignment.Stretch       => "stretch",

            // Overflow
            CssBoxAlignment.SafeCenter    => "safe center",
            CssBoxAlignment.UnsafeCenter  => "unsafe center",

            _ => throw new Exception("Unexpected alignment:" + alignment)
        };
        
        public static CssBoxAlignment Parse(string text) => text switch
        {
            "flex-start"     => CssBoxAlignment.Start,
            "flex-end"       => CssBoxAlignment.End,
            "self-start"     => CssBoxAlignment.SelfStart,
            "self-end"       => CssBoxAlignment.SelfEnd,
            "start"          => CssBoxAlignment.Start,
            "end"            => CssBoxAlignment.End,
            "center"         => CssBoxAlignment.Center,
            "baseline"       => CssBoxAlignment.Baseline,
            "first baseline" => CssBoxAlignment.FirstBaseline,
            "last baseline"  => CssBoxAlignment.LastBaseline,
            "space-around"   => CssBoxAlignment.SpaceAround,
            "space-between"  => CssBoxAlignment.SpaceBetween,
            "space-evenly"   => CssBoxAlignment.SpaceEvenly,
            "stretch"        => CssBoxAlignment.Stretch,
            "safe center"    => CssBoxAlignment.SafeCenter,
            "unsafe center"  => CssBoxAlignment.UnsafeCenter,
            _                => CssBoxAlignment.Unknown
        };
        
        // TODO: left, right, top, bottom
    }
}