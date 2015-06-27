using System;
using System.IO;

namespace Carbon.Css.Parser
{
	public class SourceReader : IDisposable
	{
		public const char EofChar = '\0';

		private readonly TextReader textReader;
		private char current;
		private int position;

		public SourceReader(string text)
		{
			this.textReader = new StringReader(text);

			current = '.';
		}

		public SourceReader(TextReader textReader) 
		{
			this.textReader = textReader;

			current = '.';
		}

		public char Current => current;

		public bool IsEof => current == EofChar;

		public bool IsWhiteSpace => Char.IsWhiteSpace(current);

		public int Position => position;

		public char Peek()
		{
			int charCode = textReader.Peek();

			return (charCode > 0) ? (char)charCode : EofChar;
		}

		/// <summary>
		/// Returns the current character and advances to the next
		/// </summary>
		/// <returns></returns>
		public char Read()
		{
			var c = current;

			Next();

			return c;
		}

		public string Read(int count)
		{
			var buffer = new char[count];

			for (int i = 0; i < count; i++)
			{
				buffer[i] = current;

				Next();
			}

			return new string(buffer);
		}

		/// <summary>
		/// Advances to the next character and returns it
		/// </summary>
		public char Next() 
		{
			if (IsEof) throw new Exception("Cannot read past EOF.");

			if (marked != -1 && (marked <= this.position))
			{
				buffer.Append(current);
			}

			int charCode = textReader.Read(); // -1 if there are no more chars to read (e.g. stream has ended)

			if (charCode > 0)
			{
				this.current = (char)charCode;
			}
			else
			{		
				this.current = EofChar;
			}

			position++;

			return current;
		}

		#region Mark

		private readonly StringBuffer buffer = new StringBuffer();

		private int markStart = -1;
		private int marked = -1;

		public int MarkStart
		{
			get { return markStart; }
		}
	
		public int Mark(bool appendCurrent = true)
		{
			markStart = this.position;
			marked = this.position;

			if (appendCurrent == false)
			{
				marked++;
			}

			return this.position;
	
		}

		public string Unmark()
		{
			marked = -1;

			return buffer.Extract();
		}

		#endregion

		#region IDisposable

		private bool isDisposed = false;

		public void Dispose()
		{
			if (isDisposed) return;

			textReader.Close();

			isDisposed = true;
		}

		#endregion
	}
}
