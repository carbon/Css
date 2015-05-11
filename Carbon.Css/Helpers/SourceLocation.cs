namespace Carbon.Css.Helpers
{
	public struct SourceLocation
	{
		private readonly int position;
		private readonly int line;
		private readonly int column;

		public SourceLocation(int position, int line, int column)
		{
			this.position = position;
			this.line = line;
			this.column = column;
		}

		// 0 based
		public int Position => position;

		// 1 based
		public int Line => line;

		// 1 based
		public int Column => column;

		public override string ToString() => $"({line},{column})";
	}
}
