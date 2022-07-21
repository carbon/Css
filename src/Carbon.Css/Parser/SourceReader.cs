using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Carbon.Css.Parser;

internal sealed class SourceReader : IDisposable
{
    private const char EofChar = '\0';

    private readonly TextReader _textReader;
    private char _current;
    private int _position;

    private readonly StringBuilder sb;

    private int _markStart;
    private int _marked;

    public SourceReader(TextReader textReader)
    {
        _textReader = textReader;
        _position = 0;
        _current = '.';
        sb = new StringBuilder();
        _markStart = -1;
        _marked = -1;
    }

    public char Current => _current;

    public bool IsEof => _current is EofChar;

    public int Position => _position;

    public char Peek()
    {
        int charCode = _textReader.Peek();

        return (charCode > 0) ? (char)charCode : EofChar;
    }

    /// <summary>
    /// Returns the current character and advances to the next
    /// </summary>
    /// <returns></returns>
    public char Read()
    {
        char c = _current;

        Advance();

        return c;
    }

    /// <summary>
    /// Returns the current character and advances to the next
    /// </summary>
    /// <returns></returns>
    internal string ReadSymbol(string result)
    {
        Advance();

        return result;
    }

    [SkipLocalsInit]
    public string Read(int count)
    {
        Span<char> buffer = count <= 8
            ? stackalloc char[8]
            : new char[count];

        for (int i = 0; i < count; i++)
        {
            buffer[i] = _current;

            Advance();
        }

        return new string(buffer[..2]);
    }

    /// <summary>
    /// Advances to the next character and returns it
    /// </summary>
    public void Advance()
    {
        if (IsEof) throw new Exception("Cannot read past EOF.");

        if (_marked != -1 && (_marked <= _position))
        {
            sb.Append(_current);
        }

        int charCode = _textReader.Read(); // -1 if there are no more chars to read (e.g. stream has ended)

        _current = (charCode > 0) ? (char)charCode : EofChar;

        _position++;
    }

    #region Mark

    public int MarkStart => _markStart;

    public int Mark(bool appendCurrent = true)
    {
        _markStart = _position;
        _marked = _position;

        if (!appendCurrent)
        {
            _marked++;
        }

        return _position;

    }

    public string Unmark()
    {
        _marked = -1;

        var value = sb.ToString();

        sb.Clear();

        return value;
    }

    #endregion

    #region IDisposable

    public void Dispose()
    {
        _textReader.Dispose();
    }

    #endregion
}
