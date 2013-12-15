namespace Carbon.Css
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class CssPrimitiveValue : CssValue
	{
		private string text;

		public CssPrimitiveValue(string text)
		{
			this.text = text.Trim();

			switch (text[0])
			{
				case '$': this.type = CssValueType.Variable;									 break;
				case 'u': if (text[1] == 'r' && text[2] == 'l') this.type = CssValueType.Url;	 break;
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

		public override string ToString()
		{
			return text;
		}
	}

	public class CssValueList : CssValue, IEnumerable<CssValue>
	{
		private readonly List<CssValue> values = new List<CssValue>();
		private bool spaceSeperated = false;

		public CssValueList(bool spaceSeperated = true)
		{
			this.spaceSeperated = spaceSeperated; // Otherwise, comma seperated

			this.type = CssValueType.ValueList;
		}

		public List<CssValue> Values
		{
			get { return values; }
		}

		public override string ToString()
		{
			return string.Join(spaceSeperated ? " " : ", ", values.Select(t => t.ToString()));
		}

		public override IEnumerator<CssValue> GetEnumerator()
		{
			return values.GetEnumerator();
		}
	}

	// Simple or Complex
	public abstract class CssValue : IEnumerable<CssValue>
	{
		protected CssValueType type = CssValueType.Unknown;

		public CssValueType Type
		{
			get { return type; }
		}

		public static CssValue Parse(string text)
		{
			#region Preconditions

			if (text == null) throw new ArgumentNullException("text");

			#endregion

			var majorParts = text.Split(',');

			if (majorParts.Length == 1)
			{
				var minorParts = majorParts[0].Split(' ');

				if (minorParts.Length == 1)
				{
					return new CssPrimitiveValue(minorParts[0].Trim());
				}

				var list = new CssValueList(true);

				foreach (var value in minorParts)
				{
					if (value.Length == 0) continue;

					list.Values.Add(new CssPrimitiveValue(value));
				}

				return list;
			}
			else
			{
				// More than 1 comma seperated list

				var list = new CssValueList();

				foreach (var part in majorParts)
				{
					list.Values.Add(CssValue.Parse(part));
				}

				return list;
			}
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
