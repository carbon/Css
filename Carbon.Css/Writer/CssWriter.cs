namespace Carbon.Css
{
	using Carbon.Css.Color;
	using Carbon.Css.Parser;
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	public class CssWriter
	{
		private readonly TextWriter writer;
		private readonly CssContext context;
		private readonly ICssResolver resolver;

		public CssWriter(TextWriter writer, CssContext context = null, ICssResolver resolver = null)
		{
			this.writer = writer;
			this.context = context ?? new CssContext();
			this.resolver = resolver;
		}

		int includeCount = 0;

		public void WriteRoot(StyleSheet sheet)
		{
			includeCount++;

			if (includeCount > 100) throw new Exception("Exceded includeLimit of 100");

			var i = 0;

			foreach (var child in sheet.Children)
			{
				var rule = child as CssRule;

				if (rule == null) continue;

				if (i != 0) writer.WriteLine();

				i++;

				if (rule.Type == RuleType.Import)
				{
					var importRule = (ImportRule)rule;

					if (!importRule.Url.IsPath || resolver == null)
					{
						WriteImportRule(importRule);
					}
					else
					{
						InlineImport(importRule, sheet);
					}
				}
				else
				{
					WriteRule(rule);
				}
			}
		}


		public void InlineImport(ImportRule importRule, StyleSheet sheet)
		{
			// var relativePath = importRule.Url;
			var absolutePath = importRule.Url.GetAbsolutePath(resolver.ScopedPath);

			// Assume to be scss if there is no extension
			if (!absolutePath.Contains('.')) absolutePath += ".scss";

			writer.Write(Environment.NewLine + "/* " + absolutePath.TrimStart('/') + " */" + Environment.NewLine);

			var text = resolver.GetText(absolutePath.TrimStart('/'));

			if (text != null)
			{
				if (Path.GetExtension(absolutePath) == ".scss")
				{
					try
					{
						var css = StyleSheet.Parse(text, context);

						css.ExecuteRewriters();

						WriteRoot(css);
					}
					catch (ParseException ex)
					{
						// response.StatusCode = 500;

						writer.WriteLine("body, html { background-color: red !important; }");
						writer.WriteLine("body * { display: none; }");

						writer.WriteLine(string.Format("/* --- Parse Error in '{0}':{1} ", absolutePath, ex.Message));

						if (ex.Lines != null)
						{
							foreach (var line in ex.Lines)
							{
								writer.Write(string.Format("{0}. ", line.Number.ToString().PadLeft(5)));

								if (line.Number == ex.Location.Line)
								{
									writer.Write("* ");
								}

								writer.WriteLine(line.Text);
							}
						}

						writer.Write("*/");

						return;
					}
				}
				else
				{
					writer.Write(text);
				}
			}
			else
			{
				writer.Write("/* NOT FOUND */" + Environment.NewLine);
			}
		}
		public void WriteValue(CssNode value)
		{
			switch(value.Kind)
			{
				case NodeKind.Variable	: WriteVariable((CssVariable)value);	break;
				case NodeKind.ValueList	: WriteValueList((CssValueList)value);	break;
				case NodeKind.Function	: WriteFunction((CssFunction)value);	break;
				default					: writer.Write(value.Text);				break;
			}
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

				WriteValue(value);

				i++;
			}
		}

		public void WriteFunction(CssFunction function)
		{
			// {name}({args})

			// If rgba & args = 2

			if (function.Name == "rgba")
			{
				var args = GetArgs(function.Args).ToArray();

				if (args.Length == 2 && args[0].ToString().StartsWith("#"))
				{
					var colorText = args[0].ToString();

					var color = WebColor.Parse(colorText);

					writer.Write(string.Format("rgba({0}, {1}, {2}, {3})", color.R, color.G, color.B, args[1].ToString()));

					return;
				}
			}

			writer.Write(function.Name);

			writer.Write("(");

			WriteValue(function.Args);
			
			writer.Write(")");
		}

		public IEnumerable<CssValue> GetArgs(CssValue value)
		{
			switch(value.Kind)
			{
				case NodeKind.Variable:
					var x = (CssVariable)value;

					yield return context.GetVariable(x.Symbol);	
					
					break;
				case NodeKind.ValueList:
					{
						var list = (CssValueList)value;

						if (list.Seperator == ValueListSeperator.Space) yield return list;

						// Break out comma seperated values
						foreach (var v in list)
						{
							foreach (var item in GetArgs((CssValue)v)) yield return item;
						}
					}
					
					break;
				case NodeKind.Function	: yield return value;	break;
				default					: yield return value;	break;
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

		public void WriteRule(CssRule rule, int level = 0)
		{
			Indent(level);

			switch (rule.Type)
			{
				case RuleType.Import	: WriteImportRule((ImportRule)rule);				break;
				case RuleType.Media		: WriteMediaRule((MediaRule)rule, level);			break;
				case RuleType.Style		: WriteStyleRule((StyleRule)rule, level);			break;
				case RuleType.FontFace	: WriteFontFaceRule((FontFaceRule)rule, level);		break;
				case RuleType.Keyframes	: WriteKeyframesRule((KeyframesRule)rule, level);	break;

				// Unknown rules
				default:
					if (rule is AtRule) WriteAtRule((AtRule)rule, level);
					
					break;
			}
		}

		public void WriteAtRule(AtRule rule, int level)
		{
			writer.Write("@" + rule.AtName);

			if (rule.SelectorText != null)
			{
				writer.Write(" " + rule.SelectorText);
			}

			writer.Write(" ");

			WriteBlock(rule, level);
		}

		public void WriteStyleRule(StyleRule rule, int level)
		{
			writer.Write(rule.Selector.ToString() + " "); // Write selector

			WriteBlock(rule, level);
		}

		public void WriteMediaRule(MediaRule rule, int level)
		{
			writer.Write("@media {0} ", rule.RuleText); // Write selector

			WriteBlock(rule, level);
		}

		public void WriteFontFaceRule(FontFaceRule rule, int level)
		{
			writer.Write("@font-face "); // Selector

			WriteBlock(rule, level);
		}

		public void WriteKeyframesRule(KeyframesRule rule, int level)
		{
			writer.Write("@keyframes {0} ", rule.Name); // Write selector

			WriteBlock(rule, level);
		}

		public void WriteBlock(CssBlock block, int level)
		{
			writer.Write("{"); // Block Start	

			var condenced = false;
			var count = 0;

			// Write the declarations
			foreach (var node in block.Children) // TODO: Change to an immutable list?
			{
				if (node.Kind == NodeKind.Include) continue;

				if (node.Kind == NodeKind.Declaration)
				{
					var declaration = (CssDeclaration)node;

					if (block.Children.Count == 1)
					{
						condenced = true;
					}
					else
					{
						if (count == 0) writer.WriteLine();

						Indent(level);

						writer.Write(" ");
					}

					writer.Write(" ");
					writer.Write(declaration.Name);
					writer.Write(": ");

					WriteValue(declaration.Value);

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

			else
			{
				Indent(level);
			}

			writer.Write("}"); // Block end
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
	}

	public enum WriterStyle
	{
		Pretty = 1,  
		OneRulePerLine = 2
	}
}
