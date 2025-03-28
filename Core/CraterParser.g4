// $antlr-format alignTrailingComments true, columnLimit 150, maxEmptyLinesToKeep 1, reflowComments false, useTab false
// $antlr-format allowShortRulesOnASingleLine true, allowShortBlocksOnASingleLine true, alignSemicolons ownLine, alignColons trailing

parser grammar CraterParser;

options {
    tokenVocab = CraterLexer;
}

program: block EOF;

block: statement*;

statement: variableDeclaration;

variableDeclaration: LOCAL IDENTIFIER COLON typeName (ASSIGN expression)?;

typeName: FUNCTION | IDENTIFIER;

expression: literal;

literal:
    number = (NUMBER | HEXADECIMAL | EXPONENTIAL)
    | STRING
    | BOOLEAN
;
