using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;

using Carbon.Css.Parser;

namespace Carbon.Css
{
    public abstract class CssValue : CssNode
    {
        public CssValue(NodeKind kind)
            : base(kind)
        { }

        public static CssUnitValue Number(double value)
        {
            return new CssUnitValue(value, CssUnitInfo.Number);
        }

        public static CssValue Parse(string text)
        {
            if (text.Length == 0)
            {
                throw new ArgumentException("Must not be empty", nameof(text));
            }

            if (char.IsDigit(text[0]) && TryParseNumberOrMeasurement(text, out CssUnitValue? value))
            {
                return value;
            }

            var reader = new SourceReader(new StringReader(text));

            using var tokenizer = new CssTokenizer(reader, LexicalMode.Value);

            var parser = new CssParser(tokenizer);

            return parser.ReadValueList();
        }

        // 60px
        // 6.5em

        private static bool TryParseNumberOrMeasurement(string text, [NotNullWhen(true)] out CssUnitValue? value)
        {
            int unitIndex = -1;

            char point;

            for (int i = 0; i < text.Length; i++)
            {
                point = text[i];

                if (point is ' ' or ',')
                {
                    value = null;
                    return false;
                }

                if (char.IsNumber(point) || point == '.')
                {
                }
                else if (unitIndex == -1)
                {
                    unitIndex = i;
                }
            }

            value = (unitIndex > 0)
                ? new CssUnitValue(ParseDouble(text.AsSpan(0, unitIndex)), CssUnitNames.Get(text.AsSpan(unitIndex)))
                : new CssUnitValue(double.Parse(text, CultureInfo.InvariantCulture), CssUnitInfo.Number);

            return true;
        }

        private static double ParseDouble(ReadOnlySpan<char> text)
        {
#if NETSTANDARD2_0
            return double.Parse(text.ToString(), CultureInfo.InvariantCulture);
#else
            return double.Parse(text, provider: CultureInfo.InvariantCulture);
#endif
        }

        public static CssValue FromComponents(IEnumerable<CssValue> components)
        {
            // A property value can have one or more components.
            // Components are seperated by a space & may include functions, literals, dimensions, etc

            var enumerator = components.GetEnumerator();

            enumerator.MoveNext();

            var first = enumerator.Current;

            if (!enumerator.MoveNext())
            {
                return first;
            }

            var values = new List<CssValue>
            {
                first,
                enumerator.Current
            };

            while (enumerator.MoveNext())
            {
                values.Add(enumerator.Current);
            }

            return new CssValueList(values, ValueSeperator.Space);
        }

        public static bool AreCompatible(CssValue left, CssValue right, BinaryOperator operation)
        {
            return operation switch
            {
                BinaryOperator.Divide                        => false,
                BinaryOperator.Add or BinaryOperator.Subtract => left.Kind == right.Kind,
                BinaryOperator.Multiply => 
                        left.Kind == right.Kind ||
                        left.Kind == NodeKind.Percentage ||
                        right.Kind == NodeKind.Percentage ||
                        left.Kind == NodeKind.Number ||
                        right.Kind == NodeKind.Number,
                BinaryOperator.Mod => right.Kind == NodeKind.Number,
                _                  => true
            };
        }

        internal virtual void WriteTo(TextWriter writer)
        {
            writer.Write(ToString());
        }
    }
}