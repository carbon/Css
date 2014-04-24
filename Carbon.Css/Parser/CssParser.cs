namespace Carbon.Css.Parser
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	public class CssParser : IDisposable
	{
		private readonly CssTokenizer tokenizer;
		private readonly LexicalModeContext context = new LexicalModeContext(LexicalMode.Unknown);

		public CssParser(TextReader textReader)
		{
			this.tokenizer = new CssTokenizer(new SourceReader(textReader));
		}

		public CssParser(string text)
		{
			this.tokenizer = new CssTokenizer(new SourceReader(new StringReader(text)));
		}

		public CssParser(CssTokenizer tokenizer)
		{
			this.tokenizer = tokenizer;
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

		public IEnumerable<CssNode> ReadNodes()
		{
			while (!tokenizer.IsEnd)
			{
				ReadTrivia();

				yield return ReadNode();
			}
		}

		public CssNode ReadNode()
		{
			switch (tokenizer.Current.Kind)
			{
				case TokenKind.AtSymbol	: return ReadAtRule();
				case TokenKind.Dollar	: return ReadAssignment();
			}

			var selector = ReadSelector();

			return ReadRuleBlock(selector);
		}

		public CssRule ReadRule()
		{
			switch(this.tokenizer.Current.Kind)
			{
				case TokenKind.Name		: return ReadStyleRule();
				case TokenKind.AtSymbol	: return ReadAtRule();

				default: throw new UnexpectedTokenException(LexicalMode.Rule, tokenizer.Current);
			}
		}

		#region At Rules

		public CssRule ReadAtRule()
		{
			// ATKEYWORD S* any* [ block | ';' S* ];
			// @{keyword} ... 

			// @import "subs.css";
			// @media print {

			tokenizer.Read(TokenKind.AtSymbol, LexicalMode.Rule);	// Read @

			var ruleType = RuleType.Unknown;

			var atName = tokenizer.Read();	// read name or string

			ReadTrivia();

			switch (atName.Text)
			{
				case "charset"		: ruleType = RuleType.Charset; break;
				case "import"		: return ReadImportRule();
				case "font-face"	: ruleType = RuleType.FontFace; break;
				case "media"		: return ReadMediaRule();
				case "page"			: ruleType = RuleType.Page; break;

				case "-webkit-keyframes"	:
				case "keyframes"			: ruleType = RuleType.Keyframes; break;
				case "mixin"				: return ReadMixinBody();
			}

			var selector = new CssSelector("@" + atName.Text);

			if (tokenizer.Current.Kind == TokenKind.Name)
			{
				var x = ReadSpan();

				selector = new CssSelector("@" + atName.Text + " " + x.ToString());
			}

			var rule = new CssRule(ruleType, selector);

			switch (tokenizer.Current.Kind)
			{
				case TokenKind.BlockStart: ReadBlock(rule); break; // {
				case TokenKind.Semicolon: tokenizer.Read(); break; // ;
			}

			return rule;
		}

		public CssRule ReadMediaRule()
		{
			// @media only screen and (min-width : 1600px) {

			var span = new TokenList();

			while (tokenizer.Current.Kind != TokenKind.BlockStart && !tokenizer.IsEnd)
			{
				var token = tokenizer.Read();

				span.Add(token);
			}


			var rule = new CssRule(RuleType.Media, new CssSelector("@media " + span.ToString().Trim()));

			ReadBlock(rule);

			return rule;

			

		
		}



		public CssRule ReadImportRule()
		{
			var value = ReadValue();

			var rule = new ImportRule
			{
				Url = CssUrlValue.Parse(value.ToString())
			};

			if (tokenizer.Current.Kind == TokenKind.Semicolon)
			{
				tokenizer.Read();
			}

			rule.Trailing = ReadTrivia();

			return rule;
		}

		#endregion

		#region Values

		// Read comma seperated values

		public CssValue ReadValue()
		{
			// : #fffff
			// : $oranges
			// : url(file.css);

			var values = new List<CssValue>();

			do
			{
				if (tokenizer.Current.Kind == TokenKind.Comma) 	// read the comma & trailing whitespace
				{					
					tokenizer.Read();
				
					ReadTrivia();
				}

				values.Add(CssValue.FromComponents(ReadComponents()));

			} while (tokenizer.Current.Kind == TokenKind.Comma);

			var trivia = ReadTrivia(); // Trialing trivia

			if (values.Count == 1) return values[0];

			var list = new CssValueList(values, ValueListSeperator.Comma);

			return list;
		}

		public IEnumerable<CssValue> ReadComponents()
		{
			while (!tokenizer.IsEnd)
			{
				yield return ReadLiteral();

				var current = tokenizer.Current;

				if   ( current.Kind == TokenKind.BlockEnd
					|| current.Kind == TokenKind.Semicolon
					|| current.Kind == TokenKind.Comma
					|| current.Kind == TokenKind.RightParenthesis)
				{
					break;
				}
			}
		}

		public CssValue ReadLiteral()
		{
			if (tokenizer.Current.Kind == TokenKind.Dollar) return ReadVariable();
			
			var value = tokenizer.Read();	// read value (string or number)

			if (value.Kind == TokenKind.Number)
			{
				var hasUnit = (tokenizer.Current.Kind == TokenKind.String || tokenizer.Current.Kind == TokenKind.Name);

				if (hasUnit)
				{
					var unit = tokenizer.Read(); // Read unit

					return new CssDimension(value, unit) {
						Trailing = ReadTrivia()
					};
				}
				else
				{
					return new CssNumber(value) {
						Trailing = ReadTrivia()
					};
				}
			}

			if (tokenizer.Current.Kind == TokenKind.LeftParenthesis)
			{
				tokenizer.Read(TokenKind.LeftParenthesis, LexicalMode.Function);

				var args = ReadValue();

				tokenizer.Read(TokenKind.RightParenthesis, LexicalMode.Function);

				if (value.Text == "url")
				{
					return new CssUrl(value, args) {
						Trailing = ReadTrivia()
					};
				}

				return new CssFunction(value, args) {
					Trailing = ReadTrivia()
				};
			}

			return new CssString(value) {
				Trailing = ReadTrivia()
			};
		}



		/*
		public CssFunction ReadFunction()
		{

			// A functional notation is a type of component value that can represent more complex types or invoke special processing.
			// The syntax starts with the name of the function immediately followed by a left parenthesis (i.e. a FUNCTION token) followed by the argument(s) 
			// to the notation followed by a right parenthesis. 
			// White space is allowed, but optional, immediately inside the parentheses. 
			// If a function takes a list of arguments, the arguments are separated by a comma (‘,’) with optional whitespace before and after the comma.


		}
		*/

		public CssVariable ReadVariable()
		{
			tokenizer.Read(TokenKind.Dollar, LexicalMode.Value);				// read $

			var symbol = tokenizer.Read(TokenKind.Name, LexicalMode.Value);		// read symbol

			return new CssVariable(symbol) { 
				Leading = ReadTrivia()
			};
		}

		/*
		public CssExpression ReadExpression()
		{
			
		}
		*/
		
		#endregion

		#region Variables

		public CssAssignment ReadAssignment()
		{
			tokenizer.Read(TokenKind.Dollar, LexicalMode.Assignment);			// read $

			var name = tokenizer.Read(TokenKind.Name, LexicalMode.Assignment);	// read name

			ReadTrivia();

			tokenizer.Read(TokenKind.Colon, LexicalMode.Assignment);			// read :

			ReadTrivia();														// Read trivia

			var value = ReadValue();

			if (tokenizer.Current.Kind == TokenKind.Semicolon)
			{
				tokenizer.Read(); // read;
			}

			return new CssAssignment(name, value);
		}

		#endregion

		public CssSelector ReadSelector()
		{
			// #id.hello { } 

			var span = new TokenList();

			while (tokenizer.Current.Kind != TokenKind.BlockStart && !tokenizer.IsEnd)
			{
				var token = tokenizer.Read();

				span.Add(token);
			}

			return new CssSelector(span);
		}

		/*
		public IEnumerable<CssSelectorList> ReadSelectors()
		{
			var names = new TokenList();

			foreach (var token in this)
			{
				if (token.IsTrivia) continue;

				if (token.Kind == TokenKind.Comma)
				{
					yield return new CssSelector(names);

					names.Clear();
				}
				else
				{
					names.Add(token);
				}
			}

			yield return new CssSelector(names);
		}
		*/

		public CssRule ReadStyleRule()
		{
			var selector = ReadSelector(); 

			var rule = new CssRule(RuleType.Style, selector);

			ReadBlock(rule);

			return rule;
		}

		#region Mixins

		// @mixin sexy-border($color, $width: 1in) {

		/*
		@mixin left($dist) {
		  float: left;
		  margin-left: $dist;
		}
		*/

		public MixinNode ReadMixinBody()
		{
			var name = tokenizer.Read(); // name or string

			ReadTrivia();

			var parameters = new List<CssParameter>();

			if(tokenizer.Current.Kind == TokenKind.LeftParenthesis)
			{
				parameters = ReadParameterList();
			}

			tokenizer.Read(TokenKind.BlockStart, LexicalMode.Block);	// read {

			ReadTrivia();

			var declarations = ReadDeclartions().ToArray();

			tokenizer.Read(); // read }

			return new MixinNode(name.Text, parameters, declarations) {
				Trailing = ReadTrivia()
			};
		}

		public List<CssParameter> ReadParameterList()
		{
			// ($color, $width: 1in)

			tokenizer.Read(); // read (

			var list = new List<CssParameter>();

			while(tokenizer.Current.Kind != TokenKind.RightParenthesis && !tokenizer.IsEnd)
			{
				tokenizer.Read(TokenKind.Dollar, LexicalMode.Unknown);

				var name = tokenizer.Read();
				CssValue @default = null;

				ReadTrivia();

				if(tokenizer.Current.Kind == TokenKind.Colon)
				{
					tokenizer.Read(); // Read the colon

					@default = CssValue.FromComponents(ReadComponents());
				}

				ReadTrivia();

				if(tokenizer.Current.Kind == TokenKind.Comma)
				{
					tokenizer.Read();
				}

				ReadTrivia();

				list.Add(new CssParameter(name.Text, @default));
			}

			tokenizer.Read(TokenKind.RightParenthesis, LexicalMode.Unknown); // read )

			ReadTrivia();

			return list;
		}

		public IEnumerable<CssDeclaration> ReadDeclartions()
		{
			while (tokenizer.Current.Kind != TokenKind.BlockEnd && !tokenizer.IsEnd)
			{
				yield return ReadDeclaration();
			}
		}


		public IncludeNode ReadInclude()
		{
			ReadTrivia();

			var name = tokenizer.Read(); // Read the name

			ReadTrivia();

			CssValue args = null;

			if (tokenizer.Current.Kind == TokenKind.LeftParenthesis)
			{
				tokenizer.Read(TokenKind.LeftParenthesis, LexicalMode.Function);

				args = ReadValue();

				tokenizer.Read(TokenKind.RightParenthesis, LexicalMode.Function);
			}

			var trivia = ReadTrivia();

			if (tokenizer.Current.Kind == TokenKind.Semicolon)
			{
				tokenizer.Read(); // Read ;
			}

			return new IncludeNode(name.Text, args) {
				Leading = ReadTrivia()
			};

			// @include name(args)
		}

		#endregion


		public CssRule ReadRuleBlock(CssSelector selector)
		{
			var rule = new CssRule(RuleType.Style, selector);

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

				// A list of delarations or blocks

				if (tokenizer.Current.Kind == TokenKind.AtSymbol)
				{
					tokenizer.Read(); // Read @

					var name = tokenizer.Read(); // Name

					if (name.Text == "include")
					{
						block.Add(ReadInclude()); continue;
					}
				}

				var statement = ReadSpan();

				switch (tokenizer.Current.Kind)
				{
					case TokenKind.Colon		: block.Add(ReadDeclarationFromName(statement));		break; // DeclarationName
					case TokenKind.BlockStart	: block.Add(ReadRuleBlock(new CssSelector(statement)));	break;
					case TokenKind.BlockEnd		: break;

					// TODO: Figure out where we missed reading the semicolon TEMP
					case TokenKind.Semicolon	: tokenizer.Read(); break;

					default: throw new UnexpectedTokenException(LexicalMode.Block, tokenizer.Current);
				}
			}

			tokenizer.Read(); // read }

			block.Leading = ReadTrivia();

			return block;
		}

		public CssDeclaration ReadDeclaration()
		{
			var name = ReadName();											// read name

			ReadTrivia();													// Read trivia

			tokenizer.Read(TokenKind.Colon, LexicalMode.Declaration);		// read :

			ReadTrivia();													// TODO: read as leading annotation

			var value = ReadValue();										// read value (value or valuelist)

			if (tokenizer.Current.Kind == TokenKind.Semicolon)
			{
				tokenizer.Read();											// read ;
			}

			ReadTrivia();

			return new CssDeclaration(name, value);
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

			return new CssDeclaration(name.ToString(), value);
		}

		public Whitespace ReadTrivia()
		{
			if (tokenizer.IsEnd || !tokenizer.Current.IsTrivia) return null;

			var trivia = new Whitespace();

			while (tokenizer.Current.IsTrivia && !tokenizer.IsEnd)
			{
				trivia.Add(tokenizer.Read());
			}

			return trivia;
		}

		public string ReadName()
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

			return name;
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

/*
stylesheet  : [ CDO | CDC | S | statement ]*;
statement   : ruleset | at-rule;
at-rule     : ATKEYWORD S* any* [ block | ';' S* ];
block       : '{' S* [ any | block | ATKEYWORD S* | ';' S* ]* '}' S*;
ruleset     : selector? '{' S* declaration? [ ';' S* declaration? ]* '}' S*;
selector    : any+;
declaration : property S* ':' S* value;
property    : IDENT;
value       : [ any | block | ATKEYWORD S* ]+;
any         : [ IDENT | NUMBER | PERCENTAGE | DIMENSION | STRING
              | DELIM | URI | HASH | UNICODE-RANGE | INCLUDES
              | DASHMATCH | ':' | FUNCTION S* [any|unused]* ')'
              | '(' S* [any|unused]* ')' | '[' S* [any|unused]* ']'
              ] S*;
unused      : block | ATKEYWORD S* | ';' S* | CDO S* | CDC S*;
*/