using System;
using System.IO;
using System.Text;

namespace Carbon.Css.Parser
{
    public sealed class SourceReader : IDisposable
    {
        private const char EofChar = '\0';

        private readonly TextReader textReader;
        private char current;
        private int position;

        private readonly StringBuilder sb;

        private int markStart;
        private int marked;

        public SourceReader(TextReader textReader)
        {
            this.textReader = textReader;

            position = 0;
            current = '.';
            sb = new StringBuilder();
            markStart = -1;
            marked = -1;
        }

        public char Current => current;

        public bool IsEof => current == EofChar;
        
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
            char c = current;

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
                sb.Append(current);
            }

            int charCode = textReader.Read(); // -1 if there are no more chars to read (e.g. stream has ended)

            this.current = (charCode > 0) ? (char)charCode : EofChar;

            position++;

            return current;
        }

        #region Mark

        public int MarkStart => markStart;

        public int Mark(bool appendCurrent = true)
        {
            markStart = position;
            marked = position;

            if (appendCurrent == false)
            {
                marked++;
            }

            return position;

        }

        public string Unmark()
        {
            marked = -1;

            return sb.Extract();
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            textReader.Dispose();
        }

        #endregion
    }
}
