namespace Core.SemanticAnalyzer.Diagnostics;

public class UnresolvableValue : ErrorDiagnostic
{
    public UnresolvableValue(PossibleValues possibleValues)
    {
        if (possibleValues.Count == 0)
            Message = Format("Could not determine values from expression");
        else
            Message = Format("Cannot resolve values to a common type");
    }
}