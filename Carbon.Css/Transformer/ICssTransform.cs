namespace Carbon.Css
{
	using System.Linq;

	public interface ICssTransformer
	{
		void Transform(CssRule rule, int index);

		int Order { get; }
	}
}