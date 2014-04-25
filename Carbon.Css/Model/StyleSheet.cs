namespace Carbon.Css
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using Carbon.Css.Parser;
	using System.IO;
	using Carbon.Css.Helpers;

	public class StyleSheet : CssNode, IStylesheet
	{
		private readonly List<CssNode> children;
		private readonly CssContext context;

		public StyleSheet(List<CssNode> children, CssContext context)
			: base(NodeKind.Document)
		{
			this.children = children;
			this.context = context;
		}

		public StyleSheet(CssContext context)
			: base(NodeKind.Document)
		{
			this.children = new List<CssNode>();
			this.context = context;
		}

		public void AddChild(CssNode node)
		{
			node.Parent = this;

			children.Add(node);
		}

		public override IList<CssNode> Children
		{
			get { return children; }
		}

		#region Helpers

		public IList<CssRule> GetRules()
		{
			return children.OfType<CssRule>().ToList(); 
		}

		#endregion

		public CssContext Context
		{
			get { return context; }
		}

		public static StyleSheet Parse(string text, CssContext context = null)
		{
			if (context == null)
			{
				context = new CssContext();
			}

			var sheet = new StyleSheet(context);

			var rules = new List<CssRule>();

			var parser = new CssParser(text);

			foreach (var node in parser.ReadNodes())
			{
				if (node.Kind == NodeKind.Assignment)
				{
					var variable = (CssAssignment)node;

					context.Variables[variable.Name] = variable.Value;
				}
				else if(node.Kind == NodeKind.Mixin)
				{
					var mixin = (MixinNode)node;

					context.Mixins.Add(mixin.Name, mixin);
				}
				else
				{
					sheet.AddChild(node);
				}
			}

			return sheet;
		}

		public static StyleSheet FromFile(FileInfo file, CssContext context = null)
		{
			var text = "";

			using (var reader = file.OpenText())
			{
				text = reader.ReadToEnd();
			}


			try
			{
				return Parse(text, context);
			}
			catch (ParseException ex)
			{
				ex.Location = TextHelper.GetLocation(text, ex.Position);

				ex.Lines = TextHelper.GetLinesAround(text, ex.Location.Line, 3).ToList();

				throw ex;
			}
		}

		public void Compile(TextWriter writer)
		{
			WriteTo(writer);
		}

		#region Transformers

		private readonly RewriterCollection rewriters = new RewriterCollection();

		public void SetCompatibility(params Browser[] targets)
		{
			// rewriters.Add(new IEOpacityTransform());

			rewriters.Add(new AddVendorPrefixesTransform(targets));
		}

		public void AllowNestedRules()
		{
			rewriters.Add(new ExpandNestedStylesRewriter());
		}

		public void AddRewriter(ICssTransformer rewriter)
		{
			rewriters.Add(rewriter);
		}

		public void ExecuteRewriters()
		{
			foreach (var rewriter in rewriters)
			{
				var index = 0;

				foreach (var node in children.ToList())
				{
					var rule = node as CssRule;

					if (rule != null)
					{
						rewriter.Transform(rule, index);
					}

					index++;
				}
			}
			
			
		}

		#endregion
		
		public void WriteTo(TextWriter textWriter)
		{
			var writer = new CssWriter(textWriter, context);

			var i = 0;

			foreach (var node in children)
			{
				var rule = node as CssRule;

				if (rule != null)
				{
					if (i != 0)
					{
						textWriter.WriteLine();
					}

					writer.WriteRule(rule);

					i++;
				}
			}
		}

		public override string Text
		{
			get { return ToString(); }
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
