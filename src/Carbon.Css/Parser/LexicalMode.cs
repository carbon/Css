namespace Carbon.Css.Parser
{
	using System;
	using System.Collections.Generic;

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

		public void Leave(LexicalMode mode)
		{
			if (current != mode) throw new Exception("Current mode is:" + current + ". Leaving " + mode + ".");

			modes.Pop();

			current = modes.Peek();
		}

		public LexicalMode Current
		{
			get { return current; }
		}
	}

	public enum LexicalMode
	{
		Unknown = 0,
		Block = 1,
		Value = 2,
		Selector = 3
	}
}
