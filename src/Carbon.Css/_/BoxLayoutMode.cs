using System;

namespace Carbon.Css;

[Flags]
public enum BoxLayoutMode
{
    Unknown    = 0,

    // <display-outside>
    // Defines how the box participates in the flow layout
    Block      = 1 << 0,
    Inline     = 1 << 1,
    RunIn      = 1 << 2, // run-in

    // <display-inside>
    // Defines how the children of the box are laid out.
    Flow        = 1 << 5,
    FlowRoot    = 1 << 6, // flow-root
    Table       = 1 << 7,
    Flex        = 1 << 8,
    Grid        = 1 << 9,
    Ruby        = 1 << 10,

    // <display-listitem>
    ListItem    = 1 << 13,

    // <display-internal>
    TableRowGroup     = 1 << 15,
    TableHeaderGroup  = 1 << 16,
    TableFooterGroup  = 1 << 17,
    TableRow          = 1 << 18,
    TableCell         = 1 << 19,
    TableColumnGroup  = 1 << 20,
    TableColumn       = 1 << 21,
    TableCaption      = 1 << 22,
    RubyBase          = 1 << 23,
    RubyText          = 1 << 24,
    RubyBaseContainer = 1 << 25,
    RubyTextContainer = 1 << 26,


    // <display-box>
    Contents    = 1 << 30,
    None        = 1 << 31,

    InlineBlock = Inline | FlowRoot  // inline flow-root
}

// flex -> block flex

// https://www.w3.org/TR/css-display-3/