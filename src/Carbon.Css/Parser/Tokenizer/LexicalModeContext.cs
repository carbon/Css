using System.Collections.Generic;

namespace Carbon.Css.Parser;

public struct LexicalModeContext
{
    private readonly Stack<LexicalMode> modes;
    
    public LexicalModeContext(LexicalMode start)
    {
        modes = new Stack<LexicalMode>(3);

        Current = start;

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