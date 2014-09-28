namespace Carbon.Css
{
	using System.Collections.Generic;

	public interface ICssFunction
	{
		CssValue Execute(CssValue[] args);
	}
}
