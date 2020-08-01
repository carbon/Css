namespace Carbon.Css.Helpers
{
    public readonly struct LineInfo
    {
        public LineInfo(int number, string text)
        {
            Number = number;
            Text = text;
        }

        public readonly int Number { get; }

        public readonly string Text { get; }
    }
}