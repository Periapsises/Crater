namespace Core.SemanticAnalyzer.Diagnostics;

public class InvalidArgumentType : ErrorDiagnostic
{
    public InvalidArgumentType(DataType source, DataType target, int argumentIndex)
    {
        Message = Format("Type mismatch in argument {0}: Cannot convert from '{1}' to '{2}' ", argumentIndex + 1, source, target);

        switch (Context.CurrentContext)
        {
            case FunctionCallStatementCtx context:
                WithContext(context.functionArguments().expression()[argumentIndex]);
                break;
            case FunctionCallCtx context:
                WithContext(context.functionArguments().expression()[argumentIndex]);
                break;
        }
    }
}