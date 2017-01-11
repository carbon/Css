using System.Collections.Generic;

namespace Carbon.Css.Parser
{
    public class LexicalModeContext
    {
        private readonly Stack<LexicalMode> modes = new Stack<LexicalMode>();

        private LexicalMode current;

        public LexicalModeContext(LexicalMode start)
        {
            this.current = start;

            modes.Push(start);
        }

        public void Enter(LexicalMode mode)
        {
            modes.Push(mode);

            current = mode;
        }

        public void Leave(LexicalMode mode, CssTokenizer tokenizer = null)
        {
            if (current != mode) throw new UnexpectedModeChange(current, mode, (tokenizer == null) ? 0 : tokenizer.Current.Position);

            modes.Pop();

            current = modes.Peek();
        }

        public LexicalMode Current => current;
    }

    public enum LexicalMode : byte
    {
        Unknown = 0,
        Rule = 1,
        Block = 2,
        Value = 3,
        Declaration = 4,
        Selector = 5,
        Assignment = 6,
        Function = 7,

        Symbol = 10,
        Unit = 11,

        Mixin = 20
    }
}
