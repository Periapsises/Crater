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

variableDeclaration: LOCAL? IDENTIFIER COLON typeName QMARK? (ASSIGN expression)?;

functionDeclaration: LOCAL? FUNCTION IDENTIFIER LPAREN functionParameters RPAREN COLON typeName QMARK? block END;

functionParameters: functionParameter (COMMA functionParameter)*;

functionParameter: IDENTIFIER COLON typeName QMARK?;

typeName: IDENTIFIER;

expression:
    LPAREN expression RPAREN    # ParenthesizedExpression
    | expression AND expression # AndOperation
    | expression OR expression  # OrOperation
    | IDENTIFIER                # VariableReference
    | literal                   # LiteralExpression
;

literal:
    number = (NUMBER | HEXADECIMAL | EXPONENTIAL)
    | STRING
    | BOOLEAN
;
