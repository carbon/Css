namespace Carbon.Css
{
	public enum NodeKind
	{
		Document,
		Comment,
		Rule,
		Expression,
		Declaration,

		Block,

		Function,

		Selector,
		
		// Values
		Variable,
		Identifier,
		PrimitiveValue, //?
		ValueList,
		Dimension,
		Url,
		Literal	// StringLiteral & NumberLiteral

	}
}
