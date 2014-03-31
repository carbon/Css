namespace Carbon.Css.Parser
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.IO;
	using System.Linq;

	public class CssParser : IDisposable
	{
		private readonly CssTokenizer tokenizer;

		public CssParser(TextReader textReader)
		{
			this.tokenizer = new CssTokenizer(new SourceReader(textReader));

			tokenizer.Next();
		}

		public CssParser(string text)
		{
			this.tokenizer = new CssTokenizer(new SourceReader(new StringReader(text)));

			tokenizer.Next();
		}

		public IEnumerable<CssRule> ReadRules()
		{
			foreach (var node in ReadNodes())
			{
				if (node.Kind == NodeKind.Rule)
				{
					yield return (CssRule)node;
				}
			}
		}

		public IEnumerable<INode> ReadNodes()
		{
			while (!tokenizer.IsEnd)
			{
				ReadTrivia();

				yield return ReadNode();
			}
		}

		public INode ReadNode()
		{
			switch (tokenizer.Current.Kind)
			{
				case TokenKind.AtKeyword:		return ReadAtRule();
				case TokenKind.Variable:		return ReadVariable();
			}

			var span = ReadSpan();

			return ReadRuleBlock(span);
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

		public CssVariable ReadVariable()
		{
			var name = tokenizer.Read(TokenKind.Variable, LexicalMode.Value);		// read $name

			ReadTrivia();

			tokenizer.Read(TokenKind.Colon, LexicalMode.Value);						// read :

			ReadTrivia();

			var value = ReadSpan();

			if (tokenizer.Current.Kind == TokenKind.Semicolon)
			{
				tokenizer.Read(); // read;
			}

			return new CssVariable(name.Text.TrimStart('$'), CssValue.Parse(value));
		}

		public CssRule ReadAtRule()
		{
			// ATKEYWORD S* any* [ block | ';' S* ];
			// @{keyword} ... 

			// @import "subs.css";
			// @media print {

			var ruleType = RuleType.Unknown;

			var atToken = tokenizer.Read(TokenKind.AtKeyword, LexicalMode.Rule);	// read @keyword

			ReadTrivia();

			switch (atToken.Text)
			{
				case "@charset":	ruleType = RuleType.Charset;	break;
				case "@import":		return ReadImportRule();		
				case "@font-face":	ruleType = RuleType.FontFace;	break;
				case "@media":		ruleType = RuleType.Media;		break;
				case "@page":		ruleType = RuleType.Page;		break;

				case "@-webkit-keyframes":
				case "@keyframes":	ruleType = RuleType.Keyframes;	break;
			}
		
			var selector = new CssSelector(atToken.Text);

			if (tokenizer.Current.Kind == TokenKind.Name || tokenizer.Current.Kind == TokenKind.Identifier)
			{
				var x = ReadSpan();

				selector = new CssSelector(atToken.Text + " " + x.ToString());
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
			var value = ReadValue();

			var rule = new ImportRule {
				Value = CssUrlValue.Parse(value.ToString())
			};

			if (tokenizer.Current.Kind == TokenKind.Semicolon)
			{
				tokenizer.Read();
			}

			return rule;
		}


		public CssName ReadName()
		{
			string name;

			// Allow leading : on selector identifiers
			if (tokenizer.Current.Kind == TokenKind.Colon)
			{
				name = tokenizer.Read().Text + tokenizer.Read().Text;
			}
			else
			{
				name = tokenizer.Read().Text;
			}

			var trivia = ReadTrivia();

			return new CssName(name) {
				Trailing = trivia
			};
		}

		public CssRule ReadStyleRule()
		{
			var selector = new CssSelector(ReadSpan());

			var rule = new CssRule(RuleType.Style, selector);

			ReadBlock(rule);

			return rule;
		}

		public CssRule ReadRuleBlock(TokenList span)
		{
			var rule = new CssRule(RuleType.Style, new CssSelector(span));

			ReadBlock(rule);

			return rule;
		}

		public CssBlock ReadBlock(CssRule block)
		{
			tokenizer.Read(TokenKind.BlockStart, LexicalMode.Block);	// read {

			ReadTrivia();

			while (tokenizer.Current.Kind != TokenKind.BlockEnd)
			{
				if (tokenizer.IsEnd) throw ParseException.UnexpectedEOF("Block");

				var span = ReadSpan();

				switch (tokenizer.Current.Kind)
				{
					case TokenKind.Colon		: block.Add(ReadDeclarationFromName(span));	break; // DeclarationName
					case TokenKind.BlockStart	: block.Children.Add(ReadRuleBlock(span));			break;
					case TokenKind.BlockEnd		: break;

					default: throw ParseException.Unexpected(tokenizer.Current, "Block");
				}
			}

			tokenizer.Read();						// read }

			ReadTrivia();

			return block;
		}

		public CssDeclaration ReadDeclaration()
		{
			var name = ReadSpan();											// read name

			tokenizer.Read(TokenKind.Colon, LexicalMode.Declaration);		// read :

			ReadTrivia();													// TODO: read as leading annotation

			var value = ReadValue();										// read value (value or cssvariable)

			if (tokenizer.Current.Kind == TokenKind.Semicolon)
			{
				tokenizer.Read();											// read ;
			}

			ReadTrivia();

			return new CssDeclaration(name.ToString(), value.ToString());
		}


		public CssDeclaration ReadDeclarationFromName(TokenList name)
		{
			tokenizer.Read(TokenKind.Colon, LexicalMode.Declaration);		// read :

			ReadTrivia();													// TODO: read as leading annotation

			var value = ReadValue();										// read value (value or cssvariable)

			if (tokenizer.Current.Kind == TokenKind.Semicolon)
			{
				tokenizer.Read();											// read ;
			}

			ReadTrivia();

			return new CssDeclaration(name.ToString(), value.ToString());
		}

		public CssValue ReadValue()
		{
			// String or Identifier

			var value = ReadSpan();

			return CssValue.Parse(value.ToString());
		}


		public Whitespace ReadTrivia()
		{
			if (tokenizer.IsEnd || !tokenizer.Current.IsTrivia) return null;

			var trivia = new Whitespace();

			while (tokenizer.Current.IsTrivia)
			{
				trivia.Add(tokenizer.Next());

				if (tokenizer.IsEnd) break;
			}

			return trivia;
		}

		public TokenList ReadSpan()
		{
			var list = new TokenList();

			while (!tokenizer.IsEnd)
			{
				list.Add(tokenizer.Read());

				if (tokenizer.Current.Kind == TokenKind.Colon
					|| tokenizer.Current.Kind == TokenKind.BlockStart
					|| tokenizer.Current.Kind == TokenKind.BlockEnd
					|| tokenizer.Current.Kind == TokenKind.Semicolon)
				{
					break;
				}
			}

			list.AddRange(ReadTrivia()); // Trialing trivia

			return list;
		}

		public void Dispose()
		{
			tokenizer.Dispose();
		}
	}
}