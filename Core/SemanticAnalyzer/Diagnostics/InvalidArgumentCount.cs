namespace Core.SemanticAnalyzer.Diagnostics;

public class InvalidArgumentCount : ErrorDiagnostic
{
    public InvalidArgumentCount(int expected, int actual)
    {
        Message = Format("Invalid argument count. Expected {0} but got {1}", expected, actual);
        WithContext(Context.CurrentContext!);
    }
}