namespace Carbon.Css
{
	using Carbon.Css.Parser;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	// Component Values 
	// Comma seperated list of a component values

	public class CssValueList : CssValue, IEnumerable<CssValue>
	{
		private readonly IList<CssValue> values;
		private ValueListSeperator seperator;

		public CssValueList(IList<CssValue> values, ValueListSeperator seperator = ValueListSeperator.Comma)
			: base(NodeKind.ValueList)
		{
			this.values		= values;
			this.seperator	= seperator;
		}

		public override string Text
		{
			get { return ToString(); }
		}

		public override string ToString()
		{
			return string.Join(seperator == ValueListSeperator.Space ? " " : ", ", values.Select(t => t.ToString()));
		}

		public override IList<CssValue> ToList()
		{
			return values;
		}

		#region IEnumerator

		IEnumerator<CssValue> IEnumerable<CssValue>.GetEnumerator()
		{
			return values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return values.GetEnumerator();
		}

		#endregion
	}

	public enum ValueListSeperator
	{
		Comma,
		Space
	}

	
	// Single value
	public abstract class CssValue : CssNode
	{
		public CssValue(NodeKind kind)
			: base(kind) { }

		public virtual IList<CssValue> ToList()
		{
			return new[] { this };
		}

		public static CssValue Parse(string text)
		{
			#region Preconditions

			if (text == null) throw new ArgumentNullException("text");

			#endregion

			var reader = new SourceReader(new StringReader(text));

			var tokenizer = new CssTokenizer(reader, LexicalMode.Value);

			var parser = new CssParser(tokenizer);

			return parser.ReadValue();			
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

			var list = new List<CssValue>();

			list.Add(first);
			list.Add(enumerator.Current);

			while (enumerator.MoveNext())
			{
				list.Add(enumerator.Current);
			}

			return new CssValueList(list, ValueListSeperator.Space);
		}
	}
}
