namespace Carbon.Css.Ast;

public interface ICssNumericValue
{
    // angle, flex, frequency, length, resolution, percent, percentHint, or time
    // public NodeKind Type { get; }

    public double Value { get; }

    // to Converts value into another one with the specified unit.

}

// https://drafts.css-houdini.org/css-typed-om/#dom-cssnumericvalue-type