namespace Carbon.Css
{
	using System.IO;

	public interface IStylesheet
	{
		// DateTime Modified { get; }
		
		void WriteTo(TextWriter writer);
	}
}