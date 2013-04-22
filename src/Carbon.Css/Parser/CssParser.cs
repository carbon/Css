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

				default: throw ParseException.Unexpected(this.tokenizer.Current, "Rule");
			}
		}

		public CssRule ReadAtRule()
		{
			// ATKEYWORD S* any* [ block | ';' S* ];
			// @{keyword} ... 

			// @import "subs.css";
			// @media print {

			var ruleType = RuleType.Unknown;

			var atToken = tokenizer.Read(TokenKind.AtKeyword, "AtRule");	// read @keyword

			SkipWhitespaceAndComments();

			switch (atToken.Value)
			{
				case "@charset":	ruleType = RuleType.Charset;	break;
				case "@import":		return ReadImportRule();		
				case "@font-face":	ruleType = RuleType.FontFace;	break;
				case "@media":		ruleType = RuleType.Media;		break;
				case "@page":		ruleType = RuleType.Page;		break;

				case "@-webkit-keyframes":
				case "@keyframes":	ruleType = RuleType.Keyframes;	break;
			}

			var selector = new CssSelector(atToken.Value);

			if (tokenizer.Current.Kind == TokenKind.Identifier)
			{
				var identifer = tokenizer.Read();

				selector = new CssSelector(atToken.Value + " " + identifer.Value);
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

		public CssRule ReadImportRule()
		{
			SkipWhitespaceAndComments();

			var value = tokenizer.Read(TokenKind.Value, "ImportRule");

			var rule = new ImportRule {
				Value = CssUrlValue.Parse(value.Value)
			};

			if (tokenizer.Current.Kind == TokenKind.Semicolon)
			{
				tokenizer.Read();
			}

			return rule;
		}

		public CssRule ReadStyleRule()
		{
			var selectorToken = tokenizer.Read(); // read selector

			var rule = new CssRule(RuleType.Style, new CssSelector(selectorToken.Value));
			
			ReadBlock(rule);

			return rule;
		}

		public CssBlock ReadBlock(CssRule block)
		{
			tokenizer.Read(TokenKind.BlockStart, "Block");	// read {

			while (tokenizer.Current.Kind != TokenKind.BlockEnd)
			{
				if (tokenizer.IsEnd) throw ParseException.UnexpectedEOF("Block");

				SkipWhitespaceAndComments();

				switch (tokenizer.Current.Kind)
				{
					case TokenKind.Identifier:	block.Children.Add(ReadRule());				break;
					case TokenKind.Name:		block.Declarations.Add(ReadDeclaration());	break; // DeclarationName
					case TokenKind.BlockEnd:	break;

					default: throw ParseException.Unexpected(tokenizer.Current, "Block");
				}
			}

			tokenizer.Read();						// read }

			return block;
		}

		public CssDeclaration ReadDeclaration()
		{
			var nameToken = tokenizer.Read();									// read name

			tokenizer.Read(TokenKind.Colon, "declaration");						// read :

			SkipWhitespaceAndComments();										// TODO: read as leading annotation

			var valueToken = tokenizer.Read(TokenKind.Value, "declaration");	// read value

			if (tokenizer.Current.Kind == TokenKind.Semicolon)
			{
				tokenizer.Read();												// read ;
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
	}
}