namespace Carbon.Css
{
	using System.Collections.Generic;
	using System.Text;

	using Carbon.Css.Parser;
	using System.IO;
	using System;

	public class StyleSheet : IStylesheet
	{
		private readonly IList<CssRule> rules;
		private readonly CssContext context;

		public StyleSheet(IList<CssRule> rules, CssContext context)
		{
			this.rules = rules;
			this.context = context;
		}

		public IList<CssRule> Rules
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

		public static StyleSheet Parse(string text, CssContext context = null)
		{
			if (context == null)
			{
				context = new CssContext();
			}

			var rules = new List<CssRule>();

			var parser = new CssParser(text);

			foreach (var node in parser.ReadNodes())
			{
				if (node.Kind == NodeKind.Variable)
				{
					var variable = (VariableAssignment)node;

					context.Variables[variable.Name] = variable.Value;
				}
				else if(node.Kind == NodeKind.Mixin)
				{
					var mixin = (MixinNode)node;

					context.Mixins.Add(mixin.Name, mixin);
				}
				else
				{
					rules.Add((CssRule)node);
				}
			}

			return new StyleSheet(rules, context);
		}

		public static StyleSheet FromFile(FileInfo file, CssContext context = null)
		{
			var text = "";

			using (var reader = file.OpenText())
			{
				text = reader.ReadToEnd();
			}

			return Parse(text, context);
		}

		public void Compile(TextWriter writer)
		{
			WriteTo(writer);
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
