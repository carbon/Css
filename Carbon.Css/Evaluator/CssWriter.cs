namespace Carbon.Css
{
	using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

	public class CssWriter
	{
		private readonly TextWriter writer;
		private readonly CssContext context;

		public CssWriter(TextWriter writer, CssContext context)
		{
			this.writer = writer;
			this.context = context;
		}

		public void WriteValueList(CssValueList list)
		{
			var i = 0;

			foreach (var value in list.Children)
			{
				if (i != 0)
				{
					writer.Write(list.Seperator == ValueListSeperator.Space ? " " : ", ");
				}

				if (value.Kind == NodeKind.Variable)
				{
					WriteVariable((CssVariable)value);
				}
				else
				{
					writer.Write(value.Text);
				}

				i++;
			}
		}

		public void WriteVariable(CssVariable variable)
		{
			if (variable.Value == null)
			{
				variable.Value = context.GetVariable(variable.Symbol);
			}


			writer.Write(variable.Value.Text);
		}

		public void WriteImportRule(ImportRule rule)
		{
			// TODO: normalize value
			writer.Write("@import " + rule.Url.ToString() + ';');
		}


		#region Mixins

		// Expand Include Nodes
		public void ExpandInclude(IncludeNode include, CssRule rule)
		{
			var index = rule.Children.IndexOf(include);

			MixinNode mixin;

			if (!context.Mixins.TryGetValue(include.Name, out mixin))
			{
				throw new Exception(string.Format("Mixin '{0}' not registered", include.Name));
			}
			
			var childContext = GetContext(mixin.Parameters, include.Args);

			var i = 0;

			foreach(var node in mixin.Children)
			{
				// Bind variables

				BindVariables(node, childContext);


				rule.Children.Insert(index + i, node);

				i++;
			}
		}

		public void BindVariables(CssNode node, CssContext c)
		{
		

			if (node.Kind == NodeKind.Declaration)
			{
				var declaration = (CssDeclaration)node;

				// TODO: Remove
				// throw new Exception(declaration.Value.Kind.ToString() + ":" + declaration.Value.ToString());

				BindVariables(declaration.Value, c);
			}
			else if (node.Kind == NodeKind.Variable)
			{
				var variable = (CssVariable)node;

				variable.Value = c.GetVariable(variable.Symbol);
			
			}
			else if (node.HasChildren)
			{
				foreach (var n in node.Children)
				{
					BindVariables(n, c);
				}
			}
		}

		public CssContext GetContext(IList<CssParameter> paramaters, CssValue args)
		{
			var list = new List<CssValue>();

			if(args != null)
			{
				var valueList = args as CssValueList;

				if(valueList == null)
				{
					list.Add(args); // Single Value
				}

				if (valueList != null && valueList.Seperator == ValueListSeperator.Comma)
				{
					list.AddRange(valueList.Children.OfType<CssValue>());
				}
			}

			var child = new CssContext(context);

			var i = 0;

			foreach (var p in paramaters)
			{
				var val = (list != null && list.Count >= i + 1) ? list[i] : p.Default;

				child.Variables.Add(p.Name, val);
			
				i++;
			}

			return child;
		}

		#endregion


		public void WriteRule(CssRule rule, int level = 0)
		{
			Indent(level);

			if(rule.Type == RuleType.Import) 
			{
				WriteImportRule((ImportRule)rule);

				return;
			}

			// Write the selector
			writer.Write(rule.Selector.ToString() + " ");

			// Block Start	
			writer.Write("{");

			var copy = rule.Children.ToList();

			// Expand includes
			foreach (var include in rule.Children.OfType<IncludeNode>().ToArray())
			{
				ExpandInclude(include, rule);
			}


			var condenced = false;
			var count = 0;

			// Write the declarations
			foreach (var node in rule.Children) // TODO: Change to an immutable list?
			{
				if (node.Kind == NodeKind.Include) continue;

				if (node.Kind == NodeKind.Declaration)
				{
					var declaration = (CssDeclaration)node;

					if (rule.Children.Count == 1)
					{
						condenced = true;
					}
					else
					{
						if (count == 0) writer.WriteLine();

						Indent(level);

						writer.Write(" ");
					}

					var value = declaration.Value;

					writer.Write(" ");
					writer.Write(declaration.Name);
					writer.Write(": ");

					if (value.Kind == NodeKind.Variable)
					{
						WriteVariable((CssVariable)value);
					}
					else if (value.Kind == NodeKind.ValueList)
					{
						var valueList = (CssValueList)value;

						WriteValueList(valueList);
					}
					else
					{
						writer.Write(value.Text);
					}

					writer.Write(";");

				}
				else if (node.Kind == NodeKind.Rule)  // Nested rule
				{
					if (count == 0) writer.WriteLine();

					var childRule = (CssRule)node;

					WriteRule(childRule, level + 1);
				}

				if (!condenced)
				{
					writer.WriteLine();
				}

				count++;
			}

			// Limit to declaration
			if (condenced)
			{
				writer.Write(" ");
			}

			// Block End
			writer.Write("}");
		}

		#region Helpers

		public void Indent(int level)
		{
			// Indent two characters for each level
			for (int i = 0; i < level; i++)
			{
				writer.Write("  ");
			}
		}

		#endregion

		// WriteUrl

		// WriteBlock();

		// WriteDeclaration();

		// WriteValue(CssValue)

	}
}
