namespace Carbon.Css
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.IO;

	public class CssBlock : ICollection<CssDeclaration>
	{
		private readonly List<CssDeclaration> declarations = new List<CssDeclaration>();
		private readonly List<CssRule> rules = new List<CssRule>();

		public IList<CssDeclaration> Declarations
		{
			get { return declarations; }
		}


		public IList<CssDeclaration> FindHavingProperty(CssPropertyInfo property)
		{
			return declarations.Where(d => d.Name == property.Name).ToList();
		}

		// Nested rules
		// { to: { opacity: 1 } }
		public IList<CssRule> Rules
		{
			get { return rules; }
		}

		public int IndexOf(CssDeclaration declaration)
		{
			return declarations.IndexOf(declaration);
		}

		#region ICollection<CssDeclaration> Members

		public void Add(CssDeclaration item)
		{
			declarations.Add(item);
		}

		void ICollection<CssDeclaration>.Clear()
		{
			declarations.Clear();
		}

		bool ICollection<CssDeclaration>.Contains(CssDeclaration item)
		{
			return declarations.Contains(item);
		}

		void ICollection<CssDeclaration>.CopyTo(CssDeclaration[] array, int arrayIndex)
		{
			declarations.CopyTo(array, arrayIndex);
		}

		int ICollection<CssDeclaration>.Count
		{
			get { return declarations.Count; }
		}

		bool ICollection<CssDeclaration>.IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(CssDeclaration item)
		{
			return declarations.Remove(item);
		}

		IEnumerator<CssDeclaration> IEnumerable<CssDeclaration>.GetEnumerator()
		{
			return declarations.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return declarations.GetEnumerator();
		}

		#endregion

		public void WriteTo(TextWriter writer)
		{
			writer.Write("{");

			// Write the declarations
			foreach (var d in declarations)
			{
				if (declarations.Count > 1)
				{
					writer.WriteLine();

					writer.Write(" ");
				}

				writer.Write(string.Format(" {0}: {1};", d.Name, d.Value.ToString()));

				if (declarations.Count == 1)
				{
					writer.Write(" ");
				}
			}

			if (declarations.Count > 1)
			{
				writer.WriteLine();
			}

			// Write the nested rules
			foreach (var b in rules)
			{
				writer.WriteLine("  ");

				b.WriteTo(writer);
			}

			writer.Write("}");
		}
	}
}

// A block starts with a left curly brace ({) and ends with the matching right curly brace (}).
// In between there may be any tokens, except that parentheses (( )), brackets ([ ]), and braces ({ }) must always occur in matching pairs and may be nested.
// Single (') and double quotes (") must also occur in matching pairs, and characters between them are parsed as a string. See Tokenization above for the definition of a string.


