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
		private readonly VariableBag variables = new VariableBag();
		private readonly List<CssRule> rules = new List<CssRule>();

		public List<CssRule> Rules
		{
			get { return rules; }
		}

		public VariableBag Variables
		{
			get { return variables; }
		}

		public void SetCompatibility(params Browser[] browsers)
		{
			foreach (var rule in rules)
			{
				rule.Expand();
			}
		}

		public void EvaluateVariables(bool removeRootRule = true)
		{
			CssRule root = null;

			foreach (var rule in rules)
			{
				if (rule.Selector.Text == ":root")
				{
					root = rule;
				}

				foreach (var d in rule.Declarations)
				{
					foreach(var value in d.Value)
					{
						if (value.Type == CssValueType.Variable)
						{
							var varName = value.ToString().Substring(1);

							((CssPrimitiveValue)value).SetText(this.variables.Get(varName).ToString());
						}

						// System.Console.WriteLine("Variable:" + d.Property.Name + ":" + d.Value.ToString());

						// sheet.Variables.Set(d.Property.Name.Replace("var-", ""), d.Value.ToString());
					}
				}
			}

			if (removeRootRule && root != null)
			{
				rules.Remove(root);
			}
		}

		public static StyleSheet Parse(string text, bool gatherVaribles = true)
		{
			var sheet = new StyleSheet();

			var parser = new CssParser(text);

			int i = 0;

			foreach (var rule in parser.ReadRules())
			{
				// Gather variables in the :root { } selector
				// http://dev.w3.org/csswg/css-variables/

				if (rule.Selector.Text == ":root")
				{
					foreach (var d in rule.Declarations)
					{
						if (d.Name.StartsWith("var-"))
						{
							sheet.Variables.Set(d.Name.Substring(4), d.Value);
						}
					}
				}

				i++;

				sheet.Rules.Add(rule);
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

				rule.WriteTo(writer);

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
