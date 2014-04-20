namespace Carbon.Css
{
	using System;
	using System.Collections.Generic;
	using System.IO;

	// A StyleRule rule has a selector, and one or more declarations

	public class CssRule : CssBlock, INode
	{
		private readonly RuleType type;
		private readonly ICssSelector selector;

		public CssRule(RuleType type, ICssSelector selector)
		{
			this.type = type;
			this.selector = selector;
		}

		public RuleType Type
		{
			get { return type; }
		}

		public ICssSelector Selector
		{
			get { return selector; }
		}

		#region Children

		private readonly List<CssRule> children = new List<CssRule>();

		// Nested rules
		// { to: { opacity: 1 } }
		public new List<CssRule> Children
		{
			get { return children; }
		}

		#endregion

		#region Helpers

		public void Expand()
		{
			new DefaultRuleTransformer().Transform(this);
		}

		#endregion


		public override string Text
		{
			get { return ToString(); }
		}

		public virtual void WriteTo(TextWriter writer, int level = 0, CssContext context = null)
		{
			// Indent two characters for each level
			for (int i = 0; i < level; i++)
			{
				writer.Write("  ");
			}

			writer.Write(Selector.ToString() + " ");

			// Block Start	
			writer.Write("{");

			// Write the declarations
			foreach (var declaration in declarations)
			{
				if (declarations.Count > 1)
				{
					writer.WriteLine();

					// Indent two characters for each level
					for (int i = 0; i < level; i++)
					{
						writer.Write("  ");
					}

					writer.Write(" ");
				}

			
				var value = declaration.Value;

				if (value.Kind == NodeKind.Identifier)
				{
					var varName = value.Text;

					writer.Write(string.Format(" {0}: {1};", declaration.Name, context.Variables.Get(varName).ToString()));
				}
				else
				{
					writer.Write(string.Format(" {0}: {1};", declaration.Name, value.Text));
				}

				if (declarations.Count == 1)
				{
					writer.Write(" ");
				}
			}

			if (declarations.Count > 1)
			{
				writer.WriteLine();
			}

			// Write the nested rules
			foreach (var b in this.Children)
			{
				writer.WriteLine();

				b.WriteTo(writer, level + 1);
			}

			if (this.Children.Count > 0)
			{
				writer.WriteLine();
			}

			// Block End
			writer.Write("}");
		}

		public override string ToString()
		{
			using (var writer = new StringWriter())
			{
				WriteTo(writer);

				return writer.ToString();
			}
		}

		#region INode

		NodeKind INode.Kind
		{
			get { return NodeKind.Rule;  }
		}

		#endregion
	}
}