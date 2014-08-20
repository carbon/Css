namespace Carbon.Css
{
	using System.IO;
	using System.Text;

	public class ImportRule : CssRule
	{
		public ImportRule()
			: base(RuleType.Import) { }

		public CssUrlValue Url { get; set; }

		public override string Text
		{
			get
			{
				var sb = new StringBuilder();

				new CssWriter(new StringWriter(sb), new CssContext()).WriteImportRule(this);

				return sb.ToString();
			}
		}

		public override string ToString()
		{
			return Text;
		}

	}

	/*
	6.3 The @import rule

	The '@import' rule allows users to import style rules from other style sheets. 
	In CSS 2.1, any @import rules must precede all other rules (except the @charset rule, if present).
	See the section on parsing for when user agents must ignore @import rules. 
	The '@import' keyword must be followed by the URI of the style sheet to include. 
	A string is also allowed; it will be interpreted as if it had url(...) around it.

	The following lines are equivalent in meaning and illustrate both '@import' syntaxes (one with "url()" and one with a bare string):

	@import "mystyle.css";
	@import url("mystyle.css");
	*/
}
