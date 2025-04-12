namespace Core.SemanticAnalyzer.Diagnostics;

public class InvalidBinaryOperator : ErrorDiagnostic
{
    public InvalidBinaryOperator(DataType left, DataType right, string op)
    {
        Message = Format("Cannot apply operator '\u001b[96m" + op + "\u001b[0m' to operands of type '%s' and '%s'", left, right);
    }
}