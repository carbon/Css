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
		public int Position
		{
			get { return position; }
		}

		// 1 based
		public int Line
		{
			get { return line; }
		}

		// 1 based
		public int Column
		{
			get { return column; }
		}

		public SourceLocation GetLocation()
		{
			return new SourceLocation(this.position, this.line, this.column);
		}

		public override string ToString()
		{
			return "(" + line + "," + column + ")";
		}
	}
}
