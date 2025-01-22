using Carbon.Css.Selectors;

namespace Carbon.Css.Parser;

public sealed partial class CssParser : IDisposable
{
    internal SelectorList ReadSelectorList()
    {
        var list = new SelectorList();

        list.Add(ReadNewSelector());

        while (ConsumeIf(CssTokenKind.Comma))
        {
            ReadTrivia();

            if (!IsEnd)
            {
                list.Add(ReadNewSelector());
            }
        }

        return list;
    }

    internal Selector ReadNewSelector()
    {
        var token = Consume();

        Selector selector;

        if (token.Kind is CssTokenKind.Name)
        {
            switch (token.Text[0])
            {
                case '#':
                    selector = new IdSelector { Text = token.Text };
                    break;
                case '.':
                    selector = new ClassSelector { Text = token.Text };
                    break;
                case ':':

                    if (token.Text is ":has")
                    {
                        selector = new Selector(CssSelectorType.HasScope) {
                            Text = token.Text,
                            Arguments = ReadArgs()
                        };

                        break;
                    }

                    else if (token.Text[1] == ':')
                    {
                        selector = new PseudoElementSelector {
                            Text = token.Text
                        };


                        break;
                    }
                    else
                    {
                        selector = new PseudoClassSelector {
                            Text = token.Text,
                            Arguments = ReadArgs()
                        };						

                        break;
                    }
                case '[':
                    selector = new AttributeSelector {
                        Text = token.Text
                    };

                    break;
                default:
                    selector = new TagSelector(token.Text);

                    break;
            }
        }
        else
        {
            throw new Exception($"Unexpected token parsing selector. Was {_tokenizer.Current}");
        }

        if (!IsEnd)
        {
            if (ConsumeIf(CssTokenKind.Whitespace) && Current.Kind is CssTokenKind.Ampersand or CssTokenKind.Name)
            {
                selector.Combinator = CombinatorType.Descendant;

                selector.Next = ReadNewSelector();
            }
            else if (ConsumeIf(CssTokenKind.Gt)) // >
            {
                selector.Combinator = CombinatorType.Child;

                ReadTrivia();

                selector.Next = ReadNewSelector();
            }
            else if (ConsumeIf(CssTokenKind.Add)) // +
            {
                selector.Combinator = CombinatorType.AdjacentSibling;

                ReadTrivia();

                selector.Next = ReadNewSelector();
            }
            else if (ConsumeIf(CssTokenKind.Tilde)) // ~
            {
                selector.Combinator = CombinatorType.SubsequentSibling;

                ReadTrivia();

                selector.Next = ReadNewSelector();
            }
            else if (_tokenizer.Current.Kind is CssTokenKind.Name)
            {
                selector.Combinator = CombinatorType.None;

                selector.Next = ReadNewSelector();
            }

            // :has
            // ...

            else if (_tokenizer.Current.Kind is CssTokenKind.Comma or CssTokenKind.BlockStart)
            {
                return selector;
            }
            else
            {
                throw new Exception($"Unexpected token reading selector. Was {_tokenizer.Current}");
            }
        }

        return selector;
    }

    private CssValue? ReadArgs()
    {
        if (ConsumeIf(CssTokenKind.LeftParenthesis))
        {
            var args = ReadValueList();

            ConsumeIf(CssTokenKind.RightParenthesis);

            return args;
        }

        return null;
    }
}