namespace Carbon.Css.Parser
{
	using System;
	using System.Collections.Generic;
	using System.IO;

	public class CssParser
	{
		private readonly CssTokenizer tokenizer;

		public CssParser(string text)
		{
			this.tokenizer = new CssTokenizer(new SourceReader(text));

			tokenizer.Next();
		}

		public IEnumerable<CssRule> ReadRules()
		{
			while (!tokenizer.IsEnd)
			{
				SkipWhitespaceAndComments();

				yield return ReadRule();
			}
		}

		public CssRule ReadRule()
		{
			switch(this.tokenizer.Current.Kind)
			{
				case TokenKind.Identifier:	return ReadStyleRule();
				case TokenKind.AtKeyword:	return ReadAtRule();

				default:					throw new Exception("Unexpected:" + this.tokenizer.Current.Kind);
			}
		}

		public CssRule ReadAtRule()
		{
			// @import "subs.css";
			// @media print {

			RuleType type = RuleType.Unknown;

			var atKeyword = tokenizer.Current;

			tokenizer.Next(); // Read the @ keyword

			SkipWhitespaceAndComments();

			switch (atKeyword.Value)
			{
				case "@charset":	type = RuleType.CharsetRule;	break;
				case "@import":		type = RuleType.ImportRule;		break;
				case "@font-face":	type = RuleType.FontFaceRule;	break;
				case "@media":		type = RuleType.MediaRule;		break;
				case "@page":		type = RuleType.PageRule;		break;
				case "@keyframes":	type = RuleType.KeyframesRule;	break;
			}

			// ATKEYWORD S* any* [ block | ';' S* ];
			// @{keyword} ... 

			var rule = new CssRule(type);

			rule.Selector = new CssSelector(atKeyword.Value);

			if (tokenizer.Current.Kind == TokenKind.Identifier || tokenizer.Current.Kind == TokenKind.Declaration)
			{
				rule.Selector = new CssSelector(atKeyword.Value + " " + tokenizer.Current.Value);

				tokenizer.Next(); // Read the selector
			}

			if (tokenizer.Current.Kind == TokenKind.BlockStart)
			{
				rule.Block = ReadBlock();
			}

			if (tokenizer.Current.Kind == TokenKind.Semicolon)
			{
				tokenizer.Next(); // Read the semicolon
			}

			return rule;
		}

		public CssRule ReadStyleRule()
		{
			var rule = new CssRule(RuleType.StyleRule);

			rule.Selector = new CssSelector(tokenizer.Current.Value);

			tokenizer.Next(); // Read the selector

			Expect(TokenKind.BlockStart);

			rule.Block = ReadBlock();

			return rule;
		}


		public CssBlock ReadBlock()
		{
			var block = new CssBlock();

			tokenizer.Next(); // Read {

			while (tokenizer.Current.Kind != TokenKind.BlockEnd)
			{
				if (tokenizer.IsEnd) throw new Exception("Unexpected EOF reading block");

				if (tokenizer.Current.Kind == TokenKind.Identifier)
				{
					block.Rules.Add(ReadRule());
				}

				if (tokenizer.Current.Kind == TokenKind.Declaration)
				{
					block.Declarations.Add(new CssDeclaration(tokenizer.Current.Value));
				}

				tokenizer.Next();
			}

			tokenizer.Next(); // Read }

			return block;
		}


		public void SkipWhitespaceAndComments()
		{
			while (tokenizer.Current.Kind == TokenKind.Whitespace || tokenizer.Current.Kind == TokenKind.Comment)
			{
				tokenizer.Next();
			}
		}

		public void Expect(TokenKind kind)
		{
			if (tokenizer.Current.Kind != kind)
			{
				throw new Exception("Expected " + kind.ToString() + ". Was " + tokenizer.Current.Kind);
			}
		}
	}
}