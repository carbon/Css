namespace Carbon.Css.Selectors;

public enum CombinatorType : int
{
    None              = '\0',
    Descendant        = ' ',  // ' '
    Child             = '>',  // '>'
    AdjacentSibling   = '+',  // '+' | DirectAdjacent
    SubsequentSibling = '~',  // '~' | IndirectAdjacent
}