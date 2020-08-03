using System;

namespace Carbon.Css
{
    public static class CssUnitNames
    {
        public static readonly string S = "s";
        public static readonly string Ms = "ms";
        public static readonly string Px = "px";
        public static readonly string Em = "em";
        public static readonly string Vw = "vw";
        public static readonly string Vh = "vh";
        public static readonly string Deg = "deg";
        public static readonly string Rem = "rem";
        public static readonly string Percent = "%";
        public static readonly string Vmin = "vmin";
        public static readonly string Vmax = "vmax";
        public static readonly string X = "x";

        public static string Get(ReadOnlySpan<char> text)
        {
            if (text.Length == 1)
            {
                switch (text[0])
                {
                    case '%': return Percent;
                    case 's': return S;
                    case 'x': return X;
                }
            }
            else if (text.Length == 2)
            {
                switch (text[0])
                {
                    case 'p':
                        if (text[1] == 'x') return Px;
                        break;

                    case 'e':
                        if (text[1] == 'm') return Em;
                        break;
                    case 'm':
                        if (text[1] == 's') return Ms;
                        break;

                    case 'v':
                        switch (text[1])
                        {
                            case 'w': return Vw;
                            case 'h': return Vh;
                        }
                        break;                   
                }
            }

            else if (text.Length == 3)
            {
                if (text[0] == 'd' && text[1] == 'e' && text[2] == 'g')
                {
                    return Deg;
                }
                else if (text[0] == 'r' && text[1] == 'e' && text[2] == 'm')
                {
                    return Rem;
                }
            }
            else if (text.Length == 4)
            {
                switch (text[0])
                {
                    case 'v':
                        switch (text[1])
                        {
                            case 'm':
                                if (text[2] == 'a' && text[3] == 'x')
                                {
                                    return Vmax;
                                }
                                else if (text[2] == 'i' && text[3] == 'n')
                                {
                                    return Vmin;
                                }
                                break;
                            default: break;
                        }
                      
                        break;
                    default:
                        break;
                }
               

            }

            return text.ToString();
        }
    }

}