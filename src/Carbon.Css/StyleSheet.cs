using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using Carbon.Css.Helpers;
using Carbon.Css.Parser;

namespace Carbon.Css
{
    public sealed class StyleSheet : CssRoot, IStylesheet
    {
        public StyleSheet(List<CssNode> children, CssContext? context)
            : base(children)
        {
            Context = context;
        }

        public StyleSheet(CssContext? context)
            : base()
        {
            Context = context;
        }

        public CssContext? Context { get; }

        public static StyleSheet Parse(Stream stream, CssContext? context = null)
        {
            return Parse(new StreamReader(stream), context);
        }

        public static StyleSheet Parse(string text, CssContext? context = null)
        {
            return Parse(new StringReader(text), context);
        }

        private static readonly char[] trimBrowserChars = { ' ', '+' };

        public static StyleSheet Parse(TextReader reader, CssContext? context = null)
        {
            var sheet = new StyleSheet(context ?? new CssContext());

            IList<BrowserInfo>? browsers = null;

            using (var parser = new CssParser(reader))
            {
                foreach (var node in parser.ReadNodes())
                {
                    if (node.Kind is NodeKind.Mixin)
                    {
                        var mixin = (MixinNode)node;

                        sheet.Context!.Mixins.Add(mixin.Name, mixin);
                    }
                    else if (node.Kind is NodeKind.Directive)
                    {
                        var directive = (CssDirective)node;

                        if (directive.Name is "support" && directive.Value != null)
                        {
                            string[] parts = directive.Value.Split(' ');

                            if (Enum.TryParse(parts[0].Trim(), true, out BrowserType browserType))
                            {
                                browsers ??= new List<BrowserInfo>(2);

                                float browserVersion = float.Parse(parts[^1].AsSpan().Trim(trimBrowserChars), provider: CultureInfo.InvariantCulture);

                                browsers.Add(new BrowserInfo(browserType, browserVersion));
                            }
                        }
                    }
                    else
                    {
                        sheet.AddChild(node);
                    }
                }

                if (browsers != null)
                {
                    sheet.Context!.SetCompatibility(browsers.ToArray());
                }
            }

            return sheet;
        }

        public void InlineImports()
        {
            if (resolver is null)
            {
                throw new ArgumentNullException(nameof(resolver));
            }

            foreach (var rule in Children.OfType<ImportRule>().ToArray())
            {
                if (rule.Url.IsPath)
                {
                    ImportInline(rule);
                }

                Children.Remove(rule);
            }
        }

        public static StyleSheet FromFile(FileInfo file, CssContext? context = null)
        {
            string text = File.ReadAllText(file.FullName);

            try
            {
                return Parse(text, context);
            }
            catch (SyntaxException ex)
            {
                ex.Location = TextHelper.GetLocation(text, ex.Position);

                ex.Lines = TextHelper.GetLinesAround(text, ex.Location.Line, 3).ToList();

                throw;
            }
        }

        public void Compile(TextWriter writer)
        {
            WriteTo(writer);
        }

        private ICssResolver? resolver;

        public void SetResolver(ICssResolver resolver)
        {
            this.resolver = resolver;
        }

        public void WriteTo(TextWriter textWriter)
        {
            var writer = new CssWriter(textWriter, Context, new CssScope(), resolver);

            writer.WriteRoot(this);
        }

        public void WriteTo(TextWriter textWriter, IEnumerable<KeyValuePair<string, CssValue>> variables)
        {
            var scope = new CssScope();

            foreach (var v in variables)
            {
                scope.Add(v.Key, v.Value);
            }

            var writer = new CssWriter(textWriter, Context, scope, resolver);

            writer.WriteRoot(this);
        }

        public string ToString(IEnumerable<KeyValuePair<string, CssValue>> variables)
        {
            using var writer = new StringWriter();

            WriteTo(writer, variables);

            return writer.ToString();
        }

        public override string ToString()
        {
            using var writer = new StringWriter();

            WriteTo(writer);

            return writer.ToString();
        }

        #region Helpers

        private void ImportInline(ImportRule rule)
        {
            if (resolver is null) throw new Exception("No resolver was set");

            // var relativePath = importRule.Url;
            var absolutePath = rule.Url.GetAbsolutePath(resolver.ScopedPath);

            // Assume to be scss if there is no extension
            if (!absolutePath.Contains('.'))
            {
                absolutePath += ".scss";
            }

            var stream = resolver.Open(absolutePath.TrimStart('/'));

            if (stream != null)
            {
                if (absolutePath.EndsWith(".scss", StringComparison.Ordinal))
                {
                    AddChild(new CssComment($"imported: '{absolutePath}"));

                    try
                    {
                        foreach (var node in Parse(stream, Context).Children)
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

                        var sb = StringBuilderCache.Aquire();

                        if (ex.Lines != null)
                        {
                            foreach (var line in ex.Lines)
                            {
                                sb.Append(line.Number.ToString().PadLeft(5));
                                sb.Append(". ");

                                if (line.Number == ex.Location.Line)
                                {
                                    sb.Append("* ");
                                }

                                sb.AppendLine(line.Text);
                            }
                        }

                        AddChild(new CssComment(StringBuilderCache.ExtractAndRelease(sb)));

                        return;
                    }
                }
                else
                {
                    throw new Exception(".css include not supported");
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