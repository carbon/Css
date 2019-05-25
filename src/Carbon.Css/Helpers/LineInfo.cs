namespace Carbon.Css.Helpers
{
    public readonly struct LineInfo
    {
        public LineInfo(int number, string text)
        {
            Number = number;
            Text = text;
        }

        public int Number { get; }

        public string Text { get; }
    }
}