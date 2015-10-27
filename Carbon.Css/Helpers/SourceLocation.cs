namespace Carbon.Css.Helpers
{
    public struct SourceLocation
    {
        public SourceLocation(int position, int line, int column)
        {
            Position = position;
            Line = line;
            Column = column;
        }

        // 0 based
        public int Position { get; }

        // 1 based
        public int Line { get; }

        // 1 based
        public int Column { get; }

        public override string ToString() => $"({Line},{Column})";
    }
}
