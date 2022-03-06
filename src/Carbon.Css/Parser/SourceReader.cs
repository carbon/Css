using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Carbon.Css.Parser;

internal sealed class SourceReader : IDisposable
{
    private const char EofChar = '\0';

    private readonly TextReader textReader;
    private char _current;
    private int _position;

    private readonly StringBuilder sb;

    private int markStart;
    private int marked;

    public SourceReader(TextReader textReader)
    {
        this.textReader = textReader;

        _position = 0;
        _current = '.';
        sb = new StringBuilder();
        markStart = -1;
        marked = -1;
    }

    public char Current => _current;

    public bool IsEof => _current == EofChar;

    public int Position => _position;

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
        char c = _current;

        Next();

        return c;
    }

    [SkipLocalsInit]
    public string Read(int count)
    {
        Span<char> buffer = count <= 16
            ? stackalloc char[16]
            : new char[count];

        for (int i = 0; i < count; i++)
        {
            buffer[i] = _current;

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

        if (marked != -1 && (marked <= _position))
        {
            sb.Append(_current);
        }

        int charCode = textReader.Read(); // -1 if there are no more chars to read (e.g. stream has ended)

        _current = (charCode > 0) ? (char)charCode : EofChar;

        _position++;

        return _current;
    }

    #region Mark

    public int MarkStart => markStart;

    public int Mark(bool appendCurrent = true)
    {
        markStart = _position;
        marked = _position;

        if (appendCurrent == false)
        {
            marked++;
        }

        return _position;

    }

    public string Unmark()
    {
        marked = -1;

        var value = sb.ToString();

        sb.Clear();

        return value;
    }

    #endregion

    #region IDisposable

    public void Dispose()
    {
        textReader.Dispose();
    }

    #endregion
}
