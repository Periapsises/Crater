namespace Core.SemanticAnalyzer.Diagnostics;

public class VariableShadowing: WarningDiagnostic
{
    public VariableShadowing(string variableName)
    {
        Message = Format("Variable '{0}' shadows existing binding", variableName);
    }
}