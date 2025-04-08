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
;

variableDeclaration: LOCAL? IDENTIFIER COLON expression QMARK? (ASSIGN expression)?;

functionDeclaration: LOCAL? FUNCTION IDENTIFIER LPAREN functionParameters RPAREN COLON expression QMARK? block END;

functionParameters: functionParameter (COMMA functionParameter)*;

functionParameter: IDENTIFIER COLON expression QMARK?;

expression:
    LPAREN expression RPAREN                                                                        # ParenthesizedExpression
    | MINUS expression                                                                              # UnaryOperation
    | expression EXP expression                                                                     # ExponentOperation
    | expression op=(MUL | DIV | MOD) expression                                                    # MultiplicativeOperation
    | expression op=(PLUS | MINUS) expression                                                       # AdditiveOperation
    | expression CONCAT expression                                                                  # ConcatenationOperation
    | expression op=(LESS | LESS_EQUAL | GREATER | GREATER_EQUAL | NOT_EQUAL | EQUAL) expression    # LogicalOperation
    | expression AND expression                                                                     # AndOperation
    | expression OR expression                                                                      # OrOperation
    | IDENTIFIER                                                                                    # VariableReference
    | literal                                                                                       # LiteralExpression
;

literal:
    number = (NUMBER | HEXADECIMAL | EXPONENTIAL | BINARY)
    | STRING
    | BOOLEAN
;
