namespace Carbon.Css.Parser
{
	using System;
	using System.Globalization;
	using System.Text;
	using System.IO;

	public class SourceReader : IDisposable
	{
		public const char EofChar = '\0';

		private readonly TextReader textReader;
		private char currentCharacter;
		private bool isDisposed = false;
		private bool isEof = false;
		private int position;

		public SourceReader(string text)
			: this(new StringReader(text)) { }

		public SourceReader(TextReader textReader) 
		{
			this.textReader = textReader;
		}

		public char Current 
		{
			get { return currentCharacter; }
		}

		public bool IsEof 
		{
			get { return isEof; }
		}

		public int Position
		{
			get { return position; }
		}

		public char Peek()
		{
			int charCode = textReader.Peek();

			return (charCode > 0) ? (char)charCode : EofChar;
		}

		/// <summary>
		/// Advances to the next character
		/// </summary>
		public void Next() 
		{
			if (marked != -1 && (marked <= this.position) && !isEof)
			{
				buffer.Append(currentCharacter);
			}

			int charCode = textReader.Read(); // -1 if there are no more chars to read (e.g. stream has ended)

			if (charCode > 0)
			{
				this.currentCharacter = (char)charCode;
			}
			else
			{
				isEof = true;
			
				this.currentCharacter = EofChar;
			}

			position++;
		}

		public void SkipWhitespace() 
		{
			while (Char.IsWhiteSpace(currentCharacter)) 
			{
				Next();
			}
		}

		#region Mark

		private readonly StringBuffer buffer = new StringBuffer();

		private int marked = -1;

		public void Mark(bool appendCurrent = true)
		{
			marked = this.position;

			if (appendCurrent == false)
				marked++;
	
		}

		public string Unmark()
		{
			marked = -1;

			return buffer.Extract();
		}

		#endregion

		#region IDisposable

		public void Dispose()
		{
			if (isDisposed) return;

			textReader.Close();

			isDisposed = true;
		}

		#endregion
	}
}
