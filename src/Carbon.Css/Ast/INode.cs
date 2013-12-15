namespace Carbon.Css
{
	using System.IO;

	public interface INode
	{
		NodeKind Kind { get; }

		void WriteTo(TextWriter writer, int level, CssContext context);
	}
}