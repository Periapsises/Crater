namespace Core.SyntaxTreeConverter.Expressions;

public class FunctionCall(List<Expression> arguments, FunctionCallCtx context) : Expression(context.GetText())
{
    public readonly List<Expression> Arguments = arguments;
    public readonly FunctionCallCtx Context = context;
}