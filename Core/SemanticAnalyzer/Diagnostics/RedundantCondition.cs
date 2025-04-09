namespace Core.SemanticAnalyzer.Diagnostics;

public abstract class RedundantCondition(string text): Diagnostic(Severity.Info)
{
    public override string GetMessage()
    {
        var message = $"{Info}{text}{GetLocation()}";
        
        if (Code != string.Empty)
            message += $"\n{GetCodeLocation()}";
        
        return message;
    }
}

public class AndAlwaysTrue() : RedundantCondition("Left side of 'and' operation is always true") {}
public class AndAlwaysFalse() : RedundantCondition("Left side of 'and' operation is always false") {}
public class OrAlwaysTrue() : RedundantCondition("Left side of 'or' operation is always true") {}
public class OrAlwaysFalse() : RedundantCondition("Left side of 'or' operation is always false") {}

public class ConditionAlwaysTrue() : RedundantCondition("Condition is always true") {}
