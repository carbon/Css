﻿namespace Carbon.Css.Parser
{
    public sealed class UnbalancedBlock : SyntaxException
    {
        // "Current mode is:" + current + ". Leaving " + mode + "."

        public UnbalancedBlock(CssToken startToken)
            : base("The block is unclosed, '}' expected", startToken.Position)
        { }
    }
}
