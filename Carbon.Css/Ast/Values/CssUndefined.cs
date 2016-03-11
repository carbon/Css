namespace Carbon.Css
{
    public class CssUndefined : CssValue
    {
        public CssUndefined(string variableName)
            : base(NodeKind.Undefined)
        {
            VariableName = variableName;
        }

        public string VariableName { get; }

        public override CssNode CloneNode() => new CssUndefined(VariableName);

        public override string ToString() => $"/* {VariableName} undefined */";
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
