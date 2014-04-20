using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carbon.Css.Model.Values
{
	public class FontSrcValue
	{
		public string Url { get; set; }

		public string Format { get; set; }

		public override string ToString()
		{
			return string.Format("url('{0}'} format('{1}')", Url, Format);
		}
	}
}

/*
    src: url('../fonts/cm-billing-webfont.eot?#iefix') format('embedded-opentype'),
         url('../fonts/cm-billing-webfont.woff') format('woff');
*/
