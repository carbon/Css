namespace Carbon.Css;

internal ref struct Tuple4
{
    public ReadOnlySpan<char> _0 { get; private set; }

    public ReadOnlySpan<char> _1 { get; private set; }

    public ReadOnlySpan<char> _2 { get; private set; }

    public ReadOnlySpan<char> _3 { get; private set; }

    public ReadOnlySpan<char> this[int i]
    {
        get
        {
            return i switch
            {
                0 => _0,
                1 => _1,
                2 => _2,
                3 => _3,
                _ => throw new IndexOutOfRangeException()
            };
        }
    }

    public int Length { get; private set; }

    public static Tuple4 Parse(ReadOnlySpan<char> d)
    {
        var result = new Tuple4();

        var splitter = new StringSplitter(d, ' ');
        
        while (splitter.TryGetNext(out var component))
        {
            switch (result.Length)
            {
                case 0: result._0 = component; break;
                case 1: result._1 = component; break;
                case 2: result._2 = component; break;
                case 3: result._3 = component; break;
            }

            result.Length++;
        }

        return result;
    }
}