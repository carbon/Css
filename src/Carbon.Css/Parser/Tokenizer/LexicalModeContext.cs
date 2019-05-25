using System.Collections.Generic;

namespace Carbon.Css.Parser
{
    public struct LexicalModeContext
    {
        private readonly Stack<LexicalMode> modes;

        public LexicalModeContext(LexicalMode start)
        {
            modes = new Stack<LexicalMode>(3);

            this.Current = start;

            modes.Push(start);
        }

        public void Enter(LexicalMode mode)
        {
            modes.Push(mode);

            Current = mode;
        }

        public void Leave(LexicalMode mode, int position = 0)
        {
            if (Current != mode)
            {
                throw new UnexpectedModeChange(Current, mode, position);
            }

            modes.Pop();

            Current = modes.Peek();
        }

        public LexicalMode Current { get; private set; }
    }

    public enum LexicalMode
    {
        Unknown            = 0,
        Rule               = 1,
        Block              = 2,
        Value              = 3,
        Declaration        = 4,
        Selector           = 5,
        Assignment         = 6,
        Function           = 7,
        Symbol             = 10,
        Unit               = 11,
        InterpolatedString = 13,
        Mixin              = 20
    }
}