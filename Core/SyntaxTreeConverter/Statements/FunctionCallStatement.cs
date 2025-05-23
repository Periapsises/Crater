﻿namespace Core.SyntaxTreeConverter.Statements;

public class FunctionCallStatement(
    Expression primaryExpression,
    List<Expression> arguments,
    FunctionCallStatementCtx context) : Statement
{
    public readonly List<Expression> Arguments = arguments;

    public readonly FunctionCallStatementCtx Context = context;
    public readonly Expression PrimaryExpression = primaryExpression;
}