namespace Carbon.Css
{
	public enum UnaryOperator
	{
		Not // !
	}

    public enum BinaryOperator
	{
        // Logical
        And		  = 30, // && 
        Or		  = 31,	// ||
        
        // Equality
        Equals	  = 40,	// ==
        NotEquals = 41,	// !=      (<>)
        
        // Relational
        Gt		  = 50,	// > 
        Gte		  = 51, // >=
        Lt		  = 52, // <
        Lte		  = 53, // <=
        
        // Math
        Divide	  = 60, // /
        Multiply  = 61, // *
        Add		  = 62,	// +
        Subtract  = 62,	// -
        Mod		  = 63  // %
	}
}
