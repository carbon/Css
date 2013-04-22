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

		public bool HasDeclarations
		{
			get { return declarations.Count > 0; }
		}

		public IList<CssDeclaration> Declarations
		{
			get { return declarations; }
		}

		public IEnumerable<CssDeclaration> FindHavingPropertyName(string propertyName)
		{
			return declarations.Where(d => d.Name == propertyName);
		}

		public CssDeclaration Get(string name)
		{
			return declarations.FirstOrDefault(d => d.Name == name);
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
	}
}

// A block starts with a left curly brace ({) and ends with the matching right curly brace (}).
// In between there may be any tokens, except that parentheses (( )), brackets ([ ]), and braces ({ }) must always occur in matching pairs and may be nested.
// Single (') and double quotes (") must also occur in matching pairs, and characters between them are parsed as a string. See Tokenization above for the definition of a string.