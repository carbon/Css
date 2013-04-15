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

				default: throw new ParseException("Unexpected:" + this.tokenizer.Current.Kind + " reading rule");
			}
		}

		public CssRule ReadAtRule()
		{
			// @import "subs.css";
			// @media print {

			var ruleType = RuleType.Unknown;

			var atToken = tokenizer.Current;

			tokenizer.Next(); // Read the @ keyword

			SkipWhitespaceAndComments();

			switch (atToken.Value)
			{
				case "@charset":	ruleType = RuleType.Charset;	break;
				case "@import":		ruleType = RuleType.Import;		break;
				case "@font-face":	ruleType = RuleType.FontFace;	break;
				case "@media":		ruleType = RuleType.Media;		break;
				case "@page":		ruleType = RuleType.Page;		break;

				case "@-webkit-keyframes":
				case "@keyframes":	ruleType = RuleType.Keyframes;	break;
			}

			// ATKEYWORD S* any* [ block | ';' S* ];
			// @{keyword} ... 

			var selector = new CssSelector(atToken.Value);

			if (tokenizer.Current.Kind == TokenKind.Identifier)
			{
				selector = new CssSelector(atToken.Value + " " + tokenizer.Current.Value);

				tokenizer.Next(); // Read the identifer (i.e. selector)
			}
			else if (tokenizer.Current.Kind == TokenKind.Name)
			{
				var declaration = ReadDeclaration();

				selector = new CssSelector(atToken.Value + " " + declaration.ToString());
			}

			var rule = new CssRule(ruleType, selector);

			switch (tokenizer.Current.Kind)
			{
				case TokenKind.BlockStart:	ReadBlock(rule);	break; // {
				case TokenKind.Semicolon:	tokenizer.Next();	break; // ;
			}

			return rule;
		}

		public CssRule ReadStyleRule()
		{
			var selectorToken = tokenizer.Current;

			tokenizer.Next(); // Read the selector

			var rule = new CssRule(RuleType.Style, new CssSelector(selectorToken.Value));
			
			ReadBlock(rule);

			return rule;
		}

		public CssBlock ReadBlock(CssRule block)
		{

			Expect(TokenKind.BlockStart);

			tokenizer.Next(); // Read {

			while (tokenizer.Current.Kind != TokenKind.BlockEnd)
			{
				if (tokenizer.IsEnd) throw new Exception("Unexpected EOF reading block");

				SkipWhitespaceAndComments();

				switch (tokenizer.Current.Kind)
				{
					case TokenKind.Identifier:	block.Rules.Add(ReadRule());				break;
					case TokenKind.Name:		block.Declarations.Add(ReadDeclaration());	break; // DeclarationName
					case TokenKind.BlockEnd:	break;

					default: throw new Exception("Unexpected " + tokenizer.Current.ToString() + " reading Block.");
				}
			}

			tokenizer.Next(); // Read }

			return block;
		}

		public CssDeclaration ReadDeclaration()
		{
			var nameToken = tokenizer.Current;

			tokenizer.Next(); // Read the name

			Expect(TokenKind.Colon);

			tokenizer.Next(); // Read the colon

			SkipWhitespaceAndComments(); // Read as leading triva

			Expect(TokenKind.Value);

			var valueToken = tokenizer.Current;

			tokenizer.Next(); // Read the value

			if (tokenizer.Current.Kind == TokenKind.Semicolon)
			{
				tokenizer.Next(); // Read the semicolon
			}

			return new CssDeclaration(nameToken.Value, valueToken.Value);
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