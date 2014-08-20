namespace Carbon.Css
{
	public class Directive
	{
		public DirectiveType Type { get; set; }

		public string Value { get; set; }
	}

	public enum DirectiveType
	{
		Patch = 1
	}
}

/*

//= patch IE >= 11
//= patch Safari >= 5.1

*/