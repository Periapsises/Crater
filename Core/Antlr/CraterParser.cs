//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from C:/Users/yannk/Desktop/Crater/Core/CraterParser.g4 by ANTLR 4.13.2

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace Core.Antlr {
using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.2")]
[System.CLSCompliant(false)]
public partial class CraterParser : Parser {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		FUNCTION=1, LOCAL=2, RETURN=3, END=4, NOT=5, AND=6, OR=7, IF=8, ELSEIF=9, 
		ELSE=10, THEN=11, WHILE=12, FOR=13, DO=14, IN=15, REPEAT=16, UNTIL=17, 
		CLASS=18, STATIC=19, NEW=20, NUMBER=21, HEXADECIMAL=22, EXPONENTIAL=23, 
		STRING=24, BOOLEAN=25, IDENTIFIER=26, ASSIGN=27, LESS=28, LESS_EQUAL=29, 
		GREATER=30, GREATER_EQUAL=31, EQUAL=32, NOT_EQUAL=33, PLUS=34, MINUS=35, 
		MUL=36, DIV=37, MOD=38, EXP=39, QMARK=40, CONCAT=41, LPAREN=42, RPAREN=43, 
		LBRACKET=44, RBRACKET=45, LSQRBRACKET=46, RSQRBRACKET=47, COLON=48, COMMA=49, 
		DOT=50, COMMENT=51, WHITESPACE=52;
	public const int
		RULE_program = 0, RULE_block = 1, RULE_statement = 2, RULE_variableDeclaration = 3, 
		RULE_typeName = 4, RULE_expression = 5, RULE_literal = 6;
	public static readonly string[] ruleNames = {
		"program", "block", "statement", "variableDeclaration", "typeName", "expression", 
		"literal"
	};

	private static readonly string[] _LiteralNames = {
		null, "'function'", "'local'", "'return'", "'end'", "'not'", "'and'", 
		"'or'", "'if'", "'elseif'", "'else'", "'then'", "'while'", "'for'", "'do'", 
		"'in'", "'repeat'", "'until'", "'class'", "'static'", "'new'", null, null, 
		null, null, null, null, "'='", "'<'", "'<='", "'>'", "'>='", "'=='", "'~='", 
		"'+'", "'-'", "'*'", "'/'", "'%'", "'^'", "'?'", "'..'", "'('", "')'", 
		"'{'", "'}'", "'['", "']'", "':'", "','", "'.'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "FUNCTION", "LOCAL", "RETURN", "END", "NOT", "AND", "OR", "IF", 
		"ELSEIF", "ELSE", "THEN", "WHILE", "FOR", "DO", "IN", "REPEAT", "UNTIL", 
		"CLASS", "STATIC", "NEW", "NUMBER", "HEXADECIMAL", "EXPONENTIAL", "STRING", 
		"BOOLEAN", "IDENTIFIER", "ASSIGN", "LESS", "LESS_EQUAL", "GREATER", "GREATER_EQUAL", 
		"EQUAL", "NOT_EQUAL", "PLUS", "MINUS", "MUL", "DIV", "MOD", "EXP", "QMARK", 
		"CONCAT", "LPAREN", "RPAREN", "LBRACKET", "RBRACKET", "LSQRBRACKET", "RSQRBRACKET", 
		"COLON", "COMMA", "DOT", "COMMENT", "WHITESPACE"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "CraterParser.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static CraterParser() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}

		public CraterParser(ITokenStream input) : this(input, Console.Out, Console.Error) { }

		public CraterParser(ITokenStream input, TextWriter output, TextWriter errorOutput)
		: base(input, output, errorOutput)
	{
		Interpreter = new ParserATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	public partial class ProgramContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public BlockContext block() {
			return GetRuleContext<BlockContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode Eof() { return GetToken(CraterParser.Eof, 0); }
		public ProgramContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_program; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			ICraterParserListener typedListener = listener as ICraterParserListener;
			if (typedListener != null) typedListener.EnterProgram(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			ICraterParserListener typedListener = listener as ICraterParserListener;
			if (typedListener != null) typedListener.ExitProgram(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ICraterParserVisitor<TResult> typedVisitor = visitor as ICraterParserVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitProgram(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public ProgramContext program() {
		ProgramContext _localctx = new ProgramContext(Context, State);
		EnterRule(_localctx, 0, RULE_program);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 14;
			block();
			State = 15;
			Match(Eof);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class BlockContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public StatementContext[] statement() {
			return GetRuleContexts<StatementContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public StatementContext statement(int i) {
			return GetRuleContext<StatementContext>(i);
		}
		public BlockContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_block; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			ICraterParserListener typedListener = listener as ICraterParserListener;
			if (typedListener != null) typedListener.EnterBlock(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			ICraterParserListener typedListener = listener as ICraterParserListener;
			if (typedListener != null) typedListener.ExitBlock(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ICraterParserVisitor<TResult> typedVisitor = visitor as ICraterParserVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitBlock(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public BlockContext block() {
		BlockContext _localctx = new BlockContext(Context, State);
		EnterRule(_localctx, 2, RULE_block);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 20;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while (_la==LOCAL || _la==IDENTIFIER) {
				{
				{
				State = 17;
				statement();
				}
				}
				State = 22;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class StatementContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public VariableDeclarationContext variableDeclaration() {
			return GetRuleContext<VariableDeclarationContext>(0);
		}
		public StatementContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_statement; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			ICraterParserListener typedListener = listener as ICraterParserListener;
			if (typedListener != null) typedListener.EnterStatement(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			ICraterParserListener typedListener = listener as ICraterParserListener;
			if (typedListener != null) typedListener.ExitStatement(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ICraterParserVisitor<TResult> typedVisitor = visitor as ICraterParserVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitStatement(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public StatementContext statement() {
		StatementContext _localctx = new StatementContext(Context, State);
		EnterRule(_localctx, 4, RULE_statement);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 23;
			variableDeclaration();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class VariableDeclarationContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode IDENTIFIER() { return GetToken(CraterParser.IDENTIFIER, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode COLON() { return GetToken(CraterParser.COLON, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public TypeNameContext typeName() {
			return GetRuleContext<TypeNameContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode LOCAL() { return GetToken(CraterParser.LOCAL, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode QMARK() { return GetToken(CraterParser.QMARK, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode ASSIGN() { return GetToken(CraterParser.ASSIGN, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ExpressionContext expression() {
			return GetRuleContext<ExpressionContext>(0);
		}
		public VariableDeclarationContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_variableDeclaration; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			ICraterParserListener typedListener = listener as ICraterParserListener;
			if (typedListener != null) typedListener.EnterVariableDeclaration(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			ICraterParserListener typedListener = listener as ICraterParserListener;
			if (typedListener != null) typedListener.ExitVariableDeclaration(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ICraterParserVisitor<TResult> typedVisitor = visitor as ICraterParserVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitVariableDeclaration(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public VariableDeclarationContext variableDeclaration() {
		VariableDeclarationContext _localctx = new VariableDeclarationContext(Context, State);
		EnterRule(_localctx, 6, RULE_variableDeclaration);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 26;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==LOCAL) {
				{
				State = 25;
				Match(LOCAL);
				}
			}

			State = 28;
			Match(IDENTIFIER);
			State = 29;
			Match(COLON);
			State = 30;
			typeName();
			State = 32;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==QMARK) {
				{
				State = 31;
				Match(QMARK);
				}
			}

			State = 36;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==ASSIGN) {
				{
				State = 34;
				Match(ASSIGN);
				State = 35;
				expression(0);
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class TypeNameContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode FUNCTION() { return GetToken(CraterParser.FUNCTION, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode IDENTIFIER() { return GetToken(CraterParser.IDENTIFIER, 0); }
		public TypeNameContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_typeName; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			ICraterParserListener typedListener = listener as ICraterParserListener;
			if (typedListener != null) typedListener.EnterTypeName(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			ICraterParserListener typedListener = listener as ICraterParserListener;
			if (typedListener != null) typedListener.ExitTypeName(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ICraterParserVisitor<TResult> typedVisitor = visitor as ICraterParserVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitTypeName(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public TypeNameContext typeName() {
		TypeNameContext _localctx = new TypeNameContext(Context, State);
		EnterRule(_localctx, 8, RULE_typeName);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 38;
			_la = TokenStream.LA(1);
			if ( !(_la==FUNCTION || _la==IDENTIFIER) ) {
			ErrorHandler.RecoverInline(this);
			}
			else {
				ErrorHandler.ReportMatch(this);
			    Consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class ExpressionContext : ParserRuleContext {
		public ExpressionContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_expression; } }
	 
		public ExpressionContext() { }
		public virtual void CopyFrom(ExpressionContext context) {
			base.CopyFrom(context);
		}
	}
	public partial class ParenthesizedExpressionContext : ExpressionContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode LPAREN() { return GetToken(CraterParser.LPAREN, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ExpressionContext expression() {
			return GetRuleContext<ExpressionContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode RPAREN() { return GetToken(CraterParser.RPAREN, 0); }
		public ParenthesizedExpressionContext(ExpressionContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			ICraterParserListener typedListener = listener as ICraterParserListener;
			if (typedListener != null) typedListener.EnterParenthesizedExpression(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			ICraterParserListener typedListener = listener as ICraterParserListener;
			if (typedListener != null) typedListener.ExitParenthesizedExpression(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ICraterParserVisitor<TResult> typedVisitor = visitor as ICraterParserVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitParenthesizedExpression(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class OrOperationContext : ExpressionContext {
		[System.Diagnostics.DebuggerNonUserCode] public ExpressionContext[] expression() {
			return GetRuleContexts<ExpressionContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public ExpressionContext expression(int i) {
			return GetRuleContext<ExpressionContext>(i);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode OR() { return GetToken(CraterParser.OR, 0); }
		public OrOperationContext(ExpressionContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			ICraterParserListener typedListener = listener as ICraterParserListener;
			if (typedListener != null) typedListener.EnterOrOperation(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			ICraterParserListener typedListener = listener as ICraterParserListener;
			if (typedListener != null) typedListener.ExitOrOperation(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ICraterParserVisitor<TResult> typedVisitor = visitor as ICraterParserVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitOrOperation(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class LiteralExpressionContext : ExpressionContext {
		[System.Diagnostics.DebuggerNonUserCode] public LiteralContext literal() {
			return GetRuleContext<LiteralContext>(0);
		}
		public LiteralExpressionContext(ExpressionContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			ICraterParserListener typedListener = listener as ICraterParserListener;
			if (typedListener != null) typedListener.EnterLiteralExpression(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			ICraterParserListener typedListener = listener as ICraterParserListener;
			if (typedListener != null) typedListener.ExitLiteralExpression(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ICraterParserVisitor<TResult> typedVisitor = visitor as ICraterParserVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitLiteralExpression(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class AndOperationContext : ExpressionContext {
		[System.Diagnostics.DebuggerNonUserCode] public ExpressionContext[] expression() {
			return GetRuleContexts<ExpressionContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public ExpressionContext expression(int i) {
			return GetRuleContext<ExpressionContext>(i);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode AND() { return GetToken(CraterParser.AND, 0); }
		public AndOperationContext(ExpressionContext context) { CopyFrom(context); }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			ICraterParserListener typedListener = listener as ICraterParserListener;
			if (typedListener != null) typedListener.EnterAndOperation(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			ICraterParserListener typedListener = listener as ICraterParserListener;
			if (typedListener != null) typedListener.ExitAndOperation(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ICraterParserVisitor<TResult> typedVisitor = visitor as ICraterParserVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitAndOperation(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public ExpressionContext expression() {
		return expression(0);
	}

	private ExpressionContext expression(int _p) {
		ParserRuleContext _parentctx = Context;
		int _parentState = State;
		ExpressionContext _localctx = new ExpressionContext(Context, _parentState);
		ExpressionContext _prevctx = _localctx;
		int _startState = 10;
		EnterRecursionRule(_localctx, 10, RULE_expression, _p);
		try {
			int _alt;
			EnterOuterAlt(_localctx, 1);
			{
			State = 46;
			ErrorHandler.Sync(this);
			switch (TokenStream.LA(1)) {
			case LPAREN:
				{
				_localctx = new ParenthesizedExpressionContext(_localctx);
				Context = _localctx;
				_prevctx = _localctx;

				State = 41;
				Match(LPAREN);
				State = 42;
				expression(0);
				State = 43;
				Match(RPAREN);
				}
				break;
			case NUMBER:
			case HEXADECIMAL:
			case EXPONENTIAL:
			case STRING:
			case BOOLEAN:
				{
				_localctx = new LiteralExpressionContext(_localctx);
				Context = _localctx;
				_prevctx = _localctx;
				State = 45;
				literal();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			Context.Stop = TokenStream.LT(-1);
			State = 56;
			ErrorHandler.Sync(this);
			_alt = Interpreter.AdaptivePredict(TokenStream,6,Context);
			while ( _alt!=2 && _alt!=global::Antlr4.Runtime.Atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( ParseListeners!=null )
						TriggerExitRuleEvent();
					_prevctx = _localctx;
					{
					State = 54;
					ErrorHandler.Sync(this);
					switch ( Interpreter.AdaptivePredict(TokenStream,5,Context) ) {
					case 1:
						{
						_localctx = new AndOperationContext(new ExpressionContext(_parentctx, _parentState));
						PushNewRecursionContext(_localctx, _startState, RULE_expression);
						State = 48;
						if (!(Precpred(Context, 3))) throw new FailedPredicateException(this, "Precpred(Context, 3)");
						State = 49;
						Match(AND);
						State = 50;
						expression(4);
						}
						break;
					case 2:
						{
						_localctx = new OrOperationContext(new ExpressionContext(_parentctx, _parentState));
						PushNewRecursionContext(_localctx, _startState, RULE_expression);
						State = 51;
						if (!(Precpred(Context, 2))) throw new FailedPredicateException(this, "Precpred(Context, 2)");
						State = 52;
						Match(OR);
						State = 53;
						expression(3);
						}
						break;
					}
					} 
				}
				State = 58;
				ErrorHandler.Sync(this);
				_alt = Interpreter.AdaptivePredict(TokenStream,6,Context);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			UnrollRecursionContexts(_parentctx);
		}
		return _localctx;
	}

	public partial class LiteralContext : ParserRuleContext {
		public IToken number;
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode NUMBER() { return GetToken(CraterParser.NUMBER, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode HEXADECIMAL() { return GetToken(CraterParser.HEXADECIMAL, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode EXPONENTIAL() { return GetToken(CraterParser.EXPONENTIAL, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode STRING() { return GetToken(CraterParser.STRING, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode BOOLEAN() { return GetToken(CraterParser.BOOLEAN, 0); }
		public LiteralContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_literal; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			ICraterParserListener typedListener = listener as ICraterParserListener;
			if (typedListener != null) typedListener.EnterLiteral(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			ICraterParserListener typedListener = listener as ICraterParserListener;
			if (typedListener != null) typedListener.ExitLiteral(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			ICraterParserVisitor<TResult> typedVisitor = visitor as ICraterParserVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitLiteral(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public LiteralContext literal() {
		LiteralContext _localctx = new LiteralContext(Context, State);
		EnterRule(_localctx, 12, RULE_literal);
		int _la;
		try {
			State = 62;
			ErrorHandler.Sync(this);
			switch (TokenStream.LA(1)) {
			case NUMBER:
			case HEXADECIMAL:
			case EXPONENTIAL:
				EnterOuterAlt(_localctx, 1);
				{
				State = 59;
				_localctx.number = TokenStream.LT(1);
				_la = TokenStream.LA(1);
				if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 14680064L) != 0)) ) {
					_localctx.number = ErrorHandler.RecoverInline(this);
				}
				else {
					ErrorHandler.ReportMatch(this);
				    Consume();
				}
				}
				break;
			case STRING:
				EnterOuterAlt(_localctx, 2);
				{
				State = 60;
				Match(STRING);
				}
				break;
			case BOOLEAN:
				EnterOuterAlt(_localctx, 3);
				{
				State = 61;
				Match(BOOLEAN);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public override bool Sempred(RuleContext _localctx, int ruleIndex, int predIndex) {
		switch (ruleIndex) {
		case 5: return expression_sempred((ExpressionContext)_localctx, predIndex);
		}
		return true;
	}
	private bool expression_sempred(ExpressionContext _localctx, int predIndex) {
		switch (predIndex) {
		case 0: return Precpred(Context, 3);
		case 1: return Precpred(Context, 2);
		}
		return true;
	}

	private static int[] _serializedATN = {
		4,1,52,65,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,6,1,0,
		1,0,1,0,1,1,5,1,19,8,1,10,1,12,1,22,9,1,1,2,1,2,1,3,3,3,27,8,3,1,3,1,3,
		1,3,1,3,3,3,33,8,3,1,3,1,3,3,3,37,8,3,1,4,1,4,1,5,1,5,1,5,1,5,1,5,1,5,
		3,5,47,8,5,1,5,1,5,1,5,1,5,1,5,1,5,5,5,55,8,5,10,5,12,5,58,9,5,1,6,1,6,
		1,6,3,6,63,8,6,1,6,0,1,10,7,0,2,4,6,8,10,12,0,2,2,0,1,1,26,26,1,0,21,23,
		66,0,14,1,0,0,0,2,20,1,0,0,0,4,23,1,0,0,0,6,26,1,0,0,0,8,38,1,0,0,0,10,
		46,1,0,0,0,12,62,1,0,0,0,14,15,3,2,1,0,15,16,5,0,0,1,16,1,1,0,0,0,17,19,
		3,4,2,0,18,17,1,0,0,0,19,22,1,0,0,0,20,18,1,0,0,0,20,21,1,0,0,0,21,3,1,
		0,0,0,22,20,1,0,0,0,23,24,3,6,3,0,24,5,1,0,0,0,25,27,5,2,0,0,26,25,1,0,
		0,0,26,27,1,0,0,0,27,28,1,0,0,0,28,29,5,26,0,0,29,30,5,48,0,0,30,32,3,
		8,4,0,31,33,5,40,0,0,32,31,1,0,0,0,32,33,1,0,0,0,33,36,1,0,0,0,34,35,5,
		27,0,0,35,37,3,10,5,0,36,34,1,0,0,0,36,37,1,0,0,0,37,7,1,0,0,0,38,39,7,
		0,0,0,39,9,1,0,0,0,40,41,6,5,-1,0,41,42,5,42,0,0,42,43,3,10,5,0,43,44,
		5,43,0,0,44,47,1,0,0,0,45,47,3,12,6,0,46,40,1,0,0,0,46,45,1,0,0,0,47,56,
		1,0,0,0,48,49,10,3,0,0,49,50,5,6,0,0,50,55,3,10,5,4,51,52,10,2,0,0,52,
		53,5,7,0,0,53,55,3,10,5,3,54,48,1,0,0,0,54,51,1,0,0,0,55,58,1,0,0,0,56,
		54,1,0,0,0,56,57,1,0,0,0,57,11,1,0,0,0,58,56,1,0,0,0,59,63,7,1,0,0,60,
		63,5,24,0,0,61,63,5,25,0,0,62,59,1,0,0,0,62,60,1,0,0,0,62,61,1,0,0,0,63,
		13,1,0,0,0,8,20,26,32,36,46,54,56,62
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
} // namespace Core.Antlr
