namespace Carbon.Css
{
	/*
	// Inherit
	PrimitiveValue = 1,
	ValueList = 2
	// Custom,
	// Initial
	*/

	// TODO: Break put into CssValueType & PrimitiveValueType
	public enum CssValueType
	{
		Unknown = 0,

		Variable = 10,

		ValueList = 2,

		String = 3,

		// Primitives

		ATTR,

		Number,
		Percentage,
		Url,

		EMS,
		EXS,
		PX,
		CM,
		MM,
		IN,
		PT,
		PC,
		DEG,
		RAD,
		GRAD,
		TURN,

		// Time
		MS,
		S,

		// Frequency
		HZ,
		KHZ
	}
}
