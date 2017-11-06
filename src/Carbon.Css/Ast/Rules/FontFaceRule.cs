namespace Carbon.Css
{
    public sealed class FontFaceRule : CssRule
    {
        public override RuleType Type => RuleType.FontFace;

    }

    /*
	Internet Explorer only supports EOT
	Mozilla browsers support OTF and TTF
	Safari and Opera support OTF, TTF and SVG
	Chrome supports TTF and SVG.
	*/

    /*
	"woff"				WOFF (Web Open Font Format)		.woff
	"truetype"			TrueType						.ttf
	"opentype"			OpenType						.ttf, .otf
	"embedded-opentype"	Embedded OpenType				.eot
	"svg"				SVG Font						.svg, .svgz
	*/
}
