namespace Carbon.Css
{
	using Carbon.Css.Parser;
	using System;
	using System.IO;

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

		public string Name
		{
			get { return name; }
		}

		public CssValue Value
		{
			get { return value; }
		}

		public override string Text
		{
			get { return ""; }
		}

		public override CssNode Clone()
		{
			throw new NotImplementedException();
		}
	}
}