namespace Core.SemanticAnalyzer.Diagnostics;

public class InvalidUnaryOperator : ErrorDiagnostic
{
    public InvalidUnaryOperator(DataType dataType, string op)
    {
        Message = Format("Cannot apply operator '{0}' to operand of type '{1}'", op, dataType);
    }
}