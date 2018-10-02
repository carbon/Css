using System;
using System.Collections.Generic;
using System.IO;

namespace Carbon.Css
{
    using Parser;

    public abstract class CssValue : CssNode
    {
        public CssValue(NodeKind kind)
            : base(kind)
        { }

        public static CssUnitValue Number(double value) => new CssUnitValue(value, CssUnitInfo.Number);

        public static CssValue Parse(string text)
        {
            if (text.Length == 0)
            {
                throw new ArgumentException("Must not be empty", nameof(text));
            }

            if (char.IsDigit(text[0]) && TryParseNumberOrMeasurement(text, out var value))
            {
                return value;
            }

            var reader = new SourceReader(new StringReader(text));

            using (var tokenizer = new CssTokenizer(reader, LexicalMode.Value))
            { 
                var parser = new CssParser(tokenizer);
                
                return parser.ReadValueList();
            }
        }

        // 60px
        // 6.5em

        private static bool TryParseNumberOrMeasurement(string text, out CssValue value)
        {
            int unitIndex = -1;

            char point;

            for (int i = 0; i < text.Length; i++)
            {
                point = text[i];

                if (point == ' ' || point == ',')
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

            if (unitIndex > 0)
            {
                value = new CssUnitValue(double.Parse(text.Substring(0, unitIndex)), text.Substring(unitIndex));
            }
            else
            {
                value = CssValue.Number(double.Parse(text));
            }

            return true;
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

            var values = new List<CssValue>();

            values.Add(first);
            values.Add(enumerator.Current);

            while (enumerator.MoveNext())
            {
                values.Add(enumerator.Current);
            }

            return new CssValueList(values, ValueSeperator.Space);
        }


        public static bool AreCompatible(CssValue left, CssValue right, BinaryOperator operation)
        {
            switch (operation)
            {
                case BinaryOperator.Divide: return false;
                case BinaryOperator.Add:
                case BinaryOperator.Subtract:
                    return left.Kind == right.Kind;
                case BinaryOperator.Multiply:
                    return
                        left.Kind == right.Kind ||
                        left.Kind == NodeKind.Percentage ||
                        right.Kind == NodeKind.Percentage ||
                        left.Kind == NodeKind.Number ||
                        right.Kind == NodeKind.Number;
                case BinaryOperator.Mod:
                    return right.Kind == NodeKind.Number;
            }

            return true;
        }
    }
}