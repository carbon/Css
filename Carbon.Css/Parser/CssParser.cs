using System;
using System.Collections.Generic;
using System.IO;

namespace Carbon.Css.Parser
{
	public class CssParser : IDisposable
	{
		private readonly CssTokenizer tokenizer;
		private readonly LexicalModeContext context = new LexicalModeContext(LexicalMode.Unknown);

		public CssParser(TextReader textReader)
			: this(new CssTokenizer(new SourceReader(textReader)))
		{ }

		public CssParser(string text)
			: this(new CssTokenizer(new SourceReader(new StringReader(text))))
		{ }

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
				case TokenKind.Directive : return ReadDirective();
				case TokenKind.AtSymbol	 : return ReadAtRule();
				case TokenKind.Dollar	 : return ReadAssignment();
			}

			var selector = ReadSelector();

			return ReadRuleBlock(selector);
		}

		public CssNode ReadDirective()
		{
			var text = tokenizer.Current.Text;

			tokenizer.Read();

			// READ: //=
			var parts = text.Substring(3).TrimStart().Split(new[] { ' ' }, 2);

			//= support Safari 5.1
			return new CssDirective(
				name  : parts[0].Trim(),
				value : parts[1].Trim()
			);
		}

		public CssNode ReadRule()
		{
			switch(this.tokenizer.Current.Kind)
			{
				case TokenKind.Name		: return ReadStyleRule();
				case TokenKind.AtSymbol	: return ReadAtRule();

				default: throw new UnexpectedTokenException(LexicalMode.Rule, tokenizer.Current);
			}
		}

		#region At Rules

		public CssNode ReadAtRule()
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
				case "charset"	 : ruleType = RuleType.Charset; break;
				case "import"	 : return ReadImportRule();
				case "font-face" : return ReadFontFaceRule();
				case "media"	 : return ReadMediaRule();
				case "page"		 : ruleType = RuleType.Page; break;
				case "keyframes" : return ReadKeyframesRule();
				case "mixin"	 : return ReadMixinBody();
				case "if"		 : return ReadIfRule();
			}

			string selectorText = null;

			if (tokenizer.Current.Kind == TokenKind.Name)
			{
				selectorText = ReadSpan().ToString();
			}

			var rule = new AtRule(atName.Text, ruleType, selectorText ?? "");

			switch (tokenizer.Current.Kind)
			{
				case TokenKind.BlockStart : ReadBlock(rule);  break; // {
				case TokenKind.Semicolon  : tokenizer.Read(); break; // ;
			}

			return rule;
		}

		public CssRule ReadKeyframesRule()
		{
			// @media only screen and (min-width : 1600px) {

			var span = new TokenList();

			while (tokenizer.Current.Kind != TokenKind.BlockStart && !tokenizer.IsEnd)
			{
				var token = tokenizer.Read();

				span.Add(token);
			}

			var rule = new KeyframesRule(span.RawText.Trim());

			ReadBlock(rule);

			return rule;
		}

		public CssRule ReadMediaRule()
		{
			// @media {

			var span = new TokenList();

			while (tokenizer.Current.Kind != TokenKind.BlockStart && !tokenizer.IsEnd)
			{
				var token = tokenizer.Read();

				span.Add(token);
			}

			var rule = new MediaRule(span.RawText.Trim());

			ReadBlock(rule);

			return rule;
		}

		public CssRule ReadFontFaceRule()
		{
			// @font-face {

			while (tokenizer.Current.Kind != TokenKind.BlockStart && !tokenizer.IsEnd)
			{
				tokenizer.Read();
			}

			var rule = new FontFaceRule();
			
			// Declarations

			ReadBlock(rule);

			return rule;
		}

		public CssRule ReadImportRule()
		{
			var value = ReadValueList();

			var rule = new ImportRule(CssUrlValue.Parse(value.ToString()));

			if (tokenizer.Current.Kind == TokenKind.Semicolon)
			{
				tokenizer.Read();
			}

			rule.Trailing = ReadTrivia();

			return rule;
		}


		public IfBlock ReadIfRule()
		{
			// @font-face [expression] {

			ReadTrivia();

			var condition = ReadExpression();

			var rule = new IfBlock(condition);

			ReadBlock(rule);

			return rule;
		}

		#endregion

		#region Values

		// Read comma seperated values

		public CssValue ReadValueList()
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

			var list = new CssValueList(values, ValueSeperator.Comma);

			return list;
		}

		public IEnumerable<CssValue> ReadComponents()
		{
			while (!tokenizer.IsEnd)
			{
				yield return ReadComponent();

				var current = tokenizer.Current;

				if   ( current.Kind == TokenKind.BlockStart
					|| current.Kind == TokenKind.BlockEnd
					|| current.Kind == TokenKind.Semicolon
					|| current.Kind == TokenKind.Comma
					|| current.Kind == TokenKind.RightParenthesis)
				{
					break;
				}
			}
		}

		public CssValue ReadComponent()
		{
			// Variable
			// Number
			// Measurement
			// Function
			// Expression

			switch (tokenizer.Current.Kind)
			{
				case TokenKind.Dollar: return ReadVariable();
				case TokenKind.Number: return ReadNumberOrMeasurement();
			}

			var value = tokenizer.Read();   // read string or number

			// Function
			// A functional notation is a type of component value that can represent more complex types or invoke special processing.
			// The syntax starts with the name of the function immediately followed by a left parenthesis (i.e. a FUNCTION token) followed by the argument(s) 
			// to the notation followed by a right parenthesis. 
			// White space is allowed, but optional, immediately inside the parentheses. 
			// If a function takes a list of arguments, the arguments are separated by a comma (‘,’) with optional whitespace before and after the comma.

			if (tokenizer.Current.Kind == TokenKind.LeftParenthesis)
			{
				tokenizer.Read(TokenKind.LeftParenthesis, LexicalMode.Function);

				var args = ReadValueList();

				tokenizer.Read(TokenKind.RightParenthesis, LexicalMode.Function);

				if (value.Text == "url")
				{
					return new CssUrl(value, args) {
						Trailing = ReadTrivia()
					};
				}

				return new CssFunction(value.Text, args) {
					Trailing = ReadTrivia()
				};
			}

			// ReadString (consider context)

			// :-ms-input-placeholder
			// #id:before
			// :link
			// :not
			
			return new CssString(value) {
				Trailing = ReadTrivia()
			};
		}

		public CssValue ReadNumberOrMeasurement()
		{
			var value = float.Parse(tokenizer.Read().Text);   // read number
			
			if (tokenizer.Current.Kind == TokenKind.Unit)
			{
				var unit = CssUnit.Get(tokenizer.Read().Text);

				return new CssMeasurement(value, unit) {
					Trailing = ReadTrivia()
				};
			}

			return new CssNumber(value) {
				Trailing = ReadTrivia()
			};
		}

		public CssVariable ReadVariable()
		{
			tokenizer.Read(TokenKind.Dollar, LexicalMode.Value);				// read $

			var symbol = tokenizer.Read(TokenKind.Name, LexicalMode.Value);		// read symbol

			return new CssVariable(symbol) { 
				Leading = ReadTrivia()
			};
		}


		#endregion

		#region Expressions

		// https://en.wikipedia.org/wiki/Shunting-yard_algorithm

		// 1: *, /, %
		// 2: +, – 
		// 3: ==, !=, >, >=, <, <= 
		// 4: &&, ||

		// | number | plus | number | equals |  number  |
		//      5      +       10       ==       5
		// |    BinaryExpression    |
		// |               BinaryExpression             |
		public CssValue ReadExpression()
		{
			var left = ReadComponent(); // Variable, FunctionCall, ...

			// Wasn't an expression
			if (!tokenizer.Current.IsBinaryOperator)
			{
				return left;
			}

			var opToken = tokenizer.Read(); // Read operator

			ReadTrivia();

			var op = (Op)((int)opToken.Kind);
		
			var right = ReadComponent();

			return new BinaryExpression(left, op, right);
		}

		#endregion

		#region Variables

		public CssAssignment ReadAssignment()
		{
			tokenizer.Read(TokenKind.Dollar, LexicalMode.Assignment);			// read $

			var name = tokenizer.Read(TokenKind.Name, LexicalMode.Assignment);	// read name

			ReadTrivia();

			tokenizer.Read(TokenKind.Colon, LexicalMode.Assignment);			// read :

			ReadTrivia();														// Read trivia

			var value = ReadValueList();

			if (tokenizer.Current.Kind == TokenKind.Semicolon)
			{
				tokenizer.Read(); // read;
			}

			ReadTrivia();

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

				// Consider multiselectors
				// if (token.Kind == TokenKind.Comma) ;

				span.Add(token);
			}

			return new CssSelector(span);
		}

		public StyleRule ReadStyleRule()
		{
			var rule = new StyleRule(ReadSelector());

			ReadBlock(rule);

			return rule;
		}

		#region Mixins

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

			var mixin = new MixinNode(name.Text, parameters);

			ReadBlock(mixin);

			return mixin;
			
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

				if (tokenizer.Current.Kind == TokenKind.Colon)
				{
					tokenizer.Read();	// Read :

					ReadTrivia();		// Read trivia

					@default = CssValue.FromComponents(ReadComponents());
				}

				ReadTrivia();

				if (tokenizer.Current.Kind == TokenKind.Comma)
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
			// @include name(args)

			ReadTrivia();

			var name = tokenizer.Read(); // Read the name

			ReadTrivia();

			CssValue args = null;

			if (tokenizer.Current.Kind == TokenKind.LeftParenthesis)
			{
				tokenizer.Read(TokenKind.LeftParenthesis, LexicalMode.Function);

				args = ReadValueList();

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
		}

		#endregion

		public CssRule ReadRuleBlock(CssSelector selector)
		{
			var rule = new StyleRule(selector);

			ReadBlock(rule);

			return rule;
		}

		public CssBlock ReadBlock(CssBlock block)
		{
			var blockStart = tokenizer.Read(TokenKind.BlockStart, LexicalMode.Block); // Read {

			ReadTrivia();

			while (tokenizer.Current.Kind != TokenKind.BlockEnd)
			{
				if (tokenizer.IsEnd) throw new UnbalancedBlock(LexicalMode.Block, blockStart);

				// A list of delarations or blocks

				if (tokenizer.Current.Kind == TokenKind.AtSymbol)
				{
					tokenizer.Read(); // Read @

					var name = tokenizer.Read(); // Name

					switch (name.Text)
					{
						case "include" : block.Add(ReadInclude()); continue;
						case "if"	   : block.Add(ReadIfRule());  continue;
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

			block.Trailing = ReadTrivia();

			return block;
		}

		public CssDeclaration ReadDeclaration()
		{
			var name = ReadName();										// read name

			ReadTrivia();												// read trivia

			tokenizer.Read(TokenKind.Colon, LexicalMode.Declaration);	// read :

			ReadTrivia();												// TODO: read as leading trivia

			var value = ReadValueList();								// read value (value or valuelist)

			if (tokenizer.Current.Kind == TokenKind.Semicolon)
			{
				tokenizer.Read();										// read ;
			}

			ReadTrivia();

			return new CssDeclaration(name, value);
		}

		public CssDeclaration ReadDeclarationFromName(TokenList name)
		{
			tokenizer.Read(TokenKind.Colon, LexicalMode.Declaration);	// read :

			ReadTrivia();												// TODO: read as leading trivia

			var value = ReadValueList();								// read value (value or cssvariable)

			if (tokenizer.Current.Kind == TokenKind.Semicolon)
			{
				tokenizer.Read();										// read ;
			}

			ReadTrivia();

			return new CssDeclaration(name.ToString(), value);
		}

		public Trivia ReadTrivia()
		{
			if (tokenizer.IsEnd || !tokenizer.Current.IsTrivia) return null;

			var trivia = new Trivia();

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

			ReadTrivia();

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

			ReadTrivia(); // Trialing trivia

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