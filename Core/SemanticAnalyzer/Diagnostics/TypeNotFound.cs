using Core.SyntaxTreeConverter;

namespace Core.SemanticAnalyzer.Diagnostics;

public class TypeNotFound : ErrorDiagnostic
{
    public TypeNotFound(Expression reference)
    {
        Message = Format("Cannot resolve type name '{0}'", reference);
    }
}