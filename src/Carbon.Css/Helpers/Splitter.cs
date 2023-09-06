using Carbon.Css.Helpers;

namespace Carbon.Css;

internal ref struct StringSplitter
{
    private readonly ReadOnlySpan<char> _text;
    private readonly char _separator;
    private int _position;

    public StringSplitter(ReadOnlySpan<char> text, char separator)
    {
        _text = text;
        _separator = separator;
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

        int separatorIndex = _text[_position..].IndexOf(_separator);

        if (separatorIndex > -1)
        {
            _position += separatorIndex + 1;

            result = _text.Slice(start, separatorIndex);
        }
        else
        {
            _position = _text.Length;

            result = _text[start..];
        }

        return true;
    }

    public bool TryGetNextF32(out float result)
    {
        if (TryGetNext(out var segment))
        {
            result = NumberHelper.ParseCssNumberAsF32(segment);

            return true;
        }
        else
        {
            result = default;

            return false;
        }
    }

    public readonly char Current => _text[_position];

    public void ReadWhitespace()
    {
        while (_position < _text.Length && Current is ' ')
        {
            _position++;
        }
    }

    public bool IsEof => _position >= _text.Length;
}