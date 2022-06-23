// Based on .NET Source code

using System;

namespace Carbon.Css;

internal ref struct Splitter
{
    private readonly ReadOnlySpan<char> _text;
    private readonly char _seperator;
    private int _position;

    public Splitter(ReadOnlySpan<char> text, char seperator)
    {
        _text = text;
        _seperator = seperator;
        _position = 0;
    }

    public bool TryGetNext(out ReadOnlySpan<char> result)
    {
        if (IsEof)
        {
            result = default;

            return false;
        }

        int start = _position;

        int seperatorIndex = _text[_position..].IndexOf(_seperator);

        if (seperatorIndex > -1)
        {
            _position += seperatorIndex + 1;

            result = _text.Slice(start, seperatorIndex);
        }
        else
        {
            _position = _text.Length;

            result = _text[start..];
        }

        return true;
    }

    public char Current => _text[_position];

    public void ReadWhitespace()
    {
        while (_position < _text.Length && Current == ' ')
        {
            _position++;
        }
    }

    public bool IsEof => _position >= _text.Length;
}