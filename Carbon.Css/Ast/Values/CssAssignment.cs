namespace Carbon.Css
{
	using Parser;

	public class CssAssignment : CssNode
	{
		private readonly string name;
		private readonly CssValue value;

		public CssAssignment(string name, CssValue value)
			: base(NodeKind.Assignment)
		{
			this.name = name;
			this.value = value;
		}

		public CssAssignment(CssToken name, CssValue value)
			: base(NodeKind.Assignment)
		{
			this.name = name.Text;
			this.value = value;
		}

		public string Name => name;

		public CssValue Value => value;
	}
}