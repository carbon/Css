using System;

namespace Carbon.Css
{
    public static class CssUnitNames
    {
        public static readonly string Lh      = "lh";
        public static readonly string S       = "s";
        public static readonly string Ms      = "ms";
        public static readonly string Px      = "px";
        public static readonly string Em      = "em";
        public static readonly string Vw      = "vw";
        public static readonly string Vh      = "vh";
        public static readonly string Ex      = "ex";
        public static readonly string Deg     = "deg";
        public static readonly string Rem     = "rem";
        public static readonly string Percent = "%";
        public static readonly string Vmin    = "vmin";
        public static readonly string Vmax    = "vmax";
        public static readonly string X       = "x";
        public static readonly string Vi      = "vi";
        public static readonly string Hz      = "Hz";
        public static readonly string Grad    = "grad";
        public static readonly string Rad     = "rad";
        public static readonly string Rlh     = "rlh";
        public static readonly string Turn    = "turn";

        public static string Get(ReadOnlySpan<char> text)
        {
            if (text.Length is 1)
            {
                switch (text[0])
                {
                    case '%': return Percent;
                    case 's': return S;
                    case 'x': return X;
                }
            }
            else if (text.Length is 2)
            {
                switch (text[0])
                {
                    case 'p' when (text[1] is 'x'): return Px;
                    case 'e' when (text[1] is 'm'): return Em;
                    case 'H' when (text[1] is 'z'): return Hz;
                    case 'm' when (text[1] is 's'): return Ms;
                    case 'v':
                        switch (text[1])
                        {
                            case 'w': return Vw;
                            case 'h': return Vh;
                        }
                        break;                   
                }
            }

            else if (text.Length is 3)
            {
                if (text[0] == 'd' && text[1] == 'e' && text[2] == 'g')
                {
                    return Deg;
                }
                else if (text[0] == 'r')
                {
                    if (text[1] == 'e' && text[2] == 'm')
                    {
                        return Rem;
                    }
                    else if (text[1] == 'l' && text[2] == 'h')
                    {
                        return Rlh;
                    }
                }
            }
            else if (text.Length is 4)
            {
                if (text[0] is 'v' && text[1] is 'm')
                {
                    if (text[2] == 'a' && text[3] == 'x')
                    {
                        return Vmax;
                    }
                    else if (text[2] == 'i' && text[3] == 'n')
                    {
                        return Vmin;
                    }
                }
            }

            return text.ToString();
        }
    }
}