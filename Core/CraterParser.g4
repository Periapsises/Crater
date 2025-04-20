// $antlr-format alignTrailingComments true, columnLimit 150, maxEmptyLinesToKeep 1, reflowComments false, useTab false
// $antlr-format allowShortRulesOnASingleLine true, allowShortBlocksOnASingleLine true, alignSemicolons ownLine, alignColons trailing

parser grammar CraterParser;

options {
    tokenVocab = CraterLexer;
}

program: block EOF;

block: statement*;

statement:
    variableDeclaration
    | functionDeclaration
    | ifStatement
    | functionCallStatement
;

variableDeclaration: LOCAL? name=IDENTIFIER COLON type=dataType (ASSIGN initializer=expression)?;

functionDeclaration: LOCAL? FUNCTION name=IDENTIFIER LPAREN functionParameters? RPAREN COLON returnType=functionReturnTypes block END;

functionParameters: functionParameter (COMMA functionParameter)*;

functionParameter: name=IDENTIFIER COLON type=dataType;

functionReturnTypes: VOID | dataType (COMMA dataType)*;

ifStatement: IF condition=expression THEN block elseIfStatement* elseStatement? END;

elseIfStatement: ELSEIF condition=expression THEN block;

elseStatement: ELSE block;

functionCallStatement: primaryExpression LPAREN functionArguments? RPAREN;

functionArguments: expression (COMMA expression)*; 

genericParameters: LESS dataType (COMMA dataType)* GREATER;

dataType:
    FUNCTION genericParameters? nullable=QMARK?                                                         # FunctionLiteral
    | FUNC LPAREN (dataType (COMMA dataType)*)? RPAREN COLON functionReturnTypes                        # FuncLiteral
    | LPAREN FUNC LPAREN (dataType (COMMA dataType)*)? RPAREN COLON functionReturnTypes RPAREN QMARK    # NullableFuncLiteral
    | expression genericParameters? nullable=QMARK?                                                     # ExpressionType
;

expression:
    primaryExpression                                                                               # BaseExpression
    | MINUS expression                                                                              # UnaryOperation
    | expression EXP expression                                                                     # ExponentOperation
    | expression op=(MUL | DIV | MOD) expression                                                    # MultiplicativeOperation
    | expression op=(PLUS | MINUS) expression                                                       # AdditiveOperation
    | expression CONCAT expression                                                                  # ConcatenationOperation
    | expression op=(LESS | LESS_EQUAL | GREATER | GREATER_EQUAL | NOT_EQUAL | EQUAL) expression    # LogicalOperation
    | expression AND expression                                                                     # AndOperation
    | expression OR expression                                                                      # OrOperation
    | literal                                                                                       # LiteralExpression
;

primaryExpression: prefixExpression postfixExpression*;

prefixExpression:
    LPAREN expression RPAREN    # ParenthesizedExpression
    | IDENTIFIER                # VariableReference
;

postfixExpression:
    DOT IDENTIFIER                          # DotIndexing
    | LSQRBRACKET expression RSQRBRACKET    # BracketIndexing
    | LPAREN functionArguments? RPAREN      # FunctionCall
;

literal:
    number=(NUMBER | HEXADECIMAL | EXPONENTIAL | BINARY)
    | STRING
    | BOOLEAN
;
