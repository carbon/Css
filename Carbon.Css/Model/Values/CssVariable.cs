namespace Carbon.Css
{
	using Carbon.Css.Parser;
	using System.IO;

	public class VariableAssignment : INode
	{
		private readonly string name;
		private readonly CssValue value;

		public VariableAssignment(string name, CssValue value)
		{
			this.name = name;
			this.value = value;
		}

		public VariableAssignment(CssToken name, CssValue value)
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

		#region Node

		NodeKind INode.Kind
		{
			get { return NodeKind.Variable; }
		}

		void INode.WriteTo(TextWriter writer, int level, CssContext context)
		{
			return;
		}

		#endregion
	}
}