using System;
using System.Collections.Generic;

namespace Carbon.Css.Parser
{
    using Helpers;

    public class SyntaxException : Exception
    {
        private readonly int position = 0;

        public SyntaxException(string message, int position = 0)
            : base(message)
        {

            this.position = position;
        }

        public int Position => position;

        public SourceLocation Location { get; set; }

        public IList<LineInfo> Lines { get; set; }

        public static SyntaxException UnexpectedEOF(string context)
        {
            return new SyntaxException($"Unexpected EOF reading '{context}'.");
        }
    }
}