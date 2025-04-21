using Antlr4.Runtime;

namespace Core.SyntaxTreeConverter;

public abstract class TypeReference(bool nullable)
{
    public readonly bool Nullable = nullable;
}

public class ExpressionTypeReference(Expression expression, bool nullable, ExpressionTypeCtx context)
    : TypeReference(nullable)
{
    public readonly ExpressionTypeCtx Context = context;
    public readonly Expression Expression = expression;
}

public class FunctionTypeReference(bool nullable, FunctionLiteralCtx context) : TypeReference(nullable)
{
    public readonly FunctionLiteralCtx Context = context;
}

public class FuncTypeReference(
    List<TypeReference> parameters,
    List<TypeReference> returns,
    bool nullable,
    ParserRuleContext context)
    : TypeReference(nullable)
{
    public readonly ParserRuleContext Context = context;
    public readonly List<TypeReference> Parameters = parameters;
    public readonly List<TypeReference> Returns = returns;
}