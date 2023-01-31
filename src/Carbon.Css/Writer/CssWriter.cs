#pragma warning disable IDE0066 // Convert switch statement to expression

using System.Globalization;
using System.IO;
using System.Linq;

using Carbon.Css.Parser;

namespace Carbon.Css;

public sealed class CssWriter : IDisposable
{
    private readonly TextWriter _writer;
    private readonly CssContext _context;
    private readonly ICssResolver? _resolver;
    private int includeCount;
    private int importCount;
    private int nodeCount;
    private bool skipMath;

    private BrowserInfo[]? _browserSupport;

    private CssScope _scope;

    public CssWriter(
        TextWriter writer,
        CssContext? context = null,
        CssScope? scope = null,
        ICssResolver? resolver = null)
    {
        _writer = writer;
        _context = context ?? new CssContext();
        _resolver = resolver;
        _scope = scope ?? new CssScope();

        includeCount = 0;
        importCount = 0;
        nodeCount = 0;
        skipMath = false;

        _browserSupport = _context.BrowserSupport;
    }

    public void WriteRoot(StyleSheet sheet)
    {
        importCount++;

        if (importCount > 200)
        {
            throw new Exception("Exceded importCount of 200");
        }

        uint i = 0;

        foreach (var node in sheet.Children)
        {
            if (node.Kind is NodeKind.If)
            {
                EvaluateIf((IfBlock)node, i: i);
            }
            else if (node.Kind is NodeKind.For)
            {
                EvaluateFor((ForBlock)node);
            }
            else if (node.Kind is NodeKind.Each)
            {
                EvaluateEach((EachBlock)node);
            }
            else if (node.Kind is NodeKind.Comment)
            {
                if (i > 0) _writer.WriteLine();

                i++;

                WriteComment((CssComment)node);
            }
            else if (node.Kind is NodeKind.Assignment)
            {
                var variable = (CssAssignment)node;

                _scope[variable.Name] = variable.Value;
            }
            else if (node is CssRule rule)
            {
                if (i > 0)
                {
                    _writer.WriteLine();
                }

                i++;

                if (rule.Type is RuleType.Import)
                {
                    var importRule = (ImportRule)rule;

                    if (!importRule.Url.IsPath || _resolver is null)
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
    }

    #region Expressions

    public void EvaluateIf(IfBlock block, int level = 0, uint i = 0)
    {
        CssValue result = EvalulateExpression(block.Condition);

        if (ToBoolean(result))
        {
            if (i > 0)
            {
                _writer.WriteLine();
            }

            WriteBlockBody(block, level);
        }
    }

    public void EvaluateFor(ForBlock block, int level = 0)
    {
        int start = (int)((CssUnitValue)EvalulateExpression(block.Start)).Value;
        int end = (int)((CssUnitValue)EvalulateExpression(block.End)).Value;

        if (!block.IsInclusive) // through
        {
            end--;
        }

        if (end < start)
        {
            throw new Exception("end must be after the start");
        }

        if (end - start > 10_000)
        {
            throw new Exception("Must be less than 10,000");
        }


        _scope = _scope.GetChildScope();

        int a = 0;

        for (int i = start; i <= end; i++)
        {
            if (a > 0) _writer.WriteLine();

            _scope[block.Variable.Symbol] = CssValue.Number(i);

            WriteBlockBody(block, level);

            a++;
        }

        _scope = _scope.Parent!;
    }

    public void EvaluateEach(EachBlock block, int level = 0)
    {
        var enumerable = (CssValueList)EvalulateExpression(block.Enumerable);

        _scope = _scope.GetChildScope();

        int a = 0;

        foreach (var value in enumerable)
        {
            if (a > 0) _writer.WriteLine();

            _scope[block.Variable.Symbol] = value;

            WriteBlockBody(block, level);

            a++;
        }

        _scope = _scope.Parent!;
    }

    private void WriteBlockBody(CssBlock block, int level = 0)
    {
        int i = 0;

        foreach (CssNode child in block.Children)
        {
            if (child is CssRule rule)
            {
                if (i > 0) _writer.WriteLine();

                WriteRule(rule);

                i++;
            }
            else if (child is CssAssignment assignment)
            {
                _scope[assignment.Name] = assignment.Value;
            }
            else if (child is CssDeclaration declaration)
            {
                if (i > 0) _writer.WriteLine();

                WriteDeclaration(declaration, level);

                i++;
            }
        }
    }

    private static bool ToBoolean(object value) => value is CssBoolean { Value: true };

    public CssValue EvalulateExpression(CssValue expression)
    {
        switch (expression.Kind)
        {
            case NodeKind.Variable   : return _scope.GetValue(((CssVariable)expression).Symbol);
            case NodeKind.Expression : return EvaluateBinaryExpression((BinaryExpression)expression);
            case NodeKind.Function:

                var function = (CssFunction)expression;

                if (IsCssFunction(function.Name))
                {
                    return function;
                }

                return EvalFunction(function);
            default: return expression;
        }
    }

    private static bool IsCssFunction(string name)
    {
        return name is "attr" or "calc" or "cubic-bezier" or "var";
    }

    public CssValue EvaluateBinaryExpression(BinaryExpression expression)
    {
        CssValue lhs = EvalulateExpression(expression.Left);
        CssValue rhs = EvalulateExpression(expression.Right);

        if (skipMath || !CssValue.AreCompatible(lhs, rhs, expression.Operator))
        {
            var values = new CssValue[] {
                    lhs,
                    new CssString(expression.OperatorToken),
                    rhs
                };

            return new CssValueList(values, CssValueSeperator.Space);
        }

        switch (expression.Operator)
        {
            case BinaryOperator.Divide    : return ((CssUnitValue)lhs).Divide(rhs);
            case BinaryOperator.Multiply  : return ((CssUnitValue)lhs).Multiply(rhs);
            case BinaryOperator.Add       : return ((CssUnitValue)lhs).Add(rhs);
            case BinaryOperator.Subtract  : return ((CssUnitValue)lhs).Subtract(rhs);
        }

        return expression.Operator switch
        {
            BinaryOperator.Eq        => CssBoolean.Get(AreEqual(lhs, rhs)),
            BinaryOperator.NotEquals => CssBoolean.Get(!AreEqual(lhs, rhs)),
            BinaryOperator.Gt        => CssBoolean.Get(ToDouble(lhs) > ToDouble(rhs)),
            BinaryOperator.Gte       => CssBoolean.Get(ToDouble(lhs) >= ToDouble(rhs)),
            BinaryOperator.Lt        => CssBoolean.Get(ToDouble(lhs) < ToDouble(rhs)),
            BinaryOperator.Lte       => CssBoolean.Get(ToDouble(lhs) <= ToDouble(rhs)),
            _                        => CssBoolean.True
        };
    }

    private static bool AreEqual(CssValue lhs, CssValue rhs)
    {
        if (lhs.Kind is NodeKind.Undefined)
        {
            return rhs.Kind is NodeKind.Undefined || rhs is CssString { Text: "undefined" };
        }

        if (lhs is CssUnitValue lv && rhs is CssUnitValue rv)
        {
            return lv.Equals(rv);
        }

        return string.Equals(lhs.ToString()!, rhs.ToString()!, StringComparison.OrdinalIgnoreCase);
    }

    private static double ToDouble(CssValue value)
    {
        if (value is CssUnitValue { Kind: NodeKind.Number } uv)
        {
            return uv.Value;
        }

        return double.Parse(value.ToString()!, CultureInfo.InvariantCulture);
    }

    public CssValue EvalFunction(CssFunction function)
    {
        if (CssFunctions.TryGet(function.Name, out var func))
        {
            CssValue[] args = GetArgs(function.Arguments).ToArray();

            return func(args);
        }

        throw new Exception($"ƒ {function.Name} not found");
    }

    #endregion

    public void InlineImport(ImportRule importRule, StyleSheet sheet)
    {
        if (_resolver is null)
        {
            throw new Exception("No resolver registered");
        }

        // var relativePath = importRule.Url;
        var absolutePath = importRule.Url.GetAbsolutePath(_resolver.ScopedPath);

        if (absolutePath[0] is '/')
        {
            absolutePath = absolutePath[1..];
        }

        // Assume to be scss if there is no extension
        if (!absolutePath.Contains('.'))
        {
            absolutePath += ".scss";
        }

        _writer.WriteLine();
        _writer.WriteLine($"/* {absolutePath} */");

        var stream = _resolver.Open(absolutePath);

        if (stream is null)
        {
            _writer.WriteLine("/* not found */");

            return;
        }

        if (absolutePath.EndsWith(".scss", StringComparison.Ordinal))
        {
            try
            {
                var css = StyleSheet.Parse(stream, _context);

                WriteRoot(css);
            }
            catch (SyntaxException ex)
            {
                _writer.WriteLine("body, html { background-color: red !important; }");
                _writer.WriteLine("body * { display: none; }");

                _writer.WriteLine($"/* --- Parse Error in '{absolutePath}' : {ex.Message} ");

                if (ex.Lines is not null)
                {
                    foreach (var line in ex.Lines)
                    {
                        _writer.Write(line.Number.ToString(CultureInfo.InvariantCulture).PadLeft(4)); // 1
                        _writer.Write(". ");

                        if (line.Number == ex.Location.Line)
                        {
                            _writer.Write("* ");
                        }

                        _writer.WriteLine(line.Text);
                    }
                }

                _writer.Write("*/");

                return;
            }
        }
        else
        {
            // Copy the file line by line...

            using var reader = new StreamReader(stream);

            string? line;

            while ((line = reader.ReadLine()) is not null)
            {
                _writer.WriteLine(line);
            }
        }
    }

    public void WriteComment(CssComment comment)
    {
        _writer.Write("/* ");
        _writer.Write(comment.Text);
        _writer.WriteLine(" */");
    }

    public void WriteValue(CssNode value)
    {
        if (nodeCount > 100_000)
        {
            throw new Exception("May not write more than 100,000 nodes");
        }

        nodeCount++;

        switch (value.Kind)
        {
            case NodeKind.Variable           : WriteVariable((CssVariable)value); break;
            case NodeKind.ValueList          : WriteValueList((CssValueList)value); break;
            case NodeKind.Function           : WriteFunction((CssFunction)value); break;
            case NodeKind.Expression         : WriteValue(EvalulateExpression((CssValue)value)); break;
            case NodeKind.InterpolatedString : WriteInterpolatedString((CssInterpolatedString)value); break;
            case NodeKind.Reference          : WriteReference((CssReference)value); break;
            case NodeKind.Sequence           : WriteSequence((CssSequence)value); break;
            case NodeKind.String             : WriteString((CssString)value); break;
            case NodeKind.Undefined          : _writer.Write(value.ToString()); break;
            default:
                // TODO: Improve error handling
                WriteUnitValue((CssUnitValue)value);

                break;
        }
    }

    public void WriteUnitValue(CssUnitValue value)
    {
        value.WriteTo(_writer);
    }

    public void WriteString(CssString value)
    {
        _writer.Write(value.Text);
    }

    public void WriteValueList(CssValueList list)
    {
        int i = 0;

        foreach (CssValue value in list)
        {
            if (i > 0)
            {
                if (list[i - 1] is CssInterpolatedString last)
                {
                    WriteTrivia(last.Trailing);
                }

                else if (list.Seperator is CssValueSeperator.Space)
                {
                    _writer.Write(' ');
                }
                else
                {
                    _writer.Write(", ");
                }
            }

            WriteValue(value);

            i++;
        }
    }

    private void WriteTrivia(Trivia? trivia)
    {
        if (trivia is null) return;

        foreach (var token in trivia)
        {
            _writer.Write(token.Text);
        }
    }

    public void WriteFunction(CssFunction function)
    {
        // {name}({args})

        if (CssFunctions.TryGet(function.Name, out var func))
        {
            CssValue[] args = GetArgs(function.Arguments).ToArray();

            _writer.Write(func(args));

            return;
        }

        if (function.Name is "calc")
        {
            skipMath = true;
        }

        _writer.Write(function.Name);

        _writer.Write('(');
        WriteValue(function.Arguments);
        _writer.Write(')');

        skipMath = false;
    }

    public IEnumerable<CssValue> GetArgs(CssValue value)
    {
        switch (value.Kind)
        {
            case NodeKind.Variable:
                yield return _scope.GetValue(((CssVariable)value).Symbol);

                break;

            case NodeKind.ValueList:
                var list = (CssValueList)value;

                if (list.Seperator is CssValueSeperator.Space) yield return list;

                // Break out comma seperated values
                foreach (var v in list)
                {
                    foreach (var item in GetArgs(v))
                    {
                        yield return item;
                    }
                }

                break;

            case NodeKind.Expression:
                yield return EvaluateBinaryExpression((BinaryExpression)value);

                break;

            // Function, etc 
            default:
                yield return value;
                break;
        }

    }

    public void WriteVariable(CssVariable variable)
    {
        var value = _scope.GetValue(variable.Symbol);

        bool sm = skipMath;

        skipMath = false;

        WriteValue(value);

        skipMath = sm;
    }

    public void WriteInterpolatedString(CssInterpolatedString node)
    {
        WriteValue(node.Expression);
    }

    public void WriteImportRule(ImportRule rule)
    {
        rule.WriteTo(_writer);
    }

    public void WriteRule(CssRule rule, int depth = 0)
    {
        if (rule.IsComplex && rule is StyleRule styleRule)
        {
            uint i = 0;

            foreach (var r in Rewrite(styleRule))
            {
                if (i > 0) _writer.WriteLine();

                WriteRuleInternal(r, depth);

                i++;
            }
        }
        else
        {
            WriteRuleInternal(rule, depth);
        }
    }

    private void WriteRuleInternal(CssRule rule, int depth = 0)
    {
        Indent(depth);

        switch (rule)
        {   
            case ImportRule importRule      : WriteImportRule(importRule);             break;
            case MediaRule mediaRule        : WriteMediaRule(mediaRule, depth);        break;
            case StyleRule styleRule        : WriteStyleRule(styleRule, depth);        break;
            case FontFaceRule fontFaceRule  : WriteFontFaceRule(fontFaceRule, depth);  break;
            case KeyframesRule keyFrameRule : WriteKeyframesRule(keyFrameRule, depth); break;
            case UnknownRule atRule         : WriteAtRule(atRule, depth);              break;
            default:
                throw new Exception($"Unhandled rule. Was {rule.GetType().Name}");
        }
    }

    public void WriteAtRule(UnknownRule rule, int depth)
    {
        _writer.Write('@');
        _writer.Write(rule.Name);

        if (rule.Text is not null)
        {
            _writer.Write(' ');
            rule.Text.WriteTo(_writer);
        }

        _writer.Write(' ');

        WriteBlock(rule, depth);
    }

    public void WriteStyleRule(StyleRule rule, int depth)
    {
        WriteSelector(rule.Selector);

        _writer.Write(' ');

        WriteBlock(rule, depth);
    }

    public void WriteSelector(CssSelector selectorList)
    {
        // throw new Exception(string.Join('|', selectorList));

        for (int selectorIndex = 0; selectorIndex < selectorList.Count; selectorIndex++)
        {
            var selector = selectorList[selectorIndex];

            if (selectorIndex > 0)
            {
                if (selector.Count is 1)
                {
                    _writer.Write(", ");
                }
                else
                {
                    _writer.WriteLine(',');
                }
            }

            for (int childIndex = 0; childIndex < selector.Count; childIndex++)
            {
                var item = selector[childIndex];

                WriteValue(item);

                bool isLast = (childIndex + 1) == selector.Count;

                if ((item.Kind is NodeKind.Sequence || item.Trailing is not null) && !isLast)
                {
                    _writer.Write(' ');
                }
            }
        }
    }

    public void WriteSequence(CssSequence sequence)
    {
        for (var i = 0; i < sequence.Count; i++)
        {
            var item = sequence[i];

            WriteValue(item);

            bool isLast = (i + 1) == sequence.Count;

            // Skip trailing trivia
            if (item.Trailing is not null && !isLast)
            {
                _writer.Write(' ');
            }
        }
    }

    public void WriteReference(CssReference reference)
    {
        // &

        var value = reference.Value;

        if (value is null) return;

        for (var i = 0; i < value.Count; i++)
        {
            var item = value[i];

            WriteValue(item);

            // Skip trailing trivia
            if ((item.Kind is NodeKind.Sequence || item.Trailing is not null) && (i + 1) != value.Count)
            {
                _writer.Write(' ');
            }
        }
    }

    public void WriteMediaRule(MediaRule rule, int depth)
    {
        _writer.Write("@media ");

        rule.Queries.WriteTo(_writer, _scope);

        _writer.Write(' ');

        WriteBlock(rule, depth);
    }

    public void WriteFontFaceRule(FontFaceRule rule, int depth)
    {
        _writer.Write("@font-face ");

        WriteBlock(rule, depth);
    }

    public void WriteKeyframesRule(KeyframesRule rule, int depth)
    {
        if (_context.BrowserSupport is not null)
        {
            // -moz-
            if (_context.Compatibility.Firefox > 0 && _context.Compatibility.Firefox < 16)
            {
                WritePrefixedKeyframesRule(BrowserInfo.Firefox(_context.Compatibility.Firefox), rule, depth);

                _writer.WriteLine();
            }

            // -webkit- 
            if (_context.Compatibility.Safari > 0 && _context.Compatibility.Safari < 9)
            {
                WritePrefixedKeyframesRule(BrowserInfo.Safari(_context.Compatibility.Safari), rule, depth);

                _writer.WriteLine();
            }
        }

        _writer.Write("@keyframes ");
        _writer.Write(rule.Name);
        _writer.Write(' ');

        _browserSupport = null;

        WriteBlock(rule, depth); // super standards

        _browserSupport = _context.BrowserSupport;
    }

    private void WritePrefixedKeyframesRule(BrowserInfo browser, KeyframesRule rule, int depth)
    {
        _browserSupport = new[] { browser };

        _writer.Write('@');
        _writer.Write(browser.Prefix.Text);
        _writer.Write("keyframes ");
        _writer.Write(rule.Name);
        _writer.Write(' ');

        WriteBlock(rule, depth);

        _browserSupport = _context.BrowserSupport;
    }

    public void WriteBlock(CssBlock block, int depth)
    {
        _writer.Write('{'); // Block start

        var condenced = false;
        var count = 0;

        // Write the declarations
        foreach (var node in block.Children) // TODO: Change to an immutable list?
        {
            if (node.Kind is NodeKind.Include)
            {
                var b2 = new CssBlock(NodeKind.Block) {
                    node
                };

                _scope = ExpandInclude((IncludeNode)node, b2);

                foreach (var rule in b2.OfType<CssRule>())
                {
                    _writer.WriteLine();

                    WriteRule(rule, depth + 1);

                    count++;
                }
            }
            else if (node.Kind is NodeKind.Declaration)
            {
                var declaration = (CssDeclaration)node;

                if (block.Children.Count is 1 && !declaration.Info.NeedsExpansion(declaration, _browserSupport))
                {
                    condenced = true;

                    _writer.Write(' ');

                    WriteDeclaration(declaration, 0);
                }
                else
                {
                    if (count is 0) _writer.WriteLine();

                    WritePatchedDeclaration(declaration, depth + 1);
                }
            }
            else if (node.Kind is NodeKind.Rule)  // Nested rule
            {
                if (count is 0) _writer.WriteLine();

                var childRule = (CssRule)node;

                WriteRule(childRule, depth + 1);
            }
            else if (node.Kind is NodeKind.If)
            {
                EvaluateIf((IfBlock)node, depth + 1);
            }
            else if (node.Kind is NodeKind.For)
            {
            }

            if (!condenced)
            {
                _writer.WriteLine();
            }

            count++;
        }

        // Limit to declaration
        if (condenced)
        {
            _writer.Write(' ');
        }
        else
        {
            Indent(depth);
        }

        _writer.Write('}'); // Block end
    }

    public void WriteDeclaration(CssDeclaration declaration, int level)
    {
        Indent(level);

        _writer.Write(declaration.Name);
        _writer.Write(": ");
        WriteValue(declaration.Value);
        _writer.Write(';');
    }

    public void WritePatchedDeclaration(CssDeclaration declaration, int level)
    {
        CssProperty prop = declaration.Info;

        if (_browserSupport is not null && prop.Compatibility.HasPatches)
        {
            BrowserPrefixKind prefixes = default;

            for (uint i = 0; i < (uint)_browserSupport.Length; i++)
            {
                ref BrowserInfo browser = ref _browserSupport[i];

                // Skip the prefix if we've already added it
                if (prefixes.HasFlag(browser.Prefix.Kind)) continue;

                if (!prop.Compatibility.HasPatch(declaration, browser)) continue;

                CssPatch patch = prop.Compatibility.GetPatch(declaration, browser);

                Indent(level);

                _writer.Write(patch.Name);
                _writer.Write(": ");
                WriteValue(patch.Value);
                _writer.Write(';');

                _writer.WriteLine();

                prefixes |= browser.Prefix.Kind;
            }
        }

        // Finally, write the standard declaration

        WriteDeclaration(declaration, level);
    }

    #region Helpers

    public void Indent(int level)
    {
        // Indent two characters for each level
        for (int i = 0; i < level; i++)
        {
            _writer.Write("  ");
        }
    }

    #endregion

    #region Sass

    public IEnumerable<CssRule> Rewrite(StyleRule rule)
    {
        if (rule.IsSimple)
        {
            yield return rule;

            yield break;
        }

        if (rule.Flags.HasFlag(CssBlockFlags.HasChildMedia))
        {
            throw new Exception("Nested @media rules are not supported yet");
        }

        var clone = rule.CloneNode(); // TODO: Eliminate

        if (rule.Flags.HasFlag(CssBlockFlags.HasIncludes)) // Expand the includes
        {
            foreach (var includeNode in clone.Children.OfType<IncludeNode>().ToList())
            {
                _scope = ExpandInclude(includeNode, clone);

                clone.Children.Remove(includeNode);
            }
        }

        var root = new List<CssRule> {
                clone
            };

        foreach (var nestedRule in clone.Children.OfType<StyleRule>().ToList())
        {
            foreach (var r in ExpandStyleRule(nestedRule, parent: clone))
            {
                root.Add(r);
            }
        }

        foreach (CssRule r in root)
        {
            if (r.HasChildren)
            {
                yield return r;
            }

        }
    }

    private IEnumerable<CssRule> ExpandStyleRule(StyleRule rule, CssRule parent)
    {
        var newRule = new StyleRule(ExpandSelector(rule));

        bool hasNestedRules = false;

        for (var i = 0; i < rule.Children.Count; i++)
        {
            if (rule.Children[i] is StyleRule)
            {
                hasNestedRules = true;

                break;
            }
        }

        if (hasNestedRules)
        {
            foreach (CssNode childNode in rule.Children.ToList())
            {
                if (childNode is StyleRule childRule)
                {
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
        }
        else
        {
            newRule.AddRange(rule.Children);
        }

        parent.Remove(rule); // Remove from parent node after it's been processed

        if (newRule.HasChildren)
        {
            yield return newRule;
        }
    }

    public CssScope ExpandInclude(IncludeNode include, CssBlock rule)
    {
        includeCount++;

        if (includeCount > 1_000)
        {
            throw new Exception("Exceeded include limit of 1,000");
        }

        if (!_context.Mixins.TryGetValue(include.Name, out MixinNode? mixin))
        {
            throw new Exception($"mixin '{include.Name}' not found");
        }

        CssScope childScope = GetScope(mixin.Parameters, include.Args);

        int i = 0;

        foreach (var node in mixin.Children.ToList())
        {
            // Bind variables

            if (node is IncludeNode includeNode)
            {
                ExpandInclude(includeNode, rule);

                mixin.Children.Remove(node);
            }

            rule.Insert(i + 1, node.CloneNode());

            i++;
        }

        return childScope;
    }

    public CssScope GetScope(IReadOnlyList<CssParameter> paramaters, CssValue? args)
    {
        CssValue[]? list = null;

        if (args is not null)
        {
            if (args is CssValueList valueList)
            {
                if (valueList.Seperator is CssValueSeperator.Comma)
                {
                    list = valueList.OfType<CssValue>().ToArray();
                }
            }
            else
            {
                list = new[] { args }; // Single Value
            }
        }

        CssScope child = _scope.GetChildScope();

        for (int i = 0; i < paramaters.Count; i++)
        {
            var p = paramaters[i];

            CssValue? val = (list is not null && list.Length >= i + 1) ? list[i] : p.DefaultValue;

            child.Add(p.Name, val!);
        }

        return child;
    }

    #endregion

    #region Selector Expansion

    public static CssSelector ExpandSelector(StyleRule rule)
    {
        var ancestors = new List<CssSelector>(rule.Depth) {
                rule.Selector
            };

        StyleRule? current = rule;

        while ((current = current!.Parent as StyleRule) is not null)
        {
            ancestors.Add(current.Selector);

            if (ancestors.Count > 6)
            {
                var debugParts = string.Join(' ', ancestors);

                throw new Exception($"May not nest more than 6 levels deep. Was {debugParts}.");
            }
        }

        ancestors.Reverse();

        var result = new List<CssSequence>();

        // { &.open { } }

        CssSequence? span = new CssSequence();

        for (int i = 0; i < ancestors.Count; i++)
        {
            var ancestor = ancestors[i];

            if (ancestor.Count is 1 && ancestor.Contains(NodeKind.Reference))
            {
                var prev = span;

                span = new CssSequence();

                foreach (var a in ancestor)
                {
                    foreach (var node in a)
                    {
                        if (node is CssReference reference)
                        {
                            reference.Value = prev;
                        }

                        span.Add(node);
                    }
                }

                continue;
            }

            if (ancestor.Count > 1)
            {
                // The node is a muliselector
                // e.g. h1, h2, h3

                var parentSelector = span;

                foreach (var item in ancestor)
                {
                    span = new CssSequence();

                    if (parentSelector.Count > 0)
                    {
                        // expand and flatten the parent
                        foreach (var part in parentSelector)
                        {
                            span.Add(part);
                        }
                    }

                    if (item.Contains(NodeKind.Reference))
                    {
                        span = SetReference(item, span);
                    }
                    else
                    {
                        span.Add(item);
                    }

                    // Remaining selectors
                    foreach (var c in ancestors.Skip(i + 1))
                    {
                        if (c.Contains(NodeKind.Reference))
                        {
                            span = SetReference(c[0], span);
                        }
                        else
                        {
                            span.Add(c[0]);
                        }
                    }

                    result.Add(span);
                }

                span = null;

                break;
            }
            else
            {
                span.Add(ancestor[0]);
            }
        }

        if (span is not null)
        {
            result.Add(span);
        }

        return new CssSelector(result);
    }

    private static CssSequence SetReference(CssSequence current, CssSequence parent)
    {
        var prev = parent;

        var span = new CssSequence(current.Count);

        foreach (var node in current)
        {
            span.Add(node.Kind is NodeKind.Reference ? new CssReference("&", prev) : node);
        }

        return span;
    }

    public void Dispose()
    {
        _writer.Dispose();
    }

    #endregion
}
