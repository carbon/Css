namespace Carbon.Css
{
	public enum NodeKind : ushort
	{
		Unknown = 0,

		Document,
		Comment,
		Rule, // Ruleset
		Expression,
		Declaration,

		Block,

		Function,

		Selector,
		

		// Rules
		/*
		StyleRule,
		ImportRule,
		AnimationRule,
		*/

		// Values
		Assignment,
		Variable,
		PrimitiveValue, //?
		ValueList,
		Url,
		Number,
		String,
		Color,

		// Dimensions
		Frequency,
		Time,
		Angle,
		Length,
		EMS,
		EXS,
		Percent,
		Dimension,


		// Sass Extensions
		Mixin,
		Include
	}
}


/*
stylesheet  : [ CDO | CDC | S | statement ]*;
statement   : ruleset | at-rule;
at-rule     : ATKEYWORD S* any* [ block | ';' S* ];
block       : '{' S* [ any | block | ATKEYWORD S* | ';' S* ]* '}' S*;
ruleset     : selector? '{' S* declaration? [ ';' S* declaration? ]* '}' S*;
selector    : any+;
declaration : property S* ':' S* value;
property    : IDENT;
value       : [ any | block | ATKEYWORD S* ]+;
any         : [ IDENT | NUMBER | PERCENTAGE | DIMENSION | STRING
              | DELIM | URI | HASH | UNICODE-RANGE | INCLUDES
              | DASHMATCH | ':' | FUNCTION S* [any|unused]* ')'
              | '(' S* [any|unused]* ')' | '[' S* [any|unused]* ']'
              ] S*;
unused      : block | ATKEYWORD S* | ';' S* | CDO S* | CDC S*;
*/