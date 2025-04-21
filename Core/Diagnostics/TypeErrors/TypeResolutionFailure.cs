using Core.SyntaxTreeConverter;

namespace Core.Diagnostics.TypeErrors;

public class TypeResolutionFailure(Expression expression)
    : Diagnostic("CRA208", "The result or '{0}' is ambiguous and could not be resolved to a single type.",
        Severity.Error)
{
    public override string[] args => [expression.FullString];
}