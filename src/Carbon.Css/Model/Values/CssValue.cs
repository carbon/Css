namespace Carbon.Css
{
	using Carbon.Css.Parser;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;

	public class CssPrimitiveValue : CssValue
	{
		private string text;

		public CssPrimitiveValue(string text, CssValueType type = CssValueType.Unknown)
		{
			this.type = type;
			this.text = text;
		}

		public string Text
		{
			get { return text; }
		}

		public void SetText(string text)
		{
			this.text = text;

			// TODO: Reparse
		}

		public IEnumerable<CssValue> Enumerable
		{
			get
			{
				yield return this;
			}
		}

		public override IEnumerator<CssValue> GetEnumerator()
		{
			return Enumerable.GetEnumerator();
		}


		public new static CssPrimitiveValue Parse(string text)
		{
			var type = CssValueType.Unknown;
			
			text = text.Trim();

			if (text.Length > 3)
			{
				switch (text[0])
				{
					case '$': type = CssValueType.Variable; break;
					case 'u': if (text[1] == 'r' && text[2] == 'l') type = CssValueType.Url; break;
					case '0':
					case '1':
					case '2':
					case '3':
					case '4':
					case '5':
					case '7':
					case '8':
					case '9': break; // Number (px,em,deg,s,ms,%)
				}
			}


			return new CssPrimitiveValue(text, type);

		}

		public override string ToString()
		{
			return text;
		}
	}

	public enum Seperator
	{
		Comma,
		Space
	}

	public class CssValueList : CssValue, IEnumerable<CssValue>
	{
		private readonly List<CssPrimitiveValue> values;
		private Seperator seperator;

		public CssValueList(List<CssPrimitiveValue> values, Seperator seperator = Seperator.Comma)
		{
			this.values = values;
			this.seperator = seperator;
		}

		public CssValueList(Seperator seperator = Seperator.Space)
		{
			this.seperator = seperator; // Otherwise, comma seperated

			this.type = CssValueType.ValueList;

			this.values = new List<CssPrimitiveValue>();
		}

		public List<CssPrimitiveValue> Values
		{
			get { return values; }
		}

		public override string ToString()
		{
			return string.Join(seperator == Seperator.Space ? " " : ", ", values.Select(t => t.ToString()));
		}

		public override IEnumerator<CssValue> GetEnumerator()
		{
			return values.GetEnumerator();
		}
	}

	// Simple or Complex
	public abstract class CssValue : CssNode, IEnumerable<CssValue>
	{
		protected CssValueType type = CssValueType.Unknown;

		public CssValue() 
			: base(NodeKind.Value) { }

			public CssValueType Type
		{
			get { return type; }
		}

		public static CssValue Parse(TokenList tokens)
		{
			var values = tokens.ToValues().ToList();

			if (values.Count == 1) return values[0];

			return new CssValueList(values, Seperator.Comma);
		}

		public static CssValue Parse(string text)
		{
			#region Preconditions

			if (text == null) throw new ArgumentNullException("text");

			#endregion

			var reader = new SourceReader(new StringReader(text));

			var tokenizer = new CssTokenizer(reader, LexicalMode.Value);

			var tokens = new TokenList();

			while (!tokenizer.IsEnd)
			{
				tokens.Add(tokenizer.Next());

				if (tokenizer.IsEnd) break;
			}

			return Parse(tokens);
			
		}

		public abstract IEnumerator<CssValue> GetEnumerator();

		#region IEnumerable<CssValue> Members

		IEnumerator<CssValue> IEnumerable<CssValue>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}
