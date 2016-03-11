using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carbon.Css
{
    using Parser;
    using Helpers;

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

        public CssContext Context => context;

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
                if (node.Kind == NodeKind.Mixin)
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
                }
            }

            if (browsers.Count > 0)
            {
                sheet.Context.SetCompatibility(browsers.ToArray());
            }

            return sheet;
        }

        public void InlineImports()
        {
            if (resolver == null) throw new ArgumentNullException(nameof(resolver));

            foreach (var rule in this.Children.OfType<ImportRule>().ToArray())
            {
                if (rule.Url.IsPath)
                {
                    ImportInline(rule);
                }

                this.children.Remove(rule);
            }
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
            catch (SyntaxException ex)
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

        public void WriteTo(TextWriter textWriter)
        {
            var writer = new CssWriter(textWriter, context, resolver);

            writer.WriteRoot(this);
        }

        public void WriteTo(TextWriter textWriter, IEnumerable<KeyValuePair<string, CssValue>> variables)
        {
            var scope = new CssScope();

            foreach (var v in variables)
            {
                scope.Add(v.Key, v.Value);
            }

            var writer = new CssWriter(textWriter, context, resolver);

            writer.SetScope(scope);

            writer.WriteRoot(this);
        }

        public string ToString(IEnumerable<KeyValuePair<string, CssValue>> variables)
        {
            var sb = new StringBuilder();

            using (var sw = new StringWriter(sb))
            {
                WriteTo(sw, variables);

                return sb.ToString();
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


        #region Helpers

        private void ImportInline(ImportRule rule)
        {
            // var relativePath = importRule.Url;
            var absolutePath = rule.Url.GetAbsolutePath(resolver.ScopedPath);

            // Assume to be scss if there is no extension
            if (!absolutePath.Contains('.')) absolutePath += ".scss";

            var text = resolver.GetText(absolutePath.TrimStart('/'));

            if (text != null)
            {
                if (Path.GetExtension(absolutePath) == ".scss")
                {
                    AddChild(new CssComment($"imported: '{absolutePath}"));

                    try
                    {

                        foreach (var node in Parse(text, context).Children)
                        {
                            AddChild(node);
                        }

                    }
                    catch (SyntaxException ex)
                    {
                        AddChild(new StyleRule("body, html") {
                           new CssDeclaration("background-color", "red", "important")
                        });

                        AddChild(new StyleRule("body *") {
                            { "display", "none" }
                        });
                        
                        AddChild(new CssComment($" --- Syntax error reading '{absolutePath}' : {ex.Message} --- "));

                        var sb = new StringBuilder();

                        if (ex.Lines != null)
                        {
                            foreach (var line in ex.Lines)
                            {
                                sb.Append(string.Format("{0}. ", line.Number.ToString().PadLeft(5)));

                                if (line.Number == ex.Location.Line)
                                {
                                    sb.Append("* ");
                                }

                                sb.AppendLine(line.Text);
                            }
                        }

                        AddChild(new CssComment(sb.ToString()));

                        return;
                    }
                }
                else
                {
                    throw new Exception(".css include not supported");
                    //  writer.Write(text);
                }
            }
            else
            {
                AddChild(new CssComment($"NOT FOUND: '{absolutePath}"));
            }
        }

        #endregion
    }
}