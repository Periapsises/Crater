namespace Core.SemanticAnalyzer.Diagnostics;

public class InvalidBinaryOperator : ErrorDiagnostic
{
    public InvalidBinaryOperator(DataType left, DataType right, string op)
    {
        Message = Format("Cannot apply operator '{0}' to operands of type '{1}' and '{2}'", op, left, right);
    }
}