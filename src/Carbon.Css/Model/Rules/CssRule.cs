namespace Carbon.Css
{
	using System.Collections.Generic;
	using System.IO;

	// A StyleRule rule has a selector, and one or more declarations

	public class CssRule : CssBlock, INode
	{
		private readonly RuleType type;
		private readonly CssSelector selector;

		public CssRule(RuleType type, CssSelector selector)
		{
			this.type = type;
			this.selector = selector;
		}

		public RuleType Type
		{
			get { return type; }
		}

		public CssSelector Selector
		{
			get { return selector; }
		}

		#region Children

		private readonly List<CssRule> children = new List<CssRule>();

		// Nested rules
		// { to: { opacity: 1 } }
		public List<CssRule> Children
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

		public virtual void WriteTo(TextWriter writer, int level = 0)
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
			foreach (var d in Declarations)
			{
				if (Declarations.Count > 1)
				{
					writer.WriteLine();

					// Indent two characters for each level
					for (int i = 0; i < level; i++)
					{
						writer.Write("  ");
					}

					writer.Write(" ");
				}

				writer.Write(string.Format(" {0}: {1};", d.Name, d.Value.ToString()));

				if (Declarations.Count == 1)
				{
					writer.Write(" ");
				}
			}

			if (this.Declarations.Count > 1)
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
