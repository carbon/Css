using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Carbon.Css
{
	using Parser;

	public class CssWriter
	{
		private readonly TextWriter writer;
		private readonly CssContext context;
		private readonly ICssResolver resolver;
		private int includeCount = 0;
		private int importCount = 0;

		private Browser[] support;

		private CssScope scope;

		public CssWriter(TextWriter writer, CssContext context = null, ICssResolver resolver = null)
		{
			#region Preconditions

			if (writer == null) throw new ArgumentNullException(nameof(writer));

			#endregion

			this.writer = writer;
			this.context = context ?? new CssContext();
			this.resolver = resolver;
			this.scope = this.context.Scope;

			this.support = this.context.BrowserSupport;			
		}

		public void WriteRoot(StyleSheet sheet)
		{
			importCount++;

			if (importCount > 200) throw new Exception("Exceded importCount of 200");

			var i = 0;

			foreach (var child in sheet.Children)
			{
				if (child.Kind == NodeKind.If)
				{
					EvaluateIf((IfBlock)child);

					continue;
				}

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

		#region Expressions

		public void EvaluateIf(IfBlock block, int level = 0)
		{
			var result = EvalulateExpression(block.Condition);

            if (ToBoolean(result))
			{
				foreach (var child in block.Children)
				{
					if (child is CssRule)
					{
						WriteRule((CssRule)child);
					}
					else if (child is CssDeclaration)
					{
						WriteDeclaration((CssDeclaration)child, level);
					}
				}
			}
		}

		public bool ToBoolean(object value)
		{
			if (value is CssBoolean) return ((CssBoolean)value).Value;

			return false;
		}

		public CssValue EvalulateExpression(CssValue expression)
		{
			switch (expression.Kind)
			{
				case NodeKind.Variable   : return scope.GetValue(((CssVariable)expression).Symbol);
				case NodeKind.Expression : return EvalBinaryExpression((BinaryExpression)expression);
				case NodeKind.Function	 : return EvalFunction((CssFunction)expression);
				default					 : return expression;
			}
		}

		public CssValue EvalBinaryExpression(BinaryExpression expression)
		{
			var left = EvalulateExpression(expression.Left);
			var right = EvalulateExpression(expression.Right);
			
			switch (expression.Operator)
			{
				case BinaryOperator.Multiply  : return ((CssMeasurement)expression.Left).Multiply(expression.Right);
				case BinaryOperator.Add       : return ((CssMeasurement)expression.Left).Add(expression.Right);
			}

			var leftS = left.ToString();
			var rightS = right.ToString();

			switch (expression.Operator)
			{
				case BinaryOperator.Equals		: return new CssBoolean(leftS == rightS);
				case BinaryOperator.NotEquals	: return new CssBoolean(leftS != rightS);
				case BinaryOperator.Gt			: return new CssBoolean(float.Parse(leftS) > float.Parse(rightS));
				case BinaryOperator.Gte			: return new CssBoolean(float.Parse(leftS) >= float.Parse(rightS));
				case BinaryOperator.Lt			: return new CssBoolean(float.Parse(leftS) < float.Parse(rightS));
				case BinaryOperator.Lte			: return new CssBoolean(float.Parse(leftS) <= float.Parse(rightS));
			}

			return new CssBoolean(true);
		}

		public CssValue EvalFunction(CssFunction function)
		{
			Func<CssValue[], CssValue> func;

			if (CssFunctions.TryGet(function.Name, out func))
			{
				var args = GetArgs(function.Arguments).ToArray();

				return func(args);
			}

			throw new Exception($"function named '{function.Name}' not registered");
		}

		#endregion

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

						WriteRoot(css);
					}
					catch (SyntaxException ex)
					{
						// response.StatusCode = 500;

						writer.WriteLine("body, html { background-color: red !important; }");
						writer.WriteLine("body * { display: none; }");

						writer.WriteLine($"/* --- Parse Error in '{absolutePath}':{ex.Message} ");

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
				case NodeKind.Variable	 : WriteVariable((CssVariable)value);				 break;
				case NodeKind.ValueList	 : WriteValueList((CssValueList)value);				 break;
				case NodeKind.Function	 : WriteFunction((CssFunction)value);				 break;
				case NodeKind.Expression : WriteValue(EvalulateExpression((CssValue)value)); break;
				default					 : writer.Write(value.ToString());					 break;
			}
		}

		public void WriteValueList(CssValueList list)
		{
			var i = 0;

			foreach (var value in list)
			{
				if (i != 0)
				{
					writer.Write(list.Seperator == ValueSeperator.Space ? " " : ", ");
				}

				WriteValue(value);

				i++;
			}
		}

		public void WriteFunction(CssFunction function)
		{
			// {name}({args})

			Func<CssValue[], CssValue> func;

			if (CssFunctions.TryGet(function.Name, out func))
			{
				var args = GetArgs(function.Arguments).ToArray();

				writer.Write(func(args));

				return;
			}
			
			writer.Write(function.Name);

			writer.Write("(");

			WriteValue(function.Arguments);
			
			writer.Write(")");
		}

		public IEnumerable<CssValue> GetArgs(CssValue value)
		{
			switch (value.Kind)
			{
				case NodeKind.Variable:					
					yield return scope.GetValue(((CssVariable)value).Symbol);
					
					break;

				case NodeKind.ValueList:
					{
						var list = (CssValueList)value;

						if (list.Seperator == ValueSeperator.Space) yield return list;

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
			var value = scope.GetValue(variable.Symbol);

			WriteValue(value);
		}

		public void WriteImportRule(ImportRule rule)
		{
			// TODO: normalize value
			writer.Write(rule.ToString());
		}

		public void WriteRule(CssRule rule, int level = 0)
		{
			var i = 0;

			foreach (var r in Rewrite(rule))
			{				
				if (i != 0) writer.WriteLine();

				_WriteRule(r, level);

				i++;			
			}
		}

		public void _WriteRule(CssRule rule, int level = 0)
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
			writer.Write("@" + rule.Name);

			if (rule.SelectorText != null)
			{
				writer.Write(" " + rule.SelectorText);
			}

			writer.Write(" ");

			WriteBlock(rule, level);
		}

		public void WriteStyleRule(StyleRule rule, int level)
		{
			WriteSelector(rule.Selector);

			writer.Write(" ");

			WriteBlock(rule, level);
		}

		public void WriteSelector(CssSelector selector)
		{
			if(selector.Count == 1) 
			{
				writer.Write(selector.ToString());
			}
			else
			{
				var i = 0;

				foreach(var s in selector)
				{
					if (i != 0)
					{
						writer.Write("," + Environment.NewLine);
					}

					writer.Write(s);

					i++;
				}
			}
		}

		public void WriteMediaRule(MediaRule rule, int level)
		{
			writer.Write("@media ");
			writer.Write(rule.RuleText); // Write rule text
			writer.Write(" ");
			WriteBlock(rule, level);
		}

		public void WriteFontFaceRule(FontFaceRule rule, int level)
		{
			writer.Write("@font-face "); // Write selector

			WriteBlock(rule, level);
		}

		public void WriteKeyframesRule(KeyframesRule rule, int level)
		{
			if (context.BrowserSupport != null)
			{
				// -moz-
				if (context.BrowserSupport.Any(a => a.Type == BrowserType.Firefox && a.Version < 16))
				{
					WriteKeyframesRule(context.BrowserSupport.First(b => b.Type == BrowserType.Firefox), rule, level);

					writer.WriteLine();
				}

				// -webkit- 
				if (context.BrowserSupport.Any(a => a.Type == BrowserType.Safari))
				{
					WriteKeyframesRule(context.BrowserSupport.First(b => b.Type == BrowserType.Safari), rule, level);

					writer.WriteLine();
				}
			}

			writer.Write("@keyframes ");
			writer.Write(rule.Name);
			writer.Write(" ");

			support = null;

			WriteBlock(rule, level); // super standards

			support = context.BrowserSupport;
		}

		private void WriteKeyframesRule(Browser browser, KeyframesRule rule, int level)
		{
			support = new[] { browser };

			writer.Write("@");
			writer.Write(browser.Prefix.Text);
			writer.Write("keyframes ");
			writer.Write(rule.Name);
			writer.Write(" ");

			WriteBlock(rule, level);

			support = context.BrowserSupport;
		}

		public void WriteBlock(CssBlock block, int level)
		{
			var prevScope = scope;

			writer.Write("{"); // Block start

			var condenced = false;
			var count = 0;

			// Write the declarations
			foreach (var node in block.Children) // TODO: Change to an immutable list?
			{
				if (node.Kind == NodeKind.Include)
				{
					var b2 = new CssBlock(NodeKind.Block);

					b2.Add(node);

					scope = ExpandInclude((IncludeNode)node, b2);

					foreach (var rule in b2.OfType<CssRule>())
					{
						writer.WriteLine();

						WriteRule(rule, level + 1);

						count++;
					}
                }
				else if (node.Kind == NodeKind.Declaration)
				{
					var declaration = (CssDeclaration)node;

					if (block.Children.Count == 1 && !declaration.Info.NeedsExpansion(support))
					{
						condenced = true;

						writer.Write(" ");

						WriteDeclaration(declaration, 0);
					}
					else
					{
						if (count == 0) writer.WriteLine();

						WritePatchedDeclaration(declaration, level + 1);
					}
				}
				else if (node.Kind == NodeKind.Rule)  // Nested rule
				{
					if (count == 0) writer.WriteLine();

					var childRule = (CssRule)node;

					WriteRule(childRule, level + 1);
				}
				else if (node.Kind == NodeKind.If)
				{
					EvaluateIf((IfBlock)node, level + 1);
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

			prevScope = scope;
		}

		public void WriteDeclaration(CssDeclaration declaration, int level)
		{
			Indent(level);

			writer.Write(declaration.Name);
			writer.Write(": ");
			WriteValue(declaration.Value);
			writer.Write(";");
		}

		public void WritePatchedDeclaration(CssDeclaration declaration, int level)
		{
            var prop = declaration.Info;

			if (support != null && prop.Compatibility.HasPatches)
			{
				var prefixes = BrowserPrefixKind.None;

				foreach (var browser in support)
				{
					if (!prop.Compatibility.IsPrefixed(browser)) continue;

					// Skip the prefix if we've already added it
					if (prefixes.HasFlag(browser.Prefix.Kind)) continue;

					var patchedValue = (prop.Compatibility.HasValuePatches)
						? GetPatchedValueFor(declaration.Value, browser)
						: declaration.Value;

					Indent(level);

					writer.Write(browser.Prefix);	// Write the prefix
					writer.Write(prop.Name);		// Write the standard name
					writer.Write(": ");
					WriteValue(patchedValue);
					writer.Write(";");

					writer.WriteLine();

					prefixes |= browser.Prefix.Kind;
				}
			}

			// Finally, write the standards declaration

			WriteDeclaration(declaration, level);
		}


		#region Compatibility helpers

		// transform 0.04s linear, opacity 0.04s linear, visibility 0.04s linear;

		private CssValue GetPatchedValueFor(CssValue value, Browser browser)
		{
			if (value.Kind != NodeKind.ValueList) return value;

			var a = (CssValueList)value;

			var list = new CssValueList(a.Seperator);

			foreach (var node in a)
			{
				if (node.Kind == NodeKind.ValueList) // For comma seperated componented lists
				{
					list.Add(GetPatchedValueFor(node, browser));
				}
				else if (node.Kind == NodeKind.String && node.ToString() == "transform")
				{
					list.Add(new CssString(browser.Prefix.Text + "transform"));
				}
				else
				{
					list.Add(node);
				}
			}

			return list;
		}

		#endregion

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

		#region Sass

		public IEnumerable<CssRule> Rewrite(CssRule rule)
		{
			var styleRule = rule as StyleRule;

			if (styleRule == null || rule.All(r => r.Kind == NodeKind.Declaration))
			{
				yield return rule;

				yield break;
			}

			// Figure out how to eliminate this clone

			var clone = (StyleRule)rule.CloneNode();		

			// Expand includes
			foreach (var includeNode in clone.Children.OfType<IncludeNode>().ToArray())
			{
				scope = ExpandInclude(includeNode, clone);

				clone.Children.Remove(includeNode);
			}

			var root = new List<CssRule>();

			root.Add(clone);

			foreach (var nestedRule in clone.Children.OfType<StyleRule>().ToArray())
			{
				foreach (var r in ExpandStyleRule(nestedRule, parent: clone))
				{
					root.Add(r);
				}
			}

			foreach (var r in root)
			{
				if (r.HasChildren) yield return r;
			}
		}

		public IEnumerable<CssRule> ExpandStyleRule(StyleRule rule, CssRule parent)
		{
			var newRule = new StyleRule(ExpandSelector(rule));

			foreach (var childNode in rule.Children.ToArray())
			{
				if (childNode is StyleRule)
				{
					var childRule = (StyleRule)childNode;

					foreach (var r in ExpandStyleRule(childRule, rule))
					{
						yield return r;
					}
				}
				else
				{
					newRule.Add(childNode);
				}
			}

			parent.Remove(rule); // Remove from parent node after it's been processed

			if (newRule.HasChildren) yield return newRule;
		}

		public CssScope ExpandInclude(IncludeNode include, CssBlock rule)
		{
			includeCount++;

			if (includeCount > 1000) throw new Exception("Exceded include limit of 1,000");

			MixinNode mixin;

			if (!context.Mixins.TryGetValue(include.Name, out mixin))
			{
				throw new Exception($"Mixin '{include.Name}' not registered");
			}

			var index = rule.Children.IndexOf(include);

			var childScope = GetScope(mixin.Parameters, include.Args);

			var i = 0;

			foreach (var node in mixin.Children.ToArray())
			{
				// Bind variables

				if (node is IncludeNode)
				{
					ExpandInclude(
						(IncludeNode)node,
						rule
					);

					mixin.Children.Remove(node);
				}

				rule.Insert(i + 1, node.CloneNode());

				i++;
			}

			return childScope;
        }

		public CssScope GetScope(IList<CssParameter> paramaters, CssValue args)
		{
			var list = new List<CssValue>();

			if (args != null)
			{
				var valueList = args as CssValueList;

				if (valueList == null)
				{
					list.Add(args); // Single Value
				}

				if (valueList != null && valueList.Seperator == ValueSeperator.Comma)
				{
					list.AddRange(valueList.OfType<CssValue>());
				}
			}

			var child = scope.GetChildScope(); 

			var i = 0;

			foreach (var p in paramaters)
			{
				var val = (list != null && list.Count >= i + 1) ? list[i] : p.DefaultValue;

				child.Add(p.Name, val);

				i++;
			}

			return child;
		}

		#endregion

		#region Selector Expansion

		public static CssSelector ExpandSelector(StyleRule rule)
		{
			var parts = new Stack<CssSelector>();

			parts.Push(rule.Selector);

			StyleRule current = rule;

			while ((current = current.Parent as StyleRule) != null)
			{
				parts.Push(current.Selector);

				if (parts.Count > 6)
				{
					throw new Exception(string.Format("Cannot nest more than 6 levels deep. Was {0}. ", string.Join(" ", parts)));
				}
			}

			var i = 0;

			var sb = new StringBuilder();

			foreach (var selector in parts)
			{
				if (selector.Contains("&"))
				{
					var x = selector.ToString().Replace("&", sb.ToString());

					sb.Clear();

					sb.Append(x);

					i++;

					continue;
				}

				if (i != 0) sb.Append(" ");

				i++;

				// h1, h2, h3

				if (selector.Count > 1)
				{
					var parentSelector = sb.ToString();

					sb.Clear();

					var c = GetSelector(parts.Skip(i));

					var q = 0;

					foreach (var a in selector)
					{
						if (q != 0) sb.Append(", ");

						sb.Append(parentSelector + a);

						if (c != null)
						{
							sb.Append(" " + c);
						}

						q++;
					}

					break;
				}
				else
				{
					sb.Append(selector);
				}
			}

			return new CssSelector(sb.ToString());
		}

		private static string GetSelector(IEnumerable<CssSelector> selectors)
		{
			// TODO: & support

			var sb = new StringBuilder();

			foreach (var selector in selectors)
			{
				sb.Append(selector);
			}

			return sb.ToString();
		}

		#endregion
	}
}