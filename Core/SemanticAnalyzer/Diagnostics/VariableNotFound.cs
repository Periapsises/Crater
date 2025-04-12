using Core.SyntaxTreeConverter;

namespace Core.SemanticAnalyzer.Diagnostics;

public class VariableNotFound : ErrorDiagnostic
{
    public VariableNotFound(VariableReference reference)
    {
        Message = Format("Cannot resolve variable name '{0}'", reference);
    }
}