namespace Core.SemanticAnalyzer.Diagnostics;

public abstract class RedundantCondition: InfoDiagnostic
{
    protected RedundantCondition(string text)
    {
        Message = text;
    }
}

public class AndAlwaysTrue() : RedundantCondition("Left side of 'and' operation is always true") {}
public class AndAlwaysFalse() : RedundantCondition("Left side of 'and' operation is always false") {}
public class OrAlwaysTrue() : RedundantCondition("Left side of 'or' operation is always true") {}
public class OrAlwaysFalse() : RedundantCondition("Left side of 'or' operation is always false") {}

public class ConditionAlwaysTrue() : RedundantCondition("Condition is always true") {}
public class ConditionAlwaysFalse() : RedundantCondition("Condition is always false") {}
