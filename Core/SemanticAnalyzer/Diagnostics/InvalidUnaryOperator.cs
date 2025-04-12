namespace Core.SemanticAnalyzer.Diagnostics;

public class InvalidUnaryOperator : ErrorDiagnostic
{
    public InvalidUnaryOperator(DataType self, string op)
    {
        Message = Format("Cannot apply operator '\u001b[96m" + op + "\u001b[0m' to operand of type '{0}'", self);
    }
}