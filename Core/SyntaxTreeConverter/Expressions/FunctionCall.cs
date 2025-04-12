using Core.Antlr;

namespace Core.SyntaxTreeConverter.Expressions;

public class FunctionCall(List<Expression> arguments, CraterParser.FunctionCallContext context) : Expression
{
    public readonly List<Expression> Arguments = arguments;
    public readonly CraterParser.FunctionCallContext Context = context;
}