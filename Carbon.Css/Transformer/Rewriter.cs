namespace Carbon.Css
{
	using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

	public class RewriterCollection : Collection<ICssTransformer>
	{
		
	

		public static RewriterCollection Default = new RewriterCollection {
			new IEOpacityTransform(),
			new AddVendorPrefixesTransform()
		};
	}
}
