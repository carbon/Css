namespace Carbon.Css
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using Carbon.Css.Parser;

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

		public void EvaluateVariables()
		{
			foreach (var rule in rules)
			{
				foreach (var d in rule.Block.Declarations)
				{
					foreach(var value in d.Value)
					{
						if (value.Type == CssValueType.Variable)
						{
							((CssPrimitiveValue)value).SetText(this.variables.Get(value.ToString().Substring(1)));
						}

						// System.Console.WriteLine("Variable:" + d.Property.Name + ":" + d.Value.ToString());

						// sheet.Variables.Set(d.Property.Name.Replace("var-", ""), d.Value.ToString());
					}
				}
			}
		}

		public static StyleSheet Parse(string text, bool gatherVaribles = true)
		{
			var sheet = new StyleSheet();

			var cssParser = new CssParser(text);

			foreach (var rule in cssParser.ReadRules())
			{
				sheet.Rules.Add(rule);
			}

			// Gather variables
			foreach (var rule in sheet.Rules)
			{
				foreach (var d in rule.Block.Declarations)
				{
					if (d.Property.Name.StartsWith("var-"))
					{
						sheet.Variables.Set(d.Property.Name.Replace("var-", ""), d.Value.ToString());
					}
				}
			}

			return sheet;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			foreach (var rule in rules)
			{
				sb.AppendLine(rule.ToString());
			}

			return sb.ToString();
		}
	}
}
