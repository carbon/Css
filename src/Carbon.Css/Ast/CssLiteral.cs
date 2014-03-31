namespace Carbon.Css.Ast
{
	public class CssLiteral
	{
	}
}

/*
math    : calc S*;
calc    : "calc(" S* sum S* ")";
sum     : product [ S+ [ "+" | "-" ] S+ product ]*;
product : unit [ S* [ "*" S* unit | "/" S* NUMBER ] ]*;
attr    : "attr(" S* qname [ S+ type-keyword ]? S* [ "," [ unit | calc ] S* ]? ")";
unit    : [ NUMBER | DIMENSION | PERCENTAGE | "(" S* sum S* ")" | calc | attr ];
*/