using System;
using System.Linq;
using System.Text;

namespace Carbon.Css
{
    public class CssPadding
    {
        public CssPadding() { }

        public void Parse(string property, string value)
        {
            value = value.Trim();

            if (property == "padding")
            {
                var parts = value.Split(' ');

                for (int i = 0; i < parts.Length; i++)
                {
                    var part = parts[i].Trim();

                    if (parts.Length == 1)
                    {
                        Top = part;
                        Left = part;
                        Bottom = part;
                        Right = part;
                    }
                    else if (parts.Length == 2)
                    {
                        // padding: 10px 20px;
                        // 10px = top & bottom
                        // 20px = left & right

                        switch (i)
                        {
                            case 0: Top = part; Bottom = part; break;
                            case 1: Left = part; Right = part; break;
                        }
                    }
                    else if (parts.Length == 3)
                    {
                        // padding: 10px 3% 20px;
                        // 10px = top
                        // 3%   = left & right
                        // 20px = bottom

                        switch (i)
                        {
                            case 0: Top = part; break;
                            case 1: Left = part; Right = part; break;
                            case 3: Bottom = part; break;
                        }
                    }
                    else
                    {
                        switch (i)
                        {
                            case 0: Top = part; break;
                            case 1: Left = part; break;
                            case 2: Bottom = part; break;
                            case 3: Right = part; break;
                        }
                    }
                }
            }

            else
            {
                var part = value;

                try
                {
                    switch (property.Split('-')[1])
                    {
                        case "top": Top = part; break;
                        case "left": Left = part; break;
                        case "bottom": Bottom = part; break;
                        case "right": Right = part; break;
                    }
                }

                catch (Exception ex)
                {
                    throw new Exception(ex.Message + ":" + property);
                }
            }
        }

        public string Top { get; set; }

        public string Left { get; set; }

        public string Bottom { get; set; }

        public string Right { get; set; }

        public CssDeclaration ToDeclaration()
        {
            var sb = new StringBuilder();

            if (new[] { Top, Left, Bottom, Right }.All(v => v == Top))
            {
                return new CssDeclaration("padding", Top);
            }
            else
            {
                sb.Append(Top ?? "0").Append(" ");
                sb.Append(Left ?? "0").Append(" ");
                sb.Append(Bottom ?? "0").Append(" ");
                sb.Append(Right ?? "0");
            }

            return new CssDeclaration("padding", sb.ToString());
        }
    }
}
