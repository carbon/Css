namespace Carbon.Css
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using Carbon.Css.Parser;
	using System.IO;
	using Carbon.Css.Helpers;
	using System;

	public class StyleSheet : CssRoot, IStylesheet
	{
		private readonly CssContext context;

		public StyleSheet(List<CssNode> children, CssContext context)
			: base(children)
		{
			this.context = context;
		}

		public StyleSheet(CssContext context)
			: base()
		{
			this.context = context;
		}

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

			var browsers = new List<Browser>();

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
				else if (node.Kind == NodeKind.Directive)
				{
					var directive = (CssDirective)node;

					if (directive.Name == "support")
					{
						
						var parts = directive.Value.Split(' ');

						var browserType = (BrowserType)Enum.Parse(typeof(BrowserType), parts[0].Trim(), true);
						// var compare = parts[1].Trim();
						var browserVersion = float.Parse(parts.Last().Trim(' ', '+'));

						browsers.Add(new Browser(browserType, browserVersion));
					}
				}
				else
				{
					sheet.AddChild(node);

					// TODO: Transform here?
				}
			}

			if (browsers.Count > 0)
			{
				sheet.Context.SetCompatibility(browsers.ToArray());
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

		private ICssResolver resolver;

		public void SetResolver(ICssResolver resolver)
		{
			this.resolver = resolver;
		}


		public void ExecuteRewriters()
		{
			return;
		}

		public void WriteTo(TextWriter textWriter)
		{
			var writer = new CssWriter(textWriter, context, resolver);

			writer.WriteRoot(this);
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
