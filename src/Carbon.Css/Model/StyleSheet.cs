namespace Carbon.Css
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using Carbon.Css.Parser;
	using System.IO;
	using System;

	public class StyleSheet
	{
		private readonly CssContext context = new CssContext();
		private readonly List<CssRule> rules = new List<CssRule>();

		public List<CssRule> Rules
		{
			get { return rules; }
		}

		public CssContext Context
		{
			get { return context; }
		}

		public void SetCompatibility(params Browser[] browsers)
		{
			foreach (var rule in rules)
			{
				rule.Expand();
			}
		}

		public static StyleSheet Parse(string text, bool gatherVaribles = true)
		{
			var sheet = new StyleSheet();

			var parser = new CssParser(text);

			foreach (var node in parser.ReadNodes())
			{
				if (node.Kind == NodeKind.Variable)
				{
					var variable = (CssVariable)node;

					sheet.Context.Variables.Set(variable.Name, variable.Value);
				}
				else
				{
					sheet.Rules.Add((CssRule)node);
				}
			}

			return sheet;
		}

		public void WriteTo(TextWriter writer)
		{
			var i = 0;

			foreach (var rule in rules)
			{
				if (i != 0)
				{
					writer.WriteLine();
				}

				rule.WriteTo(writer, context: context);

				i++;
			}
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			using (var sw = new StringWriter(sb))
			{
				WriteTo(sw);

				return sb.ToString();
			}
		}
	}
}
