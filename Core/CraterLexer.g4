// $antlr-format alignTrailingComments true, columnLimit 150, maxEmptyLinesToKeep 1, reflowComments false, useTab false
// $antlr-format allowShortRulesOnASingleLine true, allowShortBlocksOnASingleLine true, minEmptyLines 0, alignSemicolons ownLine
// $antlr-format alignColons trailing, singleLineOverrulesHangingColon true, alignLexerCommands true, alignLabels true, alignTrailers true

lexer grammar CraterLexer;

options {
    superClass = CraterLexerBase;
}

// Lua keywords
FUNCTION : 'function';
LOCAL    : 'local';
RETURN   : 'return';
END      : 'end';
NOT      : 'not';
AND      : 'and';
OR       : 'or';
IF       : 'if';
ELSEIF   : 'elseif';
ELSE     : 'else';
THEN     : 'then';
WHILE    : 'while';
FOR      : 'for';
DO       : 'do';
IN       : 'in';
REPEAT   : 'repeat';
UNTIL    : 'until';

// Crater specific keywords
CLASS  : 'class';
STATIC : 'static';
NEW    : 'new';
VOID   : 'void';
FUNC   : 'func';

// Literals
NUMBER      : Integer Decimal?;
HEXADECIMAL : '0' [xX] [a-fA-F0-9]+;
EXPONENTIAL : Integer Decimal? 'e' Integer;
BINARY      : '0' [bB] [01]+;

fragment Integer : [1-9][0-9]* | '0';
fragment Decimal : '.' [0-9]+;

STRING: '"' ( EscapeSequence | ~('\\' | '"'))* '"';

fragment EscapeSequence:
    '\\' [abfnrtvz"'|$#\\]
    | '\\' '\r'? '\n'
    | '\\' Integer
    | '\\x' [a-fA-F0-9]+
    | '\\u{' [a-fA-F0-9]+ '}'
;

BOOLEAN: 'true' | 'false';

IDENTIFIER: [a-zA-Z_][a-zA-Z0-9_]*;

ASSIGN: '=';

LESS          : '<';
LESS_EQUAL    : '<=';
GREATER       : '>';
GREATER_EQUAL : '>=';
EQUAL         : '==';
NOT_EQUAL     : '~=';

PLUS   : '+';
MINUS  : '-';
MUL    : '*';
DIV    : '/';
MOD    : '%';
EXP    : '^';
QMARK  : '?';
CONCAT : '..';

LPAREN      : '(';
RPAREN      : ')';
LBRACKET    : '{';
RBRACKET    : '}';
LSQRBRACKET : '[';
RSQRBRACKET : ']';

COLON : ':';
COMMA : ',';
DOT   : '.';

COMMENT: '--' { this.HandleComment(); } -> channel(HIDDEN);

WHITESPACE: (' ' | '\t' | '\n' | '\r')+ -> channel(HIDDEN);
